using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnayPaneli : MonoBehaviour
{
    public string yuklencekSeviye;
    public string kullanılacakKayıt;
    public int seviye;
    public int alemNo=0;

    public GameObject geriDonPanel;
    public GameObject tekrarDenePaneli;

    public void Iptal()
    {
        this.gameObject.SetActive(false);
    }
    public void Oyna()
    {
        PlayerPrefs.SetString("seviyeKayitAdi", kullanılacakKayıt);
        PlayerPrefs.SetInt(kullanılacakKayıt, seviye - 1);

        PlayerPrefs.SetInt("alemNo", alemNo);

        SceneManager.LoadScene(yuklencekSeviye);
    }

    public void GeriDonPaneliAc()
    {
        geriDonPanel.SetActive(true);
    }

    public void GeriDonPaneliKapat()
    {
        geriDonPanel.SetActive(false);
    }

    public void TekrarDenePaneliAc()
    {
       tekrarDenePaneli.SetActive(true);
    }

    public void TekrarDenePaneliKapat()
    {
        tekrarDenePaneli.SetActive(false);
    }
    public void AnaEkranaDon()
    {
        SceneManager.LoadScene("AnaEkran");
    }
}
