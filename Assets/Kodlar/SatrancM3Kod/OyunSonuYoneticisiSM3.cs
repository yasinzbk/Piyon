using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum OyunKosulTuruSM3 { hareketHakki, sure }

[System.Serializable]
public class OyunSonuKosuluSM3
{
    public OyunKosulTuruSM3 kosulTuru;
    public int sayacDegeri;
}

public class OyunSonuYoneticisiSM3 : MonoBehaviour
{
    public GameObject hakEtiketi;
    public GameObject sureEtiketi;
    public GameObject kazandinPaneli;
    public GameObject kaybettinPaneli;
    public TextMeshProUGUI sayac;
    public OyunSonuKosuluSM3 kosullar;
    private M3Tahta tahta;

    private float zamanlayiciSaniyesi;

    [HideInInspector]
    public int suankiSayacDegeri;


    // Start is called before the first frame update
    void Start()
    {
        tahta = FindObjectOfType<M3Tahta>();
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
        if (kosullar.kosulTuru == OyunKosulTuruSM3.hareketHakki)
        {
            hakEtiketi.SetActive(true);
            sureEtiketi.SetActive(false);
        }
        else  // sureli bolum oluyor
        {
            zamanlayiciSaniyesi = 1;
            hakEtiketi.SetActive(false);
            sureEtiketi.SetActive(true);
        }
        sayac.text = "" + suankiSayacDegeri;
    }

    public void SayaciAzalt()
    {
        if (tahta.suankiDurum != OyunDurumu.durdur)
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
        tahta.suankiDurum = OyunDurumu.kazandin;
        sayac.text = "" + suankiSayacDegeri;
        PanelAnimKontrol girisPanel = FindObjectOfType<PanelAnimKontrol>();
        girisPanel.OyunBitti();

        SeviyeSecimSahnesineDon seviyeSecim = gameObject.GetComponent<SeviyeSecimSahnesineDon>();
        seviyeSecim.VeriGuncelle();

        OnayPaneli onayPaneli = FindObjectOfType<OnayPaneli>();
        onayPaneli.SeviyeKaydet();
    }

    public void OyunuKaybet()
    {
        kaybettinPaneli.SetActive(true);
        tahta.suankiDurum = OyunDurumu.kaybettin;
        Debug.Log("kaybettin!");
        suankiSayacDegeri = 0;
        sayac.text = "" + suankiSayacDegeri;
        PanelAnimKontrol girisPanel = FindObjectOfType<PanelAnimKontrol>();
        girisPanel.OyunBitti();
    }

    // Update is called once per frame
    void Update()
    {
        if (kosullar.kosulTuru == OyunKosulTuruSM3.sure && suankiSayacDegeri > 0 && tahta.suankiDurum == OyunDurumu.hareket)
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
