using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AyarlarYoneticisi : MonoBehaviour
{

    public DilDegistir dilDegistir;
    public SeviyeAyarla seviyeAyarla;
    private AyarlarButon ayarlarPanel;
    private void Start()
    {
        dilDegistir = FindObjectOfType<DilDegistir>();
        seviyeAyarla = FindObjectOfType<SeviyeAyarla>();
        ayarlarPanel = FindObjectOfType<AyarlarButon>();
    }

    public void Cevir(int a)
    {
        dilDegistir.Cevir(a);
        seviyeAyarla.SeviyeyiGoster();
        ayarlarPanel.AyarlariAcKapat();
    }

}
