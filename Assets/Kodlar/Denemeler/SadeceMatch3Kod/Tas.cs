using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tas : MonoBehaviour
{
    [Header(" Tahta Degerleri ")]
    public int satir;
    public int sutun;
    public int oncekiSatir;
    public int oncekiSutun;
    public int hedefX;
    public int hedefY;
    public bool eslestiMi = false;

    private OyunSonuYoneticisi oyunSonuYoneticisi;
    private Ipucu ipucu;
    private EslesmeBulSM3 EslestirmeBulucu;
    private Tahta tahta;
    public GameObject digerTas;

    private Vector2 ilkDokunmaNoktasi;
    private Vector2 sonDokunmaNoktasi;
    private Vector2 geciciPozisyon;

    public float kaydirmaAcisi = 0;
    public float kaydirmaDirenci = 1f;

    public bool renkBombasiMi;
    public bool sutunBombasiMi;
    public bool satirBombasiMi;
    public bool bitisikBombasiMi;
    public GameObject sutunOku;
    public GameObject satirOku;
    public GameObject renkBombasi;
    public GameObject bitisikBombasi;


    // Start is called before the first frame update
    void Start()
    {
        satirBombasiMi = false;
        sutunBombasiMi = false;
        renkBombasiMi = false;
        bitisikBombasiMi = false;

        oyunSonuYoneticisi = FindObjectOfType<OyunSonuYoneticisi>();
        ipucu = FindObjectOfType<Ipucu>();
        tahta = GameObject.FindWithTag("Tahta").GetComponent<Tahta>();
        EslestirmeBulucu = FindObjectOfType<EslesmeBulSM3>();
        //hedefX = (int)transform.position.x;
        //hedefY = (int)transform.position.y;
        //satir = hedefY;
        //sutun = hedefX;
        //oncekiSatir = satir;
        //oncekiSutun = sutun;
    }

    //private void OnMouseOver()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        bitisikBombasiMi = true;
    //        GameObject color = Instantiate(bitisikBombasi, transform.position, Quaternion.identity);
    //        color.transform.parent = this.transform;
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        //if (eslestiMi)
        //{
        //    SpriteRenderer buSprite = GetComponent<SpriteRenderer>();
        //    //buSprite.color = new Color(1f, 1f, 1f, .2f);

        //}
        //EslestirmeBul();

        hedefX = sutun;
        hedefY = satir;

        if(Mathf.Abs(hedefX-transform.position.x) > .1)
        {
            //hedefe git
            geciciPozisyon = new Vector2(hedefX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, geciciPozisyon, .2f);
            if (tahta.tumTaslar[sutun, satir] != this.gameObject)
            {
                tahta.tumTaslar[sutun, satir] = this.gameObject;
            }
            EslestirmeBulucu.TumEslestirmeleriBul();
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
            }

            EslestirmeBulucu.TumEslestirmeleriBul();

        }
        else
        {
            geciciPozisyon = new Vector2(transform.position.x, hedefY);
            transform.position = geciciPozisyon;
  
        }
    }


    public IEnumerator HareketBitmeKontrol()
    {
        if (renkBombasiMi)
        {
            EslestirmeBulucu.RenkEsleme(digerTas.tag);
            eslestiMi = true;
        }
        else if (digerTas.GetComponent<Tas>().renkBombasiMi)
        {
            EslestirmeBulucu.RenkEsleme(this.gameObject.tag);
            digerTas.GetComponent<Tas>().eslestiMi = true;
        }

        yield return new WaitForSeconds(.5f);
        if(digerTas != null)
        {
            if(!eslestiMi && !digerTas.GetComponent<Tas>().eslestiMi)
            {
                digerTas.GetComponent<Tas>().satir = satir;
                digerTas.GetComponent<Tas>().sutun = sutun;
                satir = oncekiSatir;
                sutun = oncekiSutun;

                yield return new WaitForSeconds(.5f);
                tahta.suankiTas = null;
                tahta.suankiDurum = OyunDurumu2.hareket;
            }
            else
            {
                if (oyunSonuYoneticisi != null)
                {
                    if (oyunSonuYoneticisi.kosullar.kosulTuru == OyunKosulTuru.hareketHakki)
                    {
                        oyunSonuYoneticisi.SayaciAzalt();
                    }
                }
                tahta.TumEslestirmeleriYokEt();
            }
            //digerTas = null;
        }

    }

    private void OnMouseDown()
    {
        if (ipucu != null)
        {
            ipucu.IpucunuKaldir();
        }
        if (tahta.suankiDurum == OyunDurumu2.hareket)
        {
            ilkDokunmaNoktasi = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(ilkDokunmaNoktasi);
        }
    }

    private void OnMouseUp()
    {
        if (tahta.suankiDurum == OyunDurumu2.hareket)
        {
            sonDokunmaNoktasi = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            AciyiHesapla();
        }
    }

    void AciyiHesapla()
    {
        if (Mathf.Abs(sonDokunmaNoktasi.y - ilkDokunmaNoktasi.y) > kaydirmaDirenci || Mathf.Abs(sonDokunmaNoktasi.x - ilkDokunmaNoktasi.x) > kaydirmaDirenci)
        {
            tahta.suankiDurum = OyunDurumu2.bekle;
            kaydirmaAcisi = Mathf.Atan2(sonDokunmaNoktasi.y - ilkDokunmaNoktasi.y, sonDokunmaNoktasi.x - ilkDokunmaNoktasi.x) * 180 / Mathf.PI;
            TaslariKaydir();
            
            tahta.suankiTas = this;
        }
        else
        {
            tahta.suankiDurum = OyunDurumu2.hareket;
        }
    }
    void YoneTaslariKaydir(Vector2 yon)
    {
        digerTas = tahta.tumTaslar[sutun + (int)yon.x, satir + (int)yon.y];
        oncekiSatir = satir;
        oncekiSutun = sutun;
        if (digerTas != null)
        {
            digerTas.GetComponent<Tas>().sutun += -1 * (int)yon.x;
            digerTas.GetComponent<Tas>().satir += -1 * (int)yon.y;

            sutun += (int)yon.x;
            satir += (int)yon.y;

            StartCoroutine(HareketBitmeKontrol());
        }
        else
        {
            tahta.suankiDurum = OyunDurumu2.hareket;
        }
    }
    void TaslariKaydir()
    {
        if(kaydirmaAcisi > -45 && kaydirmaAcisi <= 45 && sutun < tahta.en - 1)
        {
            //Saga Kaydir
            YoneTaslariKaydir(Vector2.right);
        }
        else if (kaydirmaAcisi > 45 && kaydirmaAcisi <= 135 && satir < tahta.boy - 1)
        {
            //Yukari Kaydir
            YoneTaslariKaydir(Vector2.up);
        }
        else if (kaydirmaAcisi > 135 || kaydirmaAcisi <= -135 && sutun > 0)
        {
            //Sola Kaydir
            YoneTaslariKaydir(Vector2.left);
        }
        else if (kaydirmaAcisi < -45 && kaydirmaAcisi >= -135 && satir > 0)
        {
            //Asagiya Kaydir
            YoneTaslariKaydir(Vector2.down);
        }
        else
        {
            tahta.suankiDurum = OyunDurumu2.hareket;
        }

        

    }

    void EslestirmeBul()
    {
        if (sutun > 0 && sutun < tahta.en - 1)
        {
            GameObject solTas1 = tahta.tumTaslar[sutun - 1, satir];
            GameObject sagTas1 = tahta.tumTaslar[sutun + 1, satir];
            if (solTas1 != null && sagTas1 != null)
            {
            if(solTas1.tag==this.gameObject.tag && sagTas1.tag== this.gameObject.tag)
            {
                solTas1.GetComponent<Tas>().eslestiMi = true;
                sagTas1.GetComponent<Tas>().eslestiMi = true;
                eslestiMi = true;
                    tahta.suankiDurum = OyunDurumu2.bekle;
                }

            }
        }

        if (satir > 0 && satir < tahta.boy - 1)
        {
            GameObject yukariTas1 = tahta.tumTaslar[sutun, satir + 1];
            GameObject asagiTas1 = tahta.tumTaslar[sutun, satir - 1];
            if(yukariTas1!=null && asagiTas1 != null) {
                
               if (yukariTas1.tag == this.gameObject.tag && asagiTas1.tag == this.gameObject.tag)
                {
                   yukariTas1.GetComponent<Tas>().eslestiMi = true;
                   asagiTas1.GetComponent<Tas>().eslestiMi = true;
                   eslestiMi = true;
                    tahta.suankiDurum = OyunDurumu2.bekle;
                }
            }
        }
    }

    public void SutunBombasiYap()
    {
        if (!satirBombasiMi && !renkBombasiMi && !bitisikBombasiMi)
        {
            sutunBombasiMi = true;
            GameObject ok = Instantiate(sutunOku, transform.position, Quaternion.identity);
            ok.transform.parent = this.transform;
        }
    }

    public void SatirBombasiYap()
    {
        if (!sutunBombasiMi && !renkBombasiMi && !bitisikBombasiMi)
        {
            satirBombasiMi = true;
            GameObject ok = Instantiate(satirOku, transform.position, Quaternion.identity);
            ok.transform.parent = this.transform;
        }
    }

    public void RenkBombasiYap()
    {
        if (!satirBombasiMi && !sutunBombasiMi && !bitisikBombasiMi)
        {
            renkBombasiMi = true;
            GameObject renk = Instantiate(renkBombasi, transform.position, Quaternion.identity);
            renk.transform.parent = this.transform;
            this.gameObject.tag = "Renk";
        }
    }

    public void BitisikBombasiYap()
    {
        if (!satirBombasiMi && !renkBombasiMi && !sutunBombasiMi)
        {
            bitisikBombasiMi = true;
            GameObject bitisikB = Instantiate(bitisikBombasi, transform.position, Quaternion.identity);
            bitisikB.transform.parent = this.transform;
        }
    }
}
