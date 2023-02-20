using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeviyeAyarla : MonoBehaviour
{
    public TextMeshProUGUI seviyeText;
    public int seviye = 1;
    public int alemNo = 0;

    private OyunVerisi oyunverisi;
    public GameObject onayPaneli;

    private void Start()
    {
        oyunverisi = FindObjectOfType<OyunVerisi>();
        VeriyiYukle();
        SeviyeyiGoster();
    }

    public void SeviyeyiGoster()
    {
        if (oyunverisi.veriKaydet.oyununDili == Dil.Turkce)
        {
           seviyeText.text = "seviye " + seviye;
        }
        else
        {
           seviyeText.text = "level " + seviye;
        }
    }


    void VeriyiYukle()
    {
        int i = 0;
        if (oyunverisi != null)
        {

            while (oyunverisi.veriKaydet.aktifMi[alemNo,i])
            {
                i++;
            }

            seviye = i;
        }

        onayPaneli.GetComponent<OnayPaneli>().seviye = seviye;
    }
}
