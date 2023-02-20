using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BiHedef
{
    public int gerekenMiktar;
    public int toplananMiktar;
    public Sprite hedefResmi;
    public string eslesenDeger;
}


public class HedefYoneticisi : MonoBehaviour
{
    public BiHedef[] seviyeHedefleri;
    public List<HedefPaneli> suankiHedefler = new List<HedefPaneli>();
    public GameObject hedefSeriUretim;
    public GameObject hedefGirisPaneli;  //Giristeki Hedef Paneli
    public GameObject hedefOyundakiPanel;
    private Tahta tahta;
    private OyunSonuYoneticisi oyunSonuYoneticisi;

    // Start is called before the first frame update
    void Start()
    {
        tahta = FindObjectOfType<Tahta>();
        oyunSonuYoneticisi = FindObjectOfType<OyunSonuYoneticisi>();
        HedefleriAl();
        HedefleriYerlestir();
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
            GameObject hedef = Instantiate(hedefSeriUretim, hedefGirisPaneli.transform.position, Quaternion.identity);
            hedef.transform.SetParent(hedefGirisPaneli.transform , false);
            HedefPaneli panel = hedef.GetComponent<HedefPaneli>();
            panel.buSprite = seviyeHedefleri[i].hedefResmi;
            panel.buString = "" + seviyeHedefleri[i].gerekenMiktar;

            
            GameObject oyunHedef = Instantiate(hedefSeriUretim, hedefOyundakiPanel.transform.position, Quaternion.identity);
            oyunHedef.transform.SetParent(hedefOyundakiPanel.transform, false);
            panel = oyunHedef.GetComponent<HedefPaneli>();
            suankiHedefler.Add(panel);
            panel.buSprite = seviyeHedefleri[i].hedefResmi;
            panel.buString = "" + seviyeHedefleri[i].gerekenMiktar;

        }
    }


    public void HedefVerileriGuncelle()
    {
        int hedefTamamlandi = 0;
        for (int i = 0; i < seviyeHedefleri.Length; i++)
        {
            suankiHedefler[i].buText.text = "" + (seviyeHedefleri[i].gerekenMiktar - seviyeHedefleri[i].toplananMiktar);

            if (seviyeHedefleri[i].toplananMiktar >= seviyeHedefleri[i].gerekenMiktar)
            {
                hedefTamamlandi++;
                suankiHedefler[i].buText.text = "" + 0;
            }

        }

        if (hedefTamamlandi >= seviyeHedefleri.Length)
        {
            if (oyunSonuYoneticisi != null)
            {
                oyunSonuYoneticisi.OyunuKazan();
            }
            Debug.Log("Kazandin!");
        }
    }


    public void HedefiKarsilastir(string karilastirilacakHedef)
    {
        for (int i = 0; i < seviyeHedefleri.Length; i++)
        {
            if (karilastirilacakHedef == seviyeHedefleri[i].eslesenDeger)
            {
                seviyeHedefleri[i].toplananMiktar++;
            }

        }
    }

}
