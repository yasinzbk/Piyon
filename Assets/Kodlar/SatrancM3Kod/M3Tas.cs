using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TasTuru { Piyon, Kale, At, Fil, Vezir, Kral, Bos };
public enum TasRengi { Kirmizi, Mavi, Sari, Pembe, Yesil, Beyaz, Renkli,Bos};

public class M3Tas : MonoBehaviour
{
    [Header(" Tahta Degerleri ")]
    public int satir;
    public int sutun;
    public int oncekiSatir;
    public int oncekiSutun;
    public int hedefX;
    public int hedefY;
    public bool eslestiMi = false;
    public bool secilebilir = false;

    private bool turEslesmesiOlacak;
    public bool lanetliMi;

    private M3Tahta tahta;  

    public TasTuru tasinTuru;
    public TasRengi tasinRengi;

    [HideInInspector]
    public TasRengi renkBombasiRengi = TasRengi.Bos;  // Renk bombasina donusen tasin rengini burda tutuyor. tas tur eslesmesine ugrarsa veya baska bir bomba tarafindan tetiklenip patlarsa burdaki rengi kullanacak

    private Vector2 geciciPozisyon;
    private Vector3 hedefPozisyonTopla;

    private EslesmeBulucu eslesmeBulucu;

    public bool renkBombasiMi;
    public bool sutunBombasiMi;
    public bool satirBombasiMi;
    public bool bitisikBombasiMi;

    public bool tasToplansinMi = false;

    public GameObject sutunOku;
    public GameObject satirOku;
    public GameObject renkBombasi;
    public GameObject bitisikBombasi;

    public GameObject debugObje;

    private void Awake()
    {
        //EventSystem.current.SetSelectedGameObject(gameObject);
    }

    void Start()
    {
        satirBombasiMi = false;
        sutunBombasiMi = false;
        renkBombasiMi = false;
        bitisikBombasiMi = false;

     
        tahta = GameObject.FindWithTag("Tahta").GetComponent<M3Tahta>();
        eslesmeBulucu = FindObjectOfType<EslesmeBulucu>();

        turEslesmesiOlacak = tahta.turEslesmesiOlacakMi;
        tahta.hedefYoneticisi.TasiLanetle(this);

        RenkKontrolDEBUG();
    }


    void Update()
    {
        RenkKontrolDEBUG();
        if (!tasToplansinMi)
        {
        hedefX = sutun;
        hedefY = satir;

        if (Mathf.Abs(hedefX - transform.position.x) > .1)
        {
            //hedefe git
            geciciPozisyon = new Vector2(hedefX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, geciciPozisyon, .2f);
            if (tahta.tumTaslar[sutun, satir] != this.gameObject)
            {
                tahta.tumTaslar[sutun, satir] = this.gameObject;
                eslesmeBulucu.TumEslestirmeleriBul();
                    RenkKontrolDEBUG(); // Hata Kontrol Fonk
            }


        }
        else
        {
            geciciPozisyon = new Vector2(hedefX, transform.position.y);
            transform.position = geciciPozisyon;

        }
        if (Mathf.Abs(hedefY - transform.position.y) > .1)
        {
            //hedefe git
            geciciPozisyon = new Vector2(transform.position.x, hedefY);
            transform.position = Vector2.Lerp(transform.position, geciciPozisyon, .2f);
            if (tahta.tumTaslar[sutun, satir] != this.gameObject)
            {
                tahta.tumTaslar[sutun, satir] = this.gameObject;
               eslesmeBulucu.TumEslestirmeleriBul();
                    RenkKontrolDEBUG(); // Hata kontrol Fonk
            }


        }
        else
        {
            geciciPozisyon = new Vector2(transform.position.x, hedefY);
            transform.position = geciciPozisyon;

        }

        }

        if (tasToplansinMi)
        {
            transform.position = Vector3.Lerp(transform.position, hedefPozisyonTopla, 2f * Time.deltaTime);

            if(Mathf.Abs( transform.position.x - hedefPozisyonTopla.x) <= 1.0f  && Mathf.Abs(transform.position.y - hedefPozisyonTopla.y) <= 1.0f)
            {
                tahta.hedeflerdeOlanEslesmeMi = false;
                Destroy(gameObject);
            }
        }
    }

    //public void OnDeselect(BaseEventData eventData)
    //{
    //    tahta.SecilenleriBirak();
    //    tahta.TumKutucukIsaretleriniKapat();
    //}

    private void OnMouseDown()
    {
        if (tahta.suankiDurum == OyunDurumu.hareket && tahta.kilitliFayans[sutun,satir]==null)
        {
            if (tahta.tiklanan1 == null && !lanetliMi)
            {
                tahta.tiklanan1 = this;
                tahta.GidilebilecekKutulariGoster(tasinTuru, sutun, satir);
               // Debug.Log(this.gameObject.name + tasinTuru + tasinRengi);

                //hareket.SeciliSimgesiOlustur();
            }
            else if(secilebilir)
            {
                tahta.tiklanan2 = this;

               // Debug.Log(this.gameObject.name + tasinTuru + tasinRengi + "  Yer degistiriyor");


                tahta.suankiDurum = OyunDurumu.bekle;
                tahta.TumKutucukIsaretleriniKapat();
                tahta.TaslariHareketEttir();

            }
            else
            {
                tahta.SecilenleriBirak();
                tahta.TumKutucukIsaretleriniKapat();
            }

        }
    }

    public void RenkKontrolDEBUG()
    {
        if (tasinRengi == TasRengi.Kirmizi)
        {
            debugObje.GetComponent<SpriteRenderer>().color = Color.red;
        }else if (tasinRengi == TasRengi.Mavi)
        {
            debugObje.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else if (tasinRengi == TasRengi.Sari)
        {
            debugObje.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if (tasinRengi == TasRengi.Pembe)
        {
            debugObje.GetComponent<SpriteRenderer>().color = Color.magenta;
        }
        else if (tasinRengi == TasRengi.Yesil)
        {
            debugObje.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (tasinRengi == TasRengi.Beyaz)
        {
            debugObje.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            debugObje.GetComponent<SpriteRenderer>().color = Color.clear; //gorunmez yapıyor
        }
    }

    public bool EslesmeKontrol(M3Tas x, M3Tas y)
    {
        return (x.tasinRengi == tasinRengi && y.tasinRengi == tasinRengi ) || (( x.tasinTuru == tasinTuru && y.tasinTuru == tasinTuru) && turEslesmesiOlacak);
    }


    public void SutunBombasiYap()
    {
        eslesmeBulucu.bombaEslesmelerindeTasZatenBomba(this.GetComponent<M3Tas>());
        //if (!satirBombasiMi && !renkBombasiMi && !bitisikBombasiMi)
        //{
        sutunBombasiMi = true;
            HedeflereEkle();
            GameObject ok = Instantiate(sutunOku, transform.position, Quaternion.identity);
            ok.transform.parent = this.transform;
            MaskeAyarla(ok);
        //}
    }

    public void SatirBombasiYap()
    {
        eslesmeBulucu.bombaEslesmelerindeTasZatenBomba(this.GetComponent<M3Tas>());
        //if (!sutunBombasiMi && !renkBombasiMi && !bitisikBombasiMi)
        //{
        satirBombasiMi = true;
            HedeflereEkle();
            GameObject ok = Instantiate(satirOku, transform.position, Quaternion.identity);
            ok.transform.parent = this.transform;
            MaskeAyarla(ok);

        //}
    }

    public void RenkBombasiYap()
    {
        eslesmeBulucu.bombaEslesmelerindeTasZatenBomba(this.GetComponent<M3Tas>());
        //if (!satirBombasiMi && !sutunBombasiMi && !bitisikBombasiMi)
        //{
        renkBombasiMi = true;
        renkBombasiRengi = tasinRengi;
        tasinRengi = TasRengi.Renkli;
            HedeflereEkle();
            GameObject renk = Instantiate(renkBombasi, transform.position, Quaternion.identity);
            renk.transform.parent = this.transform;
            this.gameObject.tag = "Renk";
        //}
    }

    public void BitisikBombasiYap()
    {
        eslesmeBulucu.bombaEslesmelerindeTasZatenBomba(this.GetComponent<M3Tas>());
        //if (!satirBombasiMi && !renkBombasiMi && !sutunBombasiMi)
        //{
        bitisikBombasiMi = true;
            HedeflereEkle();
            GameObject bitisikB = Instantiate(bitisikBombasi, transform.position, Quaternion.identity);
            bitisikB.transform.parent = this.transform;
        MaskeAyarla(bitisikB);
        //}
    }

    public void HedeflereEkle()
    {
       tahta.hedefYoneticisi.HedefiKarsilastir(tasinTuru,tasinRengi);
    }

    private void MaskeAyarla(GameObject Obje)
    {
        Obje.GetComponent<SpriteMask>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }


    public void ToplaVeYokEt( Vector3 hedef)
    {
        tasToplansinMi = true;
        hedefPozisyonTopla = hedef;

    }
}
