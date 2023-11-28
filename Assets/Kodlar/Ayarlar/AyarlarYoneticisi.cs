using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AyarlarYoneticisi : MonoBehaviour
{

    public DilDegistir dilDegistir;
    public SeviyeAyarla seviyeAyarla;

    private bool AyarlariAc = false;
    public Animator ayarlar;
    private void Start()
    {
        dilDegistir = FindObjectOfType<DilDegistir>();
        seviyeAyarla = FindObjectOfType<SeviyeAyarla>();
    }

    public void Cevir(int a)
    {
        dilDegistir.Cevir(a);
        seviyeAyarla.SeviyeyiGoster();
        AyarlariAcKapat();
    }

    public void AyarlariAcKapat()
    {
        AyarlariAc = !AyarlariAc;

        if (AyarlariAc)
        {
            ayarlar.SetBool("AyarlarAc", true);
        }
        else
        {
            ayarlar.SetBool("AyarlarAc", false);
        }
    }
}
