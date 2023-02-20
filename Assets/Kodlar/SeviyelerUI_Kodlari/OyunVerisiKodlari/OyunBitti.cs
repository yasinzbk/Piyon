using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OyunBitti : MonoBehaviour
{
    private OyunVerisi oyunVerisi;
    public int sonAlemNo;

    public GameObject oyunSonuPaneli;


    
    private void Start()
    {

        PlayerPrefs.SetInt("SonDunyaSayisi", sonAlemNo);
        oyunVerisi = FindObjectOfType<OyunVerisi>();
    }

    void Update()
    {
        //if (oyunVerisi.veriKaydet.aktifMi[0, oyunVerisi.veriKaydet.aktifMi.GetLength(1) - 1])
        //{
        //    oyunSonuPaneli.SetActive(true);
        //}

        if (oyunVerisi.veriKaydet.aktifMi[1, 88])
        {
            oyunSonuPaneli.SetActive(true);
        }


    }
}
