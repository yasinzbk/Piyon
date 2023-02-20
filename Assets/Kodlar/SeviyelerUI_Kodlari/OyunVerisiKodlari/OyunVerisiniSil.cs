using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class OyunVerisiniSil : MonoBehaviour
{
    private OyunVerisi oyunverisi;

    public GameObject veriSilPaneli;

    private void Start()
    {
        oyunverisi = FindObjectOfType<OyunVerisi>();

    }




    public void KayitVerisiniSil()
    {
       oyunverisi.veriKaydet = new VeriKaydet();
        //oyunverisi.veriKaydet.aktifMi = new bool[100, 10];
        oyunverisi.veriKaydet.aktifMi = new bool[2, 100];

        for (int i = 0; i < oyunverisi.veriKaydet.aktifMi.GetLength(0); i++)
        {
            oyunverisi.veriKaydet.aktifMi[i, 0] = true;
        }

        oyunverisi.veriKaydet.oyununDili = Dil.Ingilizce;
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
