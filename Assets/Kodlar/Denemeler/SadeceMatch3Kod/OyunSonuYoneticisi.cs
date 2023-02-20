using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum OyunKosulTuru { hareketHakki, sure}

[System.Serializable]
public class OyunSonuKosulu
{
    public OyunKosulTuru kosulTuru;
    public int sayacDegeri;
}

public class OyunSonuYoneticisi : MonoBehaviour
{
    public GameObject hakEtiketi;
    public GameObject sureEtiketi;
    public GameObject kazandinPaneli;
    public GameObject kaybettinPaneli;
    public Text sayac;
    public OyunSonuKosulu kosullar;
    private Tahta tahta;
   
    private float zamanlayiciSaniyesi;

    [HideInInspector]
    public int suankiSayacDegeri;


    // Start is called before the first frame update
    void Start()
    {
        tahta = FindObjectOfType<Tahta>();
        OyunTurunuKur();
        OyunuKur();
    }

    void OyunTurunuKur()
    {
        if (tahta.alem != null)
        {
            if (tahta.seviye < tahta.alem.seviyeler.Length)
            {
                if (tahta.alem.seviyeler[tahta.seviye] != null)
                {
                    kosullar = tahta.alem.seviyeler[tahta.seviye].oyunSonuKosulu;
                }
            }
        }
    }

    void OyunuKur()
    {
        suankiSayacDegeri = kosullar.sayacDegeri;
        if(kosullar.kosulTuru== OyunKosulTuru.hareketHakki)
        {
            hakEtiketi.SetActive(true);
            sureEtiketi.SetActive(false);
        }
        else
        {
            zamanlayiciSaniyesi = 1;
            hakEtiketi.SetActive(false);
            sureEtiketi.SetActive(true);
        }
        sayac.text = "" + suankiSayacDegeri;
    }

    public void SayaciAzalt()
    {
        if (tahta.suankiDurum != OyunDurumu2.durdur)
        {
            suankiSayacDegeri--;
            sayac.text = "" + suankiSayacDegeri;

            if (suankiSayacDegeri <= 0)
            {
                OyunuKaybet();
            }
        }
    }

    public void OyunuKazan() // !!!!!!!!!!!!!! Burda Hata Var: kazandın enum moduna geçmiyor çünkü geçer geçmez hareket moduna geçiyor
    {
        kazandinPaneli.SetActive(true);
        tahta.suankiDurum = OyunDurumu2.kazandin;
        sayac.text = "" + suankiSayacDegeri;
        GirisPanelKontrolcusu girisPanel = FindObjectOfType<GirisPanelKontrolcusu>();
        girisPanel.OyunBitti();
    }

    public void OyunuKaybet()
    {
        kaybettinPaneli.SetActive(true);
        tahta.suankiDurum = OyunDurumu2.kaybettin;
        Debug.Log("kaybettin!");
        suankiSayacDegeri = 0;
        sayac.text = "" + suankiSayacDegeri;
        GirisPanelKontrolcusu girisPanel = FindObjectOfType<GirisPanelKontrolcusu>();
        girisPanel.OyunBitti();
    }

    // Update is called once per frame
    void Update()
    {
        if (kosullar.kosulTuru == OyunKosulTuru.sure && suankiSayacDegeri > 0 && tahta.suankiDurum == OyunDurumu2.hareket)
        {
            zamanlayiciSaniyesi -= Time.deltaTime;
            if (zamanlayiciSaniyesi <= 0)
            {
                SayaciAzalt();
                zamanlayiciSaniyesi = 1;
            }
        }
    }
}
