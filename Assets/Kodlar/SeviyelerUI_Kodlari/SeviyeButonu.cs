using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeviyeButonu : MonoBehaviour
{
    public bool aktifMi;
    public Sprite aktifSprite;
    public Sprite kilitliSprite;
    private Image butonImage;
    private Button buButon;
    

    //public Image[] yildizlar;
    public Text seviyeText;
    public int seviye=1;
    public int alemNo =0;
    public GameObject onayPaneli;

    private OyunVerisi oyunverisi;

    private void Start()
    {
        oyunverisi = FindObjectOfType<OyunVerisi>();
        butonImage = GetComponent<Image>();
        buButon = GetComponent<Button>();
        VeriyiYukle();
        //YildizlariAktifEt();
        SeviyeyiGoster();
        SpriteKararVer();
    }

    void VeriyiYukle()
    {
        if (oyunverisi != null)
        {
            if(oyunverisi.veriKaydet.aktifMi[alemNo, seviye - 1])
            {
                aktifMi = true;
            }
            else
            {
                aktifMi = false;
            }
        }
    }

    //void YildizlariAktifEt()
    //{
    //    for (int i = 0; i < yildizlar.Length; i++)
    //    {
    //        yildizlar[i].enabled = false;
    //    }
    //}

    void SpriteKararVer()
    {
        if (aktifMi)
        {
            butonImage.sprite = aktifSprite;
            buButon.enabled = true;
            //seviyeText.enabled = true;
        }
        else
        {
            butonImage.sprite = kilitliSprite;
            buButon.enabled = false;
            //seviyeText.enabled = false;
        }
    }

    void SeviyeyiGoster()
    {
        seviyeText.text = "" + seviye;
    }

    public void OnayPaneli(int seviye)
    {
        onayPaneli.GetComponent<OnayPaneli>().seviye = seviye;
        onayPaneli.GetComponent<OnayPaneli>().alemNo = alemNo;
        onayPaneli.SetActive(true);
    }

    public void OnayPaneli(string seviye)
    {
        onayPaneli.GetComponent<OnayPaneli>().yuklencekSeviye = seviye;
        onayPaneli.SetActive(true);
    }
}
