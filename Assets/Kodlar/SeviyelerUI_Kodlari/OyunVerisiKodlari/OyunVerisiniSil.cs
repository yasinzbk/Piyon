using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class OyunVerisiniSil : MonoBehaviour
{

    public GameObject veriSilPaneli;

    public void KayitVerisiniSil()
    {
        //oyunverisi.veriKaydet = new VeriKaydet();
        // //oyunverisi.veriKaydet.aktifMi = new bool[100, 10];
        // oyunverisi.veriKaydet.aktifMi = new bool[2, 100];

        // for (int i = 0; i < oyunverisi.veriKaydet.aktifMi.GetLength(0); i++)
        // {
        //     oyunverisi.veriKaydet.aktifMi[i, 0] = true;
        // }

        // oyunverisi.veriKaydet.oyununDili = Dil.Ingilizce;

        SaveLoadManager.instance.DeleteProfileData("test"); //elle yazilmamasi daha iyi olur
        SaveLoadManager.instance.gameData = new SaveData(); // yeni kayit verisine geciyor

        SceneManager.LoadScene("AnaEkran");     //SceneManager.LoadScene("ModSecimEkrani");
    }

    public void VeriSilPaneliAc()
    {
        veriSilPaneli.SetActive(true);
    }

    public void VeriSilPaneliKapat()
    {
        veriSilPaneli.SetActive(false);
    }
}
