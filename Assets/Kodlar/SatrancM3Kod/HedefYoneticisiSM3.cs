using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BiHedefSM3
{
    public int gerekenMiktar;
    public int toplananMiktar;
    public Sprite hedefResmi;
    public Color hedefResimRengi = Color.white;
    public TasTuru eslesenTasTuru;
    public TasRengi eslesenTasRengi;
    public FayansTur eslenenFayansTuru;
    public bool Tiklanan2Mi;
}

public class HedefYoneticisiSM3 : MonoBehaviour
{
    public BiHedefSM3[] seviyeHedefleri;
    public List<HedefPaneli> suankiHedefler = new List<HedefPaneli>();
    public GameObject hedefSeriUretim;
    public GameObject hedefGirisPaneli;  //Giristeki Hedef Paneli
    public GameObject hedefOyundakiPanel;
    private M3Tahta tahta;
    private OyunSonuYoneticisiSM3 oyunSonuYoneticisi;

    private int tamamlananHedefSayisi;

    // Start is called before the first frame update
    void Start()
    {
        tahta = FindObjectOfType<M3Tahta>();
        oyunSonuYoneticisi = FindObjectOfType<OyunSonuYoneticisiSM3>();
        HedefleriAl();
        HedefleriYerlestir();

        tamamlananHedefSayisi = 0;
    }


    void HedefleriAl()
    {
        if (tahta != null)
        {
            if (tahta.alem != null)
            {
                if (tahta.seviye < tahta.alem.seviyeler.Length)
                {
                    if (tahta.alem.seviyeler[tahta.seviye] != null)
                    {
                        seviyeHedefleri = tahta.alem.seviyeler[tahta.seviye].seviyeHedefleri;
                        for (int i = 0; i < seviyeHedefleri.Length; i++)
                        {
                            seviyeHedefleri[i].toplananMiktar = 0;
                        }
                    }
                }
            }
        }
    }
    void HedefleriYerlestir()
    {
        for (int i = 0; i < seviyeHedefleri.Length; i++)
        {
            GameObject hedef = Instantiate(hedefSeriUretim, hedefGirisPaneli.transform.position, Quaternion.identity);  // Oyunun girişinde çıkan panelde hedefleri gösterir
            hedef.transform.SetParent(hedefGirisPaneli.transform, false);
            HedefPaneli panel = hedef.GetComponent<HedefPaneli>();
            panel.buSprite = seviyeHedefleri[i].hedefResmi;
            panel.buRenk = seviyeHedefleri[i].hedefResimRengi;
            panel.buString = "" + seviyeHedefleri[i].gerekenMiktar;


            GameObject oyunHedef = Instantiate(hedefSeriUretim, hedefOyundakiPanel.transform.position, Quaternion.identity);  // Oyun esnasında yukarıdaki panelde hedefleri gösterir
            oyunHedef.transform.SetParent(hedefOyundakiPanel.transform, false);
            panel = oyunHedef.GetComponent<HedefPaneli>();
            suankiHedefler.Add(panel);
            panel.buSprite = seviyeHedefleri[i].hedefResmi;
            panel.buRenk = seviyeHedefleri[i].hedefResimRengi;
            panel.buString = "" + seviyeHedefleri[i].gerekenMiktar;

        }
    }


    public void HedefVerileriGuncelle()  // Üst panelde gözüken O bölümün hedeflerini günceller
    {
        int hedefTamamlandi = 0;
        for (int i = 0; i < seviyeHedefleri.Length; i++)
        {
            suankiHedefler[i].buText.text = "" + (seviyeHedefleri[i].gerekenMiktar - seviyeHedefleri[i].toplananMiktar);

            if (seviyeHedefleri[i].toplananMiktar >= seviyeHedefleri[i].gerekenMiktar)
            {
                hedefTamamlandi++;
                suankiHedefler[i].buText.text = "";
                suankiHedefler[i].tamamlandiIsareti.SetActive(true);
            }

        }

        if (tamamlananHedefSayisi != hedefTamamlandi)
        {
            FindObjectOfType<SesYoneticisi>().Oynat("Tamamlandi");
            tamamlananHedefSayisi = hedefTamamlandi;
        }

        if (hedefTamamlandi >= seviyeHedefleri.Length)
        {
            if (oyunSonuYoneticisi != null)
            {
                oyunSonuYoneticisi.OyunuKazan();
                tahta.suankiDurum = OyunDurumu.kazandin; // 14/07/2022
            }
            Debug.Log("Kazandin!");
        }
    }


    public void HedefiKarsilastir(TasTuru HedefTasTuru, TasRengi HedefTasRengi)
    {
        for (int i = 0; i < seviyeHedefleri.Length; i++)
        {
            if(!(seviyeHedefleri[i].toplananMiktar >= seviyeHedefleri[i].gerekenMiktar)) // Tiklanan2 icin bu haldeydi: if(!(seviyeHedefleri[i].toplananMiktar >= seviyeHedefleri[i].gerekenMiktar) && !seviyeHedefleri[i].Tiklanan2Mi)
            {

                if (HedefTasTuru == seviyeHedefleri[i].eslesenTasTuru && HedefTasRengi == seviyeHedefleri[i].eslesenTasRengi)
                {
                    FindObjectOfType<SesYoneticisi>().Oynat("Topla");

                    seviyeHedefleri[i].toplananMiktar++;
                    tahta.hedeflerdeOlanEslesmeMi = true;
                }

                if(seviyeHedefleri[i].eslesenTasTuru == TasTuru.Bos || seviyeHedefleri[i].eslesenTasRengi== TasRengi.Bos)
                {
                    if (HedefTasTuru == seviyeHedefleri[i].eslesenTasTuru || HedefTasRengi == seviyeHedefleri[i].eslesenTasRengi)
                    {
                        FindObjectOfType<SesYoneticisi>().Oynat("Topla");
                        seviyeHedefleri[i].toplananMiktar++;
                        tahta.hedeflerdeOlanEslesmeMi = true;
                    }
                }
            }

        }
        HedefVerileriGuncelle();
    }

    public void FayansiKarsilastir(FayansTur FayansTur)
    {
        for (int i = 0; i < seviyeHedefleri.Length; i++)
        {
            if (seviyeHedefleri[i].eslenenFayansTuru == FayansTur)
            {
                seviyeHedefleri[i].toplananMiktar++;
            }

        }
        HedefVerileriGuncelle();
    }

    public void HedefiKarsilastirTiklanan2(TasTuru HedefTasTuru, TasRengi HedefTasRengi)
    {
        for (int i = 0; i < seviyeHedefleri.Length; i++)
        {
            if (!(seviyeHedefleri[i].toplananMiktar >= seviyeHedefleri[i].gerekenMiktar) && seviyeHedefleri[i].Tiklanan2Mi)
            {

                if (HedefTasTuru == seviyeHedefleri[i].eslesenTasTuru && HedefTasRengi == seviyeHedefleri[i].eslesenTasRengi)
                {
                    FindObjectOfType<SesYoneticisi>().Oynat("Topla");

                    seviyeHedefleri[i].toplananMiktar++;
                    tahta.hedeflerdeOlanEslesmeMi = true;
                }

                if (seviyeHedefleri[i].eslesenTasTuru == TasTuru.Bos || seviyeHedefleri[i].eslesenTasRengi == TasRengi.Bos)
                {
                    if (HedefTasTuru == seviyeHedefleri[i].eslesenTasTuru || HedefTasRengi == seviyeHedefleri[i].eslesenTasRengi)
                    {
                        FindObjectOfType<SesYoneticisi>().Oynat("Topla");

                        seviyeHedefleri[i].toplananMiktar++;
                        tahta.hedeflerdeOlanEslesmeMi = true;
                    }
                }
            }

        }
        HedefVerileriGuncelle();
    }

    public void TasiLanetle(M3Tas tas)
    {
        for (int i = 0; i < seviyeHedefleri.Length; i++)
        {
            if (seviyeHedefleri[i].Tiklanan2Mi)
            {

                if (tas.tasinTuru == seviyeHedefleri[i].eslesenTasTuru && tas.tasinRengi == seviyeHedefleri[i].eslesenTasRengi)  // hedeflerde lanetli tas hedefi varsa o hedefteki tasi lanetliyor.
                {
                    tas.lanetliMi = true;
                }

                if (seviyeHedefleri[i].eslesenTasTuru == TasTuru.Bos || seviyeHedefleri[i].eslesenTasRengi == TasRengi.Bos)  // 1 turu veya bir rengi lanetlemek istiyorsak
                {
                    if (tas.tasinTuru == seviyeHedefleri[i].eslesenTasTuru || tas.tasinRengi == seviyeHedefleri[i].eslesenTasRengi)
                    {

                        tas.lanetliMi = true;

                    }
                }
            }

        }
    }
}
