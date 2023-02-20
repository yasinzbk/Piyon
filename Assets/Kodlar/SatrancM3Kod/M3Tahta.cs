using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OyunDurumu { hareket, bekle, durdur, kazandin, kaybettin};

public enum FayansTur { kirilabilir, bos, kilitli, buz, sarmasik, normal, yok }

//[System.Serializable]
//public class EslesmeTipi
//{
//    public int tipNO;
//    public TasTuru tasinTuru;
//    public TasRengi tasinRengi;
//    public M3Tas eslesenBomba;
//}


[System.Serializable]
public class Fayans
{
    public int x;
    public int y;
    public FayansTur fayTur;
}

public class M3Tahta : MonoBehaviour
{
    [Header("Scriptable Obje Seyleri")]
    public AlemSM3 alem;
    public int seviye;
    public int alemNo;

    [Header("Tahta Boyutlari")]
    public int en;
    public int boy;
    public int ilkNoktaMesafesi;

    public OyunDurumu suankiDurum = OyunDurumu.hareket;

    [Header("SeriUretimler")]
    public GameObject kareSeriUretim;
    public GameObject kirilabilirFayansSU;
    public GameObject kilitliFayansSU;
    public GameObject buzFayansSU;
    public GameObject sarmasikFayansSU;
    public GameObject[] taslar;
    public GameObject YokOlmaEfekti;
    public GameObject satirBombasiEfektiSU;
    public GameObject sutunBombasiEfektiSU;
    public GameObject bitisikBombasiEfektiSU;

    [Header("Diger")]
    public Fayans[] tahtaYerlesim;  // Seviyenin fayans planını belirlemek için Fayans Array'i
    private bool[,] bosAlanlar;
    private KirilabilirFayans[,] kirilabilirFayans;
    public KirilabilirFayans[,] kilitliFayans;
    private KirilabilirFayans[,] buzFayans;
    private KirilabilirFayans[,] sarmasikFayans;
    public Kutucuk[,] tumKutucuklar;
    public GameObject[,] tumTaslar;

    private SesYoneticisi sesYoneticisi;
    Camera kamera;

    [Header("Eslesme Seyleri")]
    //public EslesmeTipi eslesmetipi;
    public M3Tas tiklanan1, tiklanan2;
    private EslesmeBulucu eslesmeBulucu;
    public int anaTasDegeri = 1;
    private int komboDegeri = 1;
    private SkorYoneticisi skorYoneticisi;
    public HedefYoneticisiSM3 hedefYoneticisi;
    private OyunSonuYoneticisiSM3 oyunSonuYoneticisi;
    public float dolmaSuresi = 0.2f;
    public bool turEslesmesiOlacakMi = true;
    private OyunVerisi oyunVerisi;
    private OnayPaneli onayPaneli;
    public GameObject hedefAlani;
    public bool hedeflerdeOlanEslesmeMi = false;
    private bool sarmasikYap = true;

    [Header("Kamera")]
    public KameraSallanma kameraSallanma;
    public float sallanmaSuresi;
    public float sallanmaMiktari;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("seviyeKayitAdi"))
        {
            seviye = PlayerPrefs.GetInt(PlayerPrefs.GetString("seviyeKayitAdi"));
        }
        if (alem != null)
        {
            if (seviye < alem.seviyeler.Length)
            {
                if (alem.seviyeler[seviye] != null)
                {
                    en = alem.seviyeler[seviye].en;
                    boy = alem.seviyeler[seviye].boy;
                    taslar = alem.seviyeler[seviye].taslar;
                    tahtaYerlesim = alem.seviyeler[seviye].fayansYerlesim;
                    turEslesmesiOlacakMi = alem.seviyeler[seviye].turEslesmesiOlacakMi;
                    //skorHedefleri = alem.seviyeler[seviye].skorHedefleri;
                }
            }
        }

        if (PlayerPrefs.HasKey("alemNo"))
        {
            alemNo = PlayerPrefs.GetInt("alemNo");
        }
        else
        {
            Debug.Log("Kayit yok");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        oyunSonuYoneticisi = FindObjectOfType<OyunSonuYoneticisiSM3>();
        hedefYoneticisi = FindObjectOfType<HedefYoneticisiSM3>();
        skorYoneticisi = FindObjectOfType<SkorYoneticisi>();
        oyunVerisi = FindObjectOfType<OyunVerisi>();
        onayPaneli = FindObjectOfType<OnayPaneli>();
        kamera = FindObjectOfType<Camera>();
        sesYoneticisi = FindObjectOfType<SesYoneticisi>();
        eslesmeBulucu = FindObjectOfType<EslesmeBulucu>();

        kirilabilirFayans = new KirilabilirFayans[en, boy];
        kilitliFayans = new KirilabilirFayans[en, boy];
        buzFayans = new KirilabilirFayans[en, boy];
        sarmasikFayans = new KirilabilirFayans[en, boy];
        tumKutucuklar = new Kutucuk[en, boy];
        tumTaslar = new GameObject[en, boy];
        bosAlanlar = new bool[en, boy];
        TahtayiOlustur();

        //StartCoroutine(HareketeBaslatCo());

        OnayPaneliniAyarla();
    }


    public void BosFayanslariAyarla() // "tahtaYerlesim" düzeninden boş fayansların yerini "bosAlanlar" Bool'una atıyor.
    {
        for (int i = 0; i < tahtaYerlesim.Length; i++)
        {
            if (tahtaYerlesim[i].fayTur == FayansTur.bos)
            {
                bosAlanlar[tahtaYerlesim[i].x, tahtaYerlesim[i].y] = true;
            }

        }
    }

    public void KirilabilirFayanslariAyarla() // Seviyedeki Kırılabilir fayansları oluşturur ve kirilabilirFayans'a konumunu kaydeder 
    {
        for (int i = 0; i < tahtaYerlesim.Length; i++)
        {
            if (tahtaYerlesim[i].fayTur == FayansTur.kirilabilir)
            {
                Vector2 geciciPoz = new Vector2(tahtaYerlesim[i].x, tahtaYerlesim[i].y);
                GameObject fayans = Instantiate(kirilabilirFayansSU, geciciPoz, Quaternion.identity);
                kirilabilirFayans[tahtaYerlesim[i].x, tahtaYerlesim[i].y] = fayans.GetComponent<KirilabilirFayans>();
            }
        }
    }

    private void KilitliFayanslariAyarla()
    {
        for (int i = 0; i < tahtaYerlesim.Length; i++)
        {
            if (tahtaYerlesim[i].fayTur == FayansTur.kilitli)
            {
                Vector2 geciciPoz = new Vector2(tahtaYerlesim[i].x, tahtaYerlesim[i].y);
                GameObject fayans = Instantiate(kilitliFayansSU, geciciPoz, Quaternion.identity);
                kilitliFayans[tahtaYerlesim[i].x, tahtaYerlesim[i].y] = fayans.GetComponent<KirilabilirFayans>();
            }
        }
    }

    private void BuzFayanslariAyarla()
    {
        for (int i = 0; i < tahtaYerlesim.Length; i++)
        {
            if (tahtaYerlesim[i].fayTur == FayansTur.buz)
            {
                Vector2 geciciPoz = new Vector2(tahtaYerlesim[i].x, tahtaYerlesim[i].y);
                GameObject fayans = Instantiate(buzFayansSU, geciciPoz, Quaternion.identity);
                buzFayans[tahtaYerlesim[i].x, tahtaYerlesim[i].y] = fayans.GetComponent<KirilabilirFayans>();
            }
        }
    }

    private void SarmasikFayanslariAyarla()
    {
        for (int i = 0; i < tahtaYerlesim.Length; i++)
        {
            if (tahtaYerlesim[i].fayTur == FayansTur.sarmasik)
            {
                Vector2 geciciPoz = new Vector2(tahtaYerlesim[i].x, tahtaYerlesim[i].y);
                GameObject fayans = Instantiate(sarmasikFayansSU, geciciPoz, Quaternion.identity);
                sarmasikFayans[tahtaYerlesim[i].x, tahtaYerlesim[i].y] = fayans.GetComponent<KirilabilirFayans>();
            }
        }
    }
    public void TahtayiOlustur() // Tahtaya taşları rasgele ekler MatchesAt() İle eşleşmeyi önler
    {
        BosFayanslariAyarla();
        KirilabilirFayanslariAyarla();
        KilitliFayanslariAyarla();
        BuzFayanslariAyarla();
        SarmasikFayanslariAyarla();

        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if (!bosAlanlar[i, j] && !buzFayans[i,j] && !sarmasikFayans[i,j])
                {
                    Vector2 geciciPosizyon = new Vector2(i, j + ilkNoktaMesafesi);
                    Vector2 geciciPosizyonKutu = new Vector2(i, j);
                    GameObject birKare = Instantiate(kareSeriUretim, geciciPosizyonKutu, Quaternion.identity) as GameObject;
                    birKare.transform.parent = this.transform;
                    birKare.name = "( " + i + ", " + j + " )";
                    int kullanilacakTas = Random.Range(0, taslar.Length);
                    int tavanYineleme = 0;
                    while (MatchesAt(i, j, taslar[kullanilacakTas]) && tavanYineleme < 100)
                    {
                        kullanilacakTas = Random.Range(0, taslar.Length);
                        tavanYineleme++;

                    }
                    //tavanYineleme = 0;
                    GameObject tas = Instantiate(taslar[kullanilacakTas], geciciPosizyon, Quaternion.identity);
                    tas.transform.parent = this.transform;
                    tas.GetComponent<M3Tas>().satir = j;
                    tas.GetComponent<M3Tas>().sutun = i;
                    tas.name = "( " + i + ", " + j + " )";
                    tumTaslar[i, j] = tas;
                    tumKutucuklar[i, j] = birKare.GetComponent<Kutucuk>();
                }
            }
        }

        KilitlenmeKontrol();
    }


    public void SecilenleriBirak()
    {

        tiklanan1 = null;
        tiklanan2 = null;
    }



    public void GidilebilecekKutulariGoster(TasTuru tasinTuru, int sutun, int satir)  // Taşların türüne göre gidebilecekleri kareleri gösteren işaretler yakar
    {
        if (tasinTuru == TasTuru.At)
        {
            //L
            tumKutucuklar[sutun, satir].YesilKutuYak();

            KutucukSiyahIsareti(sutun + 2, satir + 1);
            KutucukSiyahIsareti(sutun + 2, satir - 1);
            KutucukSiyahIsareti(sutun - 2, satir + 1);
            KutucukSiyahIsareti(sutun - 2, satir - 1);
            KutucukSiyahIsareti(sutun - 1, satir + 2);
            KutucukSiyahIsareti(sutun + 1, satir + 2);
            KutucukSiyahIsareti(sutun - 1, satir - 2);
            KutucukSiyahIsareti(sutun + 1, satir - 2);


        }
        else if (tasinTuru == TasTuru.Fil)
        {
            //(1,1) (-1,-1), (1,-1), (-1,1)

            tumKutucuklar[sutun, satir].YesilKutuYak();

                bool engelVarMi1 = false;    // tasin gidecegi dort yonden biri icin engel olup olmadigi.
                bool engelVarMi2 = false;
                bool engelVarMi3 = false;
                bool engelVarMi4 = false;         

            for (int i = 0; i < EnMiBoyMuBuyuk(); i++)
            {
                if (!engelVarMi1)
                {
                    engelVarMi1 = TasinOnundeEngelKontrol(sutun + 1 * i, satir - 1 * i);
                }
                if (!engelVarMi2)
                {
                    engelVarMi2 = TasinOnundeEngelKontrol(sutun - 1 * i, satir - 1 * i);
                }

                if(!engelVarMi3)
                engelVarMi3 = TasinOnundeEngelKontrol(sutun + 1 * i, satir + 1 * i);

                if(!engelVarMi4)
                engelVarMi4 = TasinOnundeEngelKontrol(sutun - 1 * i, satir + 1 * i);

                if (!engelVarMi1)
                {
                    KutucukSiyahIsareti(sutun + 1 * i, satir - 1 * i);
                }
                if (!engelVarMi2)
                {
                    KutucukSiyahIsareti(sutun - 1*i, satir - 1*i);
                }
                if (!engelVarMi3)
                {
                    KutucukSiyahIsareti(sutun + 1 * i, satir + 1 * i);
                }
                if (!engelVarMi4)
                {
                    KutucukSiyahIsareti(sutun - 1 * i, satir + 1 * i);
                }
            }



        }
        else if (tasinTuru == TasTuru.Kale)
        {
            //(1,0) (0,1)
            tumKutucuklar[sutun, satir].YesilKutuYak();

            bool engelVarMi1 = false;                // tasin gidecegi dort yonden biri icin engel olup olmadigi.
            bool engelVarMi2 = false;
            bool engelVarMi3 = false;
            bool engelVarMi4 = false;
            for (int i = 0; i < EnMiBoyMuBuyuk(); i++)
            {
                if (!engelVarMi1)
                {
                    engelVarMi1 = TasinOnundeEngelKontrol(sutun +i, satir);
                    KutucukSiyahIsareti(sutun +i, satir);
                }
                if (!engelVarMi2)
                {
                    engelVarMi2 = TasinOnundeEngelKontrol(sutun, satir +i);
                    KutucukSiyahIsareti(sutun, satir +i);
                }
                if (!engelVarMi3)
                {
                    engelVarMi3 = TasinOnundeEngelKontrol(sutun -i, satir);
                    KutucukSiyahIsareti(sutun -i, satir);
                }
                if (!engelVarMi4)
                {
                    engelVarMi4 = TasinOnundeEngelKontrol(sutun, satir -i);
                    KutucukSiyahIsareti(sutun, satir -i);
                }
            }
        }
        else if (tasinTuru == TasTuru.Kral)
        {
            //sadece bir kare Caprazlar Dahil (bilmiyorum)  (Belki Bonus olabilir Satır patlatma Gibi ya da daha mantıklısı krallar hareket edemez eger 3 kral yan yana girerse tum krallari, 3 aynı renk kral yan yana gelirse tum o rengi yok eder)
            tumKutucuklar[sutun, satir].YesilKutuYak();

            KutucukSiyahIsareti(sutun + 1, satir);
            KutucukSiyahIsareti(sutun - 1, satir);
            KutucukSiyahIsareti(sutun, satir + 1);
            KutucukSiyahIsareti(sutun, satir - 1);

            KutucukSiyahIsareti(sutun + 1, satir + 1);
            KutucukSiyahIsareti(sutun - 1, satir - 1);
            KutucukSiyahIsareti(sutun - 1 , satir + 1);
            KutucukSiyahIsareti(sutun + 1, satir - 1);

        }
        else if (tasinTuru == TasTuru.Piyon)
        {
            tumKutucuklar[sutun, satir].YesilKutuYak();

            //1 kare
            KutucukSiyahIsareti(sutun + 1 , satir  );
            KutucukSiyahIsareti(sutun - 1 , satir  );
            KutucukSiyahIsareti(sutun  , satir + 1 );
            KutucukSiyahIsareti(sutun  , satir - 1 );
        }
        else  // Vezirin engellerin uzerinden atlamamasi istenilirse Kilitlenme kontrole de eklemeyi unutma
        {
            tumKutucuklar[sutun, satir].YesilKutuYak();
            // L haric Hepsi

            for (int i = 0; i < EnMiBoyMuBuyuk(); i++)
            {
                KutucukSiyahIsareti(sutun + 1 * i, satir);
                KutucukSiyahIsareti(sutun, satir + 1 * i);
                KutucukSiyahIsareti(sutun - 1 * i, satir);
                KutucukSiyahIsareti(sutun, satir - 1 * i);
            }

            for (int i = 0; i < EnMiBoyMuBuyuk(); i++)
            {
                KutucukSiyahIsareti(sutun + 1 * i, satir - 1 * i);
                KutucukSiyahIsareti(sutun - 1 * i, satir - 1 * i);
                KutucukSiyahIsareti(sutun + 1 * i, satir + 1 * i);
                KutucukSiyahIsareti(sutun - 1 * i, satir + 1 * i);
            }
        }
    }

    private int EnMiBoyMuBuyuk()
    {
        if (en > boy)
        {
            return en;
        }

        return boy;
    }

    private void KutucukSiyahIsareti(int sutun, int satir)
    { 
        if (sutun < tumKutucuklar.GetLength(0)  && satir < tumKutucuklar.GetLength(1) && sutun >=0 && satir >=0)
        {
            if ( tumKutucuklar[sutun, satir] != null && tumTaslar[sutun, satir])
            {
                tumKutucuklar[sutun, satir].SiyahKutuYak();
                tumTaslar[sutun, satir].GetComponent<M3Tas>().secilebilir = true;
            }
        }
            
    }

    private bool TasinOnundeEngelKontrol(int sutun, int satir)
    {
        if (sutun < tumKutucuklar.GetLength(0) && satir < tumKutucuklar.GetLength(1) && sutun >= 0 && satir >= 0)
        {
            if(bosAlanlar[sutun, satir] || buzFayans[sutun, satir] || kilitliFayans[sutun,satir])
            {
                return true;
            }
        }

        return false;
    }

    public void TumKutucukIsaretleriniKapat() // Sectigin tasi ve o taşin gidebileceigi yerleri gosteren isaretleri yakar
    {
        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if(tumTaslar[i, j] && tumKutucuklar[i,j])
                {
                    tumKutucuklar[i, j].SiyahKutuSondur();
                    tumKutucuklar[i, j].YesilKutuSondur();

                    tumTaslar[i, j].GetComponent<M3Tas>().secilebilir = false;
                }

            }
        }
    }

    public void TaslariHareketEttir()  // Eski Degerli Onceki Sutun Satir olarak burda kaydedebilirim
    {
        tiklanan1.oncekiSatir = tiklanan1.satir;
        tiklanan1.oncekiSutun = tiklanan1.sutun;

        tiklanan2.oncekiSatir = tiklanan2.satir;
        tiklanan2.oncekiSutun = tiklanan2.sutun;


        int x, y;
        x = tiklanan1.sutun;
        y = tiklanan1.satir;

        tiklanan1.sutun = tiklanan2.sutun;
        tiklanan1.satir = tiklanan2.satir;

        tiklanan2.sutun = x;
        tiklanan2.satir = y;

        sesYoneticisi.Oynat("TasHareket");
        StartCoroutine(HareketBitmeKontrol());

        Debug.Log("Taslar hareket ediyor");
    }

    public IEnumerator HareketBitmeKontrol()
    {
        if (tiklanan1.renkBombasiMi)
        {
            eslesmeBulucu.RenkEsleme(tiklanan2.tasinRengi , tiklanan2.tasinTuru);
            tiklanan1.eslestiMi = true;
        }
        if (tiklanan2.renkBombasiMi)   // else if idi (Hem tiklanan1 hemde tiklanan2'de renkbombasi durumu için else i sildim)
        {
            eslesmeBulucu.RenkEsleme(tiklanan1.tasinRengi, tiklanan1.tasinTuru);
            tiklanan2.eslestiMi = true;
        }

        yield return new WaitForSeconds(.5f);
        if (tiklanan2 != null)
        {
            if (!tiklanan1.eslestiMi && !tiklanan2.eslestiMi)
            {
                tiklanan2.satir = tiklanan1.satir;
                tiklanan2.sutun = tiklanan1.sutun;
                tiklanan1.satir = tiklanan1.oncekiSatir;
                tiklanan1.sutun = tiklanan1.oncekiSutun;

                yield return new WaitForSeconds(.5f);
                if (suankiDurum == OyunDurumu.bekle)
                {
                    suankiDurum = OyunDurumu.hareket;
                }
            }
            else
            {
                //if (tiklanan2.eslestiMi)
                //{
                //    hedefYoneticisi.HedefiKarsilastirTiklanan2(tiklanan2.tasinTuru, tiklanan2.tasinRengi);
                //}

                if (oyunSonuYoneticisi.kosullar.kosulTuru == OyunKosulTuruSM3.hareketHakki)
                {
                    oyunSonuYoneticisi.SayaciAzalt();
                }
                TumEslestirmeleriYokEt();

                if (suankiDurum==OyunDurumu.bekle)
                {
                    suankiDurum = OyunDurumu.hareket;
                }
            }

            SecilenleriBirak();
        }

    }


    private void YapilacakBombaKontrol2()
    {
        List<GameObject> eslesmeKopyasi = eslesmeBulucu.suankiEslesmeler as List<GameObject>;
        List<GameObject> bulunanTaslar = new List<GameObject>();

        for (int i = 0; i < eslesmeKopyasi.Count  && !bulunanTaslar.Contains(eslesmeKopyasi[i]); i++)       // !!!!!!!!!!!! bisiler hatali ama kontrol et
        {
            M3Tas buTas = eslesmeKopyasi[i].GetComponent<M3Tas>();
            TasRengi tasrengi = eslesmeKopyasi[i].GetComponent<M3Tas>().tasinRengi;
            TasTuru tasturu = eslesmeKopyasi[i].GetComponent<M3Tas>().tasinTuru;
            List<M3Tas> ayniSatirEslenmisler = new List<M3Tas>();                         //aynı satırdaki aynı tür veya aynı renkteki eşlenmişler
            ayniSatirEslenmisler.Clear();
            List<M3Tas> ayniSutunEslenmisler = new List<M3Tas>();                         // ''  sutundaki '' '' ...
            ayniSutunEslenmisler.Clear();

            List<GameObject> bulunanTaslarSatir = new List<GameObject>();
            List<GameObject> bulunanTaslarSutun = new List<GameObject>();

            for (int j = 0; j < eslesmeKopyasi.Count; j++)
            {
                M3Tas sonrakiTas = eslesmeKopyasi[j].GetComponent<M3Tas>();
                if (sonrakiTas == buTas)
                {
                    continue;
                }
                if (sonrakiTas.sutun == buTas.sutun && (sonrakiTas.tasinRengi == tasrengi || sonrakiTas.tasinTuru == tasturu))
                {

                    ayniSutunEslenmisler.Add(sonrakiTas);

                }
                if (sonrakiTas.satir == buTas.satir && (sonrakiTas.tasinRengi == tasrengi || sonrakiTas.tasinTuru == tasturu))
                {

                    ayniSatirEslenmisler.Add(sonrakiTas);

                }
            }

            for (int g = 1; g >= 0; g++)       // buTas'in sag kismi icin    // arasinda bosluk olmayacak sekilde sagdaki eslenmeleri kontrol ediyor
            {
                bool bulunduMu = false;
                for (int k = 0; k < ayniSatirEslenmisler.Count; k++)
                {
                    if (buTas.sutun + g == ayniSatirEslenmisler[k].sutun)
                    {
                        bulunduMu = true;
                        bulunanTaslarSatir.Add(ayniSatirEslenmisler[k].gameObject);
                    }
                }
                if (!bulunduMu)
                {
                    g = -3;
                }
            }

            for (int g = -1; g <= 0; g--)       // buTas'in sol kismi icin   // arasinda bosluk olmayacak sekilde soldaki eslenmeleri kontrol ediyor
            {
                bool bulunduMu = false;
                for (int k = 0; k < ayniSatirEslenmisler.Count; k++)
                {
                    if (buTas.sutun + g == ayniSatirEslenmisler[k].sutun)
                    {
                        bulunduMu = true;
                        bulunanTaslarSatir.Add(ayniSatirEslenmisler[k].gameObject);
                    }
                }
                if (!bulunduMu)
                {
                    g = 3;
                }
            }

            for (int g = 1;  g>=0;  g++)       // buTas'in yukari kismi icin  
            {
                bool bulunduMu = false;
                for (int k = 0; k < ayniSutunEslenmisler.Count; k++)
                {
                    if (buTas.satir + g == ayniSutunEslenmisler[k].satir)
                    {
                        bulunduMu = true;
                        bulunanTaslarSutun.Add(ayniSutunEslenmisler[k].gameObject);
                    }
                }
                if (!bulunduMu)
                {
                    g = -3;
                }
            }

            for (int g = -1; g <= 0; g--)       // buTas'in asagi kismi icin  
            {
                bool bulunduMu = false;
                for (int k = 0; k < ayniSutunEslenmisler.Count; k++)
                {
                    if (buTas.satir + g == ayniSutunEslenmisler[k].satir)
                    {
                        bulunduMu = true;
                        bulunanTaslarSutun.Add(ayniSutunEslenmisler[k].gameObject);
                    }
                }
                if (!bulunduMu)
                {
                    g = 3;
                }
            }

            void TiklananVarMiKontrol()  // hata olabilir muhtemelen tiklanan varken başka bi yerde bomba oluşacaksa onu da buna eşitleyebilir
            {
                if (tiklanan1 != null && tiklanan1.eslestiMi && (tiklanan1.tasinRengi == buTas.tasinRengi || tiklanan1.tasinTuru == buTas.tasinTuru))
                {
                    buTas = tiklanan1;
                }
                else if (tiklanan2 != null && tiklanan2.eslestiMi && (tiklanan2.tasinRengi == buTas.tasinRengi || tiklanan2.tasinTuru == buTas.tasinTuru))
                {
                    buTas = tiklanan2;
                }
            }



            Debug.Log("SutunEslenmeler: " + bulunanTaslarSutun.Count);
            Debug.Log("SatirEslenmeler: " + bulunanTaslarSatir.Count);

            if (bulunanTaslarSutun.Count == 4 || bulunanTaslarSatir.Count == 4) // veyaları 2 ye ayırmalıyım alttaki satir sutun icinde
            {
                TiklananVarMiKontrol();  // kontrol Edilecek

                buTas.eslestiMi = false;
                buTas.RenkBombasiYap();
                EslesmeEtkisi(buTas.sutun, buTas.satir);

                foreach (GameObject item in bulunanTaslarSatir)
                {
                    bulunanTaslar.Add(item);
                }
                foreach (GameObject item in bulunanTaslarSutun)
                {
                    bulunanTaslar.Add(item);
                }
            }
            else if (bulunanTaslarSutun.Count == 2 && bulunanTaslarSatir.Count == 2)
            {
                buTas.eslestiMi = false;
                buTas.BitisikBombasiYap();
                EslesmeEtkisi(buTas.sutun, buTas.satir);


                foreach (GameObject item in bulunanTaslarSatir)
                {
                    bulunanTaslar.Add(item);
                }
                foreach (GameObject item in bulunanTaslarSutun)
                {
                    bulunanTaslar.Add(item);
                }
            }
            else if (bulunanTaslarSutun.Count == 3 || bulunanTaslarSatir.Count == 3)
            {
                TiklananVarMiKontrol();

                buTas.eslestiMi = false;
                int bombaTuru = Random.Range(0, 100);
                if (bombaTuru < 50)  // Burayı rasgele degil de switchAngle a gore degistirmis
                {
                    buTas.SatirBombasiYap();
                    EslesmeEtkisi(buTas.sutun, buTas.satir);
                }
                else if (bombaTuru >= 50)
                {
                    buTas.SutunBombasiYap();
                    EslesmeEtkisi(buTas.sutun, buTas.satir);
                }

                foreach (GameObject item in bulunanTaslarSatir)
                {
                    bulunanTaslar.Add(item);
                }
                foreach (GameObject item in bulunanTaslarSutun)
                {
                    bulunanTaslar.Add(item);
                }


            }
        }

    }


    private bool MatchesAt(int sutun, int satir, GameObject tas)
    {
        if (sutun > 1 && satir > 1)
        {
            if (tumTaslar[sutun - 1, satir] != null && tumTaslar[sutun - 2, satir] != null)
            {
                if (EslesmeKontrol(tas, tumTaslar[sutun - 1, satir], tumTaslar[sutun - 2, satir]))
                {
                    return true;
                }
            }
            if (tumTaslar[sutun, satir - 1] != null && tumTaslar[sutun, satir - 2] != null)
            {
                if (EslesmeKontrol(tas, tumTaslar[sutun, satir - 1], tumTaslar[sutun, satir - 2]))
                {
                    return true;
                }
            }
        }
        else if (sutun <= 1 || satir <= 1)
        {
            if (satir > 1)
            {
                if (tumTaslar[sutun, satir - 1] != null && tumTaslar[sutun, satir - 2] != null)
                {
                    if (EslesmeKontrol(tas, tumTaslar[sutun, satir - 1], tumTaslar[sutun, satir - 2]))
                    {
                        return true;
                    }
                }
            }
            if (sutun > 1)
            {
                if (tumTaslar[sutun - 1, satir] != null && tumTaslar[sutun - 2, satir] != null)
                {
                    if (EslesmeKontrol(tas, tumTaslar[sutun - 1, satir], tumTaslar[sutun - 2, satir]))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool MatchesAt2(int sutun, int satir, GameObject tas)  // Bu eslesme kontrolu sag tarafi da kontrol ediyor. matchesat1'de tum taslar yeni eklendigi icin buna gerek yoktu  ama 2'yi belli sayıda tas icin kullanabilirim
    {
        if (sutun > 1 && satir > 1)
        {
            if (tumTaslar[sutun - 1, satir] != null && tumTaslar[sutun - 2, satir] != null)
            {
                if (EslesmeKontrol(tas, tumTaslar[sutun - 1, satir], tumTaslar[sutun - 2, satir])) 
                {
                    return true;
                }
            }
            if (tumTaslar[sutun, satir - 1] != null && tumTaslar[sutun, satir - 2] != null)
            {
                if (EslesmeKontrol(tas, tumTaslar[sutun, satir - 1], tumTaslar[sutun, satir - 2]))
                {
                    return true;
                }
            }
            

        }
        else if (sutun <= 1 || satir <= 1)
        {
            if (satir > 1)
            {
                if (tumTaslar[sutun, satir - 1] != null && tumTaslar[sutun, satir - 2] != null)
                {
                    if (EslesmeKontrol(tas, tumTaslar[sutun, satir - 1], tumTaslar[sutun, satir - 2]))
                    {
                        return true;
                    }
                }

            }
            if (sutun > 1)
            {
                if (tumTaslar[sutun - 1, satir] != null && tumTaslar[sutun - 2, satir] != null)
                {
                    if (EslesmeKontrol(tas, tumTaslar[sutun - 1, satir], tumTaslar[sutun - 2, satir]))
                    {
                        return true;
                    }
                }


            }
        }


        if (sutun < en-2 && satir < boy-2 && sutun > 1 && satir > 1)
        {
            if (tumTaslar[sutun + 1, satir] != null && tumTaslar[sutun + 2, satir] != null)
            {
                if (EslesmeKontrol(tas, tumTaslar[sutun + 1, satir], tumTaslar[sutun + 2, satir]))
                {
                    return true;
                }
            }

            if (tumTaslar[sutun + 1, satir] != null && tumTaslar[sutun - 1, satir] != null)
            {
                if (EslesmeKontrol(tas, tumTaslar[sutun + 1, satir], tumTaslar[sutun - 1, satir]))
                {
                    return true;
                }
            }
        }
        else if(sutun < en - 1 && sutun > 0)
        {
            if (tumTaslar[sutun + 1, satir] != null && tumTaslar[sutun - 1, satir] != null)
            {
                if (EslesmeKontrol(tas, tumTaslar[sutun + 1, satir], tumTaslar[sutun - 1, satir]))
                {
                    return true;
                }
            }
        }



            return false;
    }

    public void BombaSatirEtkisi(int satir)
    {
        for (int i = 0; i < en; i++)
        {
                if (buzFayans[i, satir])
                {
                    buzFayans[i, satir].HasarAl(1);
                    if (buzFayans[i, satir].vurusNoktasi <= 0)
                    {
                        buzFayans[i, satir] = null;
                        Vector2 geciciPosizyonKutu = new Vector2(i, satir);
                        GameObject birKare = Instantiate(kareSeriUretim, geciciPosizyonKutu, Quaternion.identity) as GameObject;
                        tumKutucuklar[i, satir] = birKare.GetComponent<Kutucuk>();
                    }
                }            
        }
    }

    public void BombaSutunEtkisi(int sutun)
    {
            for (int j = 0; j < boy; j++)
            {
                if (buzFayans[sutun, j])
                {
                    buzFayans[sutun, j].HasarAl(1);
                    if (buzFayans[sutun, j].vurusNoktasi <= 0)
                    {
                        buzFayans[sutun, j] = null;
                        Vector2 geciciPosizyonKutu = new Vector2(sutun, j);
                        GameObject birKare = Instantiate(kareSeriUretim, geciciPosizyonKutu, Quaternion.identity) as GameObject;
                        tumKutucuklar[sutun, j] = birKare.GetComponent<Kutucuk>();
                    }
                }
            }        
    }

    private bool EslesmeKontrol(GameObject kullanilacakTas, GameObject x, GameObject y)
    {
        return (x.GetComponent<M3Tas>().tasinRengi == kullanilacakTas.GetComponent<M3Tas>().tasinRengi && y.GetComponent<M3Tas>().tasinRengi == kullanilacakTas.GetComponent<M3Tas>().tasinRengi) || (x.GetComponent<M3Tas>().tasinTuru == kullanilacakTas.GetComponent<M3Tas>().tasinTuru && y.GetComponent<M3Tas>().tasinTuru == kullanilacakTas.GetComponent<M3Tas>().tasinTuru && turEslesmesiOlacakMi);
    }


    public void TumEslestirmeleriYokEt()
    {
        if (eslesmeBulucu.suankiEslesmeler.Count >= 4)
        {
            YapilacakBombaKontrol2();
        }
        eslesmeBulucu.suankiEslesmeler.Clear();

        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if (tumTaslar[i, j] != null)
                {
                   EslesmeleriYokEt(i, j);
                }
            }
        }
        StartCoroutine(GecEslesmelerYokEt());
    }

    private IEnumerator GecEslesmelerYokEt()
    {
        if (eslesmeBulucu.gecEslesmeler.Count != 0)
        {
            yield return new WaitForSeconds(1f);

            foreach (GameObject gecEslesmeler in eslesmeBulucu.gecEslesmeler)
            {
                EslesmeleriYokEt(gecEslesmeler.GetComponent<M3Tas>().sutun, gecEslesmeler.GetComponent<M3Tas>().satir);
            }

        }
        sesYoneticisi.Oynat("TasDusme");
        StartCoroutine(AsagaDusurCo());
    }

    private void EslesmeleriYokEt(int sutun, int satir)
    {
        if (tumTaslar[sutun, satir].GetComponent<M3Tas>().eslestiMi)
        {
            hedeflerdeOlanEslesmeMi = false;

            if (tumTaslar[sutun, satir].GetComponent<M3Tas>().satirBombasiMi)
            {
                GameObject satirEfekti = Instantiate(satirBombasiEfektiSU, tumTaslar[sutun, satir].transform.position, Quaternion.identity);

                ParticleSystem.TrailModule settings = satirEfekti.transform.GetChild(0).GetComponent<ParticleSystem>().trails;
                settings.colorOverTrail = new ParticleSystem.MinMaxGradient(tumTaslar[sutun, satir].GetComponent<SpriteRenderer>().color);

                ParticleSystem.TrailModule settings2 = satirEfekti.transform.GetChild(1).GetComponent<ParticleSystem>().trails;
                settings2.colorOverTrail = new ParticleSystem.MinMaxGradient(tumTaslar[sutun, satir].GetComponent<SpriteRenderer>().color);

                sesYoneticisi.Oynat("Efekt");

                StartCoroutine(kameraSallanma.Salla(sallanmaSuresi, sallanmaMiktari));

                Destroy(satirEfekti, 2f);
                //Invoke("BombaSatirEtkisi(satir)", 1f);
                BombaSatirEtkisi(satir);
            }

            if (tumTaslar[sutun, satir].GetComponent<M3Tas>().sutunBombasiMi)
            {
                GameObject sutunEfekti = Instantiate(sutunBombasiEfektiSU, tumTaslar[sutun, satir].transform.position, Quaternion.identity);

                ParticleSystem.TrailModule settings = sutunEfekti.transform.GetChild(0).GetComponent<ParticleSystem>().trails;
                settings.colorOverTrail = new ParticleSystem.MinMaxGradient(tumTaslar[sutun, satir].GetComponent<SpriteRenderer>().color);

                ParticleSystem.TrailModule settings2 = sutunEfekti.transform.GetChild(1).GetComponent<ParticleSystem>().trails;
                settings2.colorOverTrail = new ParticleSystem.MinMaxGradient(tumTaslar[sutun, satir].GetComponent<SpriteRenderer>().color);

                sesYoneticisi.Oynat("Efekt");

                StartCoroutine(kameraSallanma.Salla(sallanmaSuresi, sallanmaMiktari));
                Destroy(sutunEfekti, 2f);
                BombaSutunEtkisi(sutun);
            }

            if (tumTaslar[sutun, satir].GetComponent<M3Tas>().bitisikBombasiMi)
            {
                GameObject bombaEfekti = Instantiate(bitisikBombasiEfektiSU, tumTaslar[sutun, satir].transform.position, Quaternion.identity);

                ParticleSystem.TrailModule settings = bombaEfekti.transform.GetChild(0).GetComponent<ParticleSystem>().trails;
                settings.colorOverTrail = new ParticleSystem.MinMaxGradient(tumTaslar[sutun, satir].GetComponent<SpriteRenderer>().color);

                sesYoneticisi.Oynat("Efekt");

                StartCoroutine(kameraSallanma.Salla(sallanmaSuresi, sallanmaMiktari));
                Destroy(bombaEfekti, 2f);
            }

            EslesmeEtkisi(sutun, satir);

            if (hedefYoneticisi != null)
            {
                hedefYoneticisi.HedefiKarsilastir(tumTaslar[sutun, satir].GetComponent<M3Tas>().tasinTuru, tumTaslar[sutun, satir].GetComponent<M3Tas>().tasinRengi);

                if (hedeflerdeOlanEslesmeMi)
                {
                    tumTaslar[sutun, satir].GetComponent<M3Tas>().ToplaVeYokEt( kamera.ScreenToWorldPoint(hedefAlani.transform.position));
                }
            }

            eslesmeBulucu.suankiEslesmeler.Remove(tumTaslar[sutun, satir]);
            GameObject parcacik = Instantiate(YokOlmaEfekti, tumTaslar[sutun, satir].transform.position, Quaternion.identity);
            parcacik.GetComponent<EfektRenk>().RenkDegis(tumTaslar[sutun, satir].GetComponent<SpriteRenderer>().color);
            Destroy(parcacik, 0.5f);

            if (!hedeflerdeOlanEslesmeMi)
            {  
                Destroy(tumTaslar[sutun, satir]);              
            }
            skorYoneticisi.SkoruArttir(anaTasDegeri * komboDegeri);
            tumTaslar[sutun, satir] = null;
        }
    }

    private void EslesmeEtkisi(int sutun, int satir)
    {
        if (kirilabilirFayans[sutun, satir] != null && kilitliFayans[sutun, satir] == null)
        {
            kirilabilirFayans[sutun, satir].HasarAl(1);
            if (kirilabilirFayans[sutun, satir].vurusNoktasi <= 0)
            {
                kirilabilirFayans[sutun, satir] = null;
            }
        }

        if (kilitliFayans[sutun, satir] != null)
        {
            kilitliFayans[sutun, satir].HasarAl(1);
            if (kilitliFayans[sutun, satir].vurusNoktasi <= 0)
            {
                kilitliFayans[sutun, satir] = null;
            }


        }
        else
        {
            BuzaHasarVer(sutun, satir);
            SarmasigaHasarVer(sutun, satir);

        }




    }

    private void BuzaHasarVer(int sutun, int satir)
    {
        if (sutun > 0)
        {
            if (buzFayans[sutun - 1,satir])
            {
                buzFayans[sutun - 1, satir].HasarAl(1);
                if (buzFayans[sutun - 1, satir].vurusNoktasi <= 0)
                {
                    buzFayans[sutun - 1, satir] = null;
                    Vector2 geciciPosizyonKutu = new Vector2(sutun - 1, satir);
                    GameObject birKare = Instantiate(kareSeriUretim, geciciPosizyonKutu, Quaternion.identity) as GameObject;
                    tumKutucuklar[sutun - 1, satir] = birKare.GetComponent<Kutucuk>();
                }
            }
        }
        if (sutun < en - 1)
        {
            if (buzFayans[sutun + 1, satir])
            {
                buzFayans[sutun + 1, satir].HasarAl(1);
                if (buzFayans[sutun + 1, satir].vurusNoktasi <= 0)
                {
                    buzFayans[sutun + 1, satir] = null;
                    Vector2 geciciPosizyonKutu = new Vector2(sutun + 1, satir);
                    GameObject birKare = Instantiate(kareSeriUretim, geciciPosizyonKutu, Quaternion.identity) as GameObject;
                    tumKutucuklar[sutun + 1, satir] = birKare.GetComponent<Kutucuk>();
                }
            }
        }
        if (satir > 0)
        {
            if (buzFayans[sutun, satir - 1])
            {
                buzFayans[sutun, satir - 1].HasarAl(1);
                if (buzFayans[sutun, satir - 1].vurusNoktasi <= 0)
                {
                    buzFayans[sutun, satir - 1] = null;
                    Vector2 geciciPosizyonKutu = new Vector2(sutun, satir - 1);
                    GameObject birKare = Instantiate(kareSeriUretim, geciciPosizyonKutu, Quaternion.identity) as GameObject;
                    tumKutucuklar[sutun, satir - 1] = birKare.GetComponent<Kutucuk>();
                }
            }
        }
        if (satir < boy - 1)
        {
            if (buzFayans[sutun, satir + 1])
            {
                buzFayans[sutun, satir + 1].HasarAl(1);
                if (buzFayans[sutun, satir + 1].vurusNoktasi <= 0)
                {
                    buzFayans[sutun, satir + 1] = null;
                    Vector2 geciciPosizyonKutu = new Vector2(sutun, satir + 1);
                    GameObject birKare = Instantiate(kareSeriUretim, geciciPosizyonKutu, Quaternion.identity) as GameObject;
                    tumKutucuklar[sutun, satir + 1] = birKare.GetComponent<Kutucuk>();
                }
            }
        }
    }

    private void SarmasigaHasarVer(int sutun, int satir)
    {
        if (sutun > 0)
        {
            if (sarmasikFayans[sutun - 1, satir])
            {
                sarmasikFayans[sutun - 1, satir].HasarAl(1);
                if (sarmasikFayans[sutun - 1, satir].vurusNoktasi <= 0)
                {
                    sarmasikFayans[sutun - 1, satir] = null;
                    Vector2 geciciPosizyonKutu = new Vector2(sutun - 1, satir);
                    GameObject birKare = Instantiate(kareSeriUretim, geciciPosizyonKutu, Quaternion.identity) as GameObject;
                    tumKutucuklar[sutun - 1, satir] = birKare.GetComponent<Kutucuk>();
                }
                sarmasikYap = false;
            }
        }
        if (sutun < en - 1)
        {
            if (sarmasikFayans[sutun + 1, satir])
            {
                sarmasikFayans[sutun + 1, satir].HasarAl(1);
                if (sarmasikFayans[sutun + 1, satir].vurusNoktasi <= 0)
                {
                    sarmasikFayans[sutun + 1, satir] = null;
                    Vector2 geciciPosizyonKutu = new Vector2(sutun + 1, satir);
                    GameObject birKare = Instantiate(kareSeriUretim, geciciPosizyonKutu, Quaternion.identity) as GameObject;
                    tumKutucuklar[sutun + 1, satir] = birKare.GetComponent<Kutucuk>();
                }
                sarmasikYap = false;
            }
        }
        if (satir > 0)
        {
            if (sarmasikFayans[sutun, satir - 1])
            {
                sarmasikFayans[sutun, satir - 1].HasarAl(1);
                if (sarmasikFayans[sutun, satir - 1].vurusNoktasi <= 0)
                {
                    sarmasikFayans[sutun, satir - 1] = null;
                    Vector2 geciciPosizyonKutu = new Vector2(sutun, satir - 1);
                    GameObject birKare = Instantiate(kareSeriUretim, geciciPosizyonKutu, Quaternion.identity) as GameObject;
                    tumKutucuklar[sutun, satir - 1] = birKare.GetComponent<Kutucuk>();
                }
                sarmasikYap = false;
            }
        }
        if (satir < boy - 1)
        {
            if (sarmasikFayans[sutun, satir + 1])
            {
                sarmasikFayans[sutun, satir + 1].HasarAl(1);
                if (sarmasikFayans[sutun, satir + 1].vurusNoktasi <= 0)
                {
                    sarmasikFayans[sutun, satir + 1] = null;
                    Vector2 geciciPosizyonKutu = new Vector2(sutun, satir + 1);
                    GameObject birKare = Instantiate(kareSeriUretim, geciciPosizyonKutu, Quaternion.identity) as GameObject;
                    tumKutucuklar[sutun, satir + 1] = birKare.GetComponent<Kutucuk>();
                }
                sarmasikYap = false;
            }
        }
    }

    private IEnumerator AsagaDusurCo()
    {
        Debug.Log("Asagi Dusur");
        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if (!bosAlanlar[i, j] && tumTaslar[i, j] == null && !buzFayans[i,j] && !sarmasikFayans[i,j])
                {

                    for (int k = j + 1; k < boy; k++)
                    {
                        if (tumTaslar[i, k] != null)
                        {
                            sesYoneticisi.Oynat("TasDusme");
                            tumTaslar[i, k].GetComponent<M3Tas>().satir = j;
                            tumTaslar[i, k] = null;
                            break;
                        }
                    }
                }
            }

        }
        yield return new WaitForSeconds(dolmaSuresi * 0.5f);
        StartCoroutine(TahtayiDoldurCo());

    }


    private bool TahtadaEslesmeVarMi()
    {
       /* eslesmeBulucu.suankiEslesmeler.Clear();*/ // deneme asamasi
        eslesmeBulucu.TumEslestirmeleriBul();
        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if (tumTaslar[i, j] != null)
                {
                    if (tumTaslar[i, j].GetComponent<M3Tas>().eslestiMi)
                    {
                        Debug.Log("EslesmeBulundu");
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void TahtayiDoldur()
    {
        Debug.Log("Tahtayi dolduruluyor");
        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if (tumTaslar[i, j] == null && !bosAlanlar[i, j] && !buzFayans[i,j] && !sarmasikFayans[i,j])
                {
                    Vector2 geciciPosizyon = new Vector2(i, j + ilkNoktaMesafesi);
                    int kullanilacakTas = Random.Range(0, taslar.Length);

                    int maxIterations = 0;
                    while (MatchesAt2(i,j, taslar[kullanilacakTas]) && maxIterations < 100)
                    {
                        maxIterations++;
                        kullanilacakTas = Random.Range(0, taslar.Length);
                    }
                    

                    GameObject tas = Instantiate(taslar[kullanilacakTas], geciciPosizyon, Quaternion.identity);
                    tumTaslar[i, j] = tas;
                    tas.GetComponent<M3Tas>().satir = j;
                    tas.GetComponent<M3Tas>().sutun = i;
                }
            }
        }
    }

    private IEnumerator TahtayiDoldurCo()
    {

        yield return new WaitForSeconds(dolmaSuresi);
        TahtayiDoldur();
        yield return new WaitForSeconds(dolmaSuresi);


        while (TahtadaEslesmeVarMi())
        {
            yield return new WaitForSeconds(dolmaSuresi);
            TumEslestirmeleriYokEt();
            //suankiDurum = OyunDurumu.hareket;
            yield break; // belki bunu değiştirebilirm
        }

        SarmasiklaKontrol();
        yield return new WaitForSeconds(dolmaSuresi);

        Debug.Log("Done Refilling");
        System.GC.Collect();

        if (TahtadaEslesmeVarMi())
        {
            TumEslestirmeleriYokEt();
            yield break;
        }

        //yield return new WaitForSeconds(dolmaSuresi);


        if (suankiDurum == OyunDurumu.bekle && suankiDurum != OyunDurumu.kazandin && suankiDurum != OyunDurumu.kaybettin)   // oyun kazandiginda ,durdugunda kaybettiginde hareket durumuna gecemiyor
        {
            KilitlenmeKontrol();


            yield return new WaitForSeconds(dolmaSuresi);

            suankiDurum = OyunDurumu.hareket;
            sarmasikYap = true;

        }
    }

    private void SarmasiklaKontrol()
    {
        //sarmasik olusturmak icin sarmasik arrayini kontrol et

        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if (sarmasikFayans[i, j] != null && sarmasikYap)
                {
                    //Sarmasik ureten fonksiyonu cagir
                    YeniSarmasikYap();
                    return;
                }
            }
        }
    }

    private Vector2 BitisikKontrol(int sutun, int satir)
    {
            if (sutun < en - 1 && tumTaslar[sutun + 1, satir])
            {
                return Vector2.right;
            }
            if ( sutun > 0 && tumTaslar[sutun - 1, satir])
            {
                return Vector2.left;
            }
            if (satir < boy - 1 && tumTaslar[sutun, satir + 1])
            {
                return Vector2.up;
            }
            if (satir > 0 && tumTaslar[sutun, satir - 1])
            {
                return Vector2.down;
            }

        return Vector2.zero;
    }

    private void YeniSarmasikYap()
    {
        bool sarmasik = false;
        int tekrar = 0;
        while (!sarmasik && tekrar < 200)
        {
            int yeniX = Random.Range(0, en);
            int yeniY = Random.Range(0, boy);

            if (sarmasikFayans[yeniX, yeniY])
            {
                Vector2 bitisik = BitisikKontrol(yeniX, yeniY);
                if(bitisik != Vector2.zero)
                {
                    Destroy(tumTaslar[yeniX + (int)bitisik.x, yeniY + (int)bitisik.y]);
                    Destroy(tumKutucuklar[yeniX + (int)bitisik.x, yeniY + (int)bitisik.y].gameObject);
                    Vector2 geciciPoz = new Vector2(yeniX + (int)bitisik.x, yeniY + (int)bitisik.y);
                    GameObject fayans = Instantiate(sarmasikFayansSU, geciciPoz, Quaternion.identity);
                    sarmasikFayans[yeniX + (int)bitisik.x, yeniY + (int)bitisik.y] = fayans.GetComponent<KirilabilirFayans>();
                    sarmasik = true;
                }
            }
            tekrar++;
        }
    }

    private void OnayPaneliniAyarla()   // Bir sonraki Bolumu ayarlıyor.
    {
        if (onayPaneli != null)
        {
            //if (seviye + 2 >= oyunVerisi.veriKaydet.aktifMi.GetLength(1))  // Eski Bolum tasarımında ana bölüm ekranina goturuyor
            //{
            //    onayPaneli.yuklencekSeviye = "ModSecimEkrani";
            //}
            onayPaneli.seviye = seviye + 2;
            onayPaneli.alemNo = alemNo;
        }
    }



    private bool EslesmeleriKontrolEt()
    {
        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if (tumTaslar[i, j] != null)
                {
                    if (i < en - 2)
                    {
                        if (tumTaslar[i + 1, j] != null && tumTaslar[i + 2, j] != null)
                        {
                            if (tumTaslar[i, j].GetComponent<M3Tas>().EslesmeKontrol(tumTaslar[i + 1, j].GetComponent<M3Tas>(), tumTaslar[i + 2, j].GetComponent<M3Tas>()))
                            {
                                return true;
                            }
                        }
                    }

                    if (j < boy - 2)
                    {
                        if (tumTaslar[i, j + 1] != null && tumTaslar[i, j + 2] != null)
                        {
                            if (tumTaslar[i, j].GetComponent<M3Tas>().EslesmeKontrol(tumTaslar[i, j + 1].GetComponent<M3Tas>(), tumTaslar[i, j + 2].GetComponent<M3Tas>()))
                            {
                                return true;
                            }
                        }
                    }

                }
            }
        }
        return false;
    }

    private void KilitlenmeKontrol()
    {
        bool eslesmeVarMi = false;
        for (int i = 0; i < en && !eslesmeVarMi; i++)
        {
            for (int j = 0; j < boy && !eslesmeVarMi; j++)
            {
                if (tumTaslar[i, j])
                {
                    //Degistir
                    if (DegistiripKontrolEt(tumTaslar[i, j].GetComponent<M3Tas>().tasinTuru, i,j))
                    {
                        eslesmeVarMi = true;
                        //Debug.Log("Degisim Var Aloooooooooo");
                        break;
                    }

                }
            }

            if (eslesmeVarMi)
            {
                break;
            }
        }

        if (!eslesmeVarMi)
        {
            //Tahtayi Kar
            Debug.Log("Eslesme Bulunamadi !!!!!!!!!!!!");
            StartCoroutine(TahtayiKaristir());
        }
        else
        {
            Debug.Log("Eslesme VAR GECEBilirsin");
        }
    }



    private bool DegistiripKontrolEt(TasTuru tasinTuru,int sutun,int satir)
    {

        void Degistir(int digerSutun, int digerSatir)
        {
            if (digerSutun >= 0 && digerSutun < en && digerSatir >= 0 && digerSatir < boy && tumTaslar[digerSutun, digerSatir])
            {
                //Debug.Log("Degistirildi" + tasinTuru + sutun + satir);

                GameObject tut = tumTaslar[digerSutun, digerSatir].gameObject;
                tumTaslar[digerSutun, digerSatir] = tumTaslar[sutun, satir];
                tumTaslar[sutun, satir] = tut;

                //Debug.Log("Degistirildi ve Denendi:" + tasinTuru + sutun + satir + " Yeni degerler:" + digerSutun + digerSatir );
            }
            //else
            //{
            //    Debug.Log("Degistirilmedi" + tasinTuru + sutun + satir + digerSatir + digerSutun);
            //}

        }

        bool EslesenBak(int sutunY,int satirY, GameObject obje)
        {
            Degistir(sutunY, satirY);

            if (EslesmeleriKontrolEt())
            {
                Degistir(sutunY, satirY);
                return true;
            }
            Degistir(sutunY, satirY);
            return false;
        }

        bool digerTasKontrol(int dSutun,int dSatir)
        {
            return ( dSutun<en && dSutun >=0 && dSatir < boy && dSatir >= 0 && tumTaslar[dSutun,dSatir]&& EslesenBak(dSutun, dSatir, tumTaslar[dSutun,dSatir]));
        }

        if (tasinTuru == TasTuru.At)
        {

             if(digerTasKontrol(sutun + 2, satir + 1))
            {
                return true;
            }


            else if(digerTasKontrol(sutun + 2, satir - 1))
            {
                return true;
            }

            else if(digerTasKontrol(sutun - 2, satir + 1))
            {
                return true;
            }

            else if(digerTasKontrol(sutun - 2, satir - 1))
            {
                return true;
            }

            else if(digerTasKontrol(sutun - 1, satir + 2))
            {
                return true;
            }

            else if(digerTasKontrol(sutun + 1, satir + 2))
            {
                return true;
            }

            else if (digerTasKontrol(sutun - 1, satir - 2))
            {
                return true;
            }

            else if (digerTasKontrol(sutun + 1, satir - 2))
            {
                return true;

            }

        }
        else if (tasinTuru == TasTuru.Fil)
        {
            bool engelVarMi1 = false;                // tasin gidecegi dort yonden biri icin engel olup olmadigi.
            bool engelVarMi2 = false;
            bool engelVarMi3 = false;
            bool engelVarMi4 = false;

            for (int i = 0; i < en; i++)
            {
                if (!engelVarMi1)
                {
                    engelVarMi1 = TasinOnundeEngelKontrol(sutun + i, satir - i);
                    if (digerTasKontrol(sutun + i, satir - i))
                    {
                        return true;

                    }
                }
                else if (!engelVarMi2)
                {
                    engelVarMi2 = TasinOnundeEngelKontrol(sutun - i, satir - i);
                    if (digerTasKontrol(sutun - i, satir - i))
                    {
                        return true;

                    }
                }
                else if (!engelVarMi3)
                {
                    engelVarMi3 = TasinOnundeEngelKontrol(sutun + i, satir + i);
                    if (digerTasKontrol(sutun + i, satir + i))
                    {
                        return true;

                    }
                }
                else if (!engelVarMi4)
                {
                    engelVarMi4 = TasinOnundeEngelKontrol(sutun - i, satir + i);
                    if (digerTasKontrol(sutun - i, satir + i))
                    {
                        return true;

                    }
                }
            }

        }
        else if (tasinTuru == TasTuru.Kale)
        {
            bool engelVarMi1 = false;                // tasin gidecegi dort yonden biri icin engel olup olmadigi.
            bool engelVarMi2 = false;
            bool engelVarMi3 = false;
            bool engelVarMi4 = false;
            for (int i = 0; i < en; i++)
            {
                if (!engelVarMi1)
                {
                    engelVarMi1 = TasinOnundeEngelKontrol(sutun + i, satir);
                    if(digerTasKontrol(sutun + i, satir))
                    {

                        return true;
                    }
                }

                else if (!engelVarMi2)
                {
                    engelVarMi2 = TasinOnundeEngelKontrol(sutun, satir + i);
                    if (digerTasKontrol(sutun, satir + i))
                    {

                        return true;
                    }
                }

                else if (!engelVarMi3)
                {
                    engelVarMi3 = TasinOnundeEngelKontrol(sutun - i, satir);
                    if (digerTasKontrol(sutun - i, satir))
                    {

                        return true;
                    }
                }

                else if (!engelVarMi4)
                {
                    engelVarMi4 = TasinOnundeEngelKontrol(sutun, satir - i);
                    if (digerTasKontrol(sutun, satir - i))
                    {
                        return true;
                    }
                }

            }
        }
        else if (tasinTuru == TasTuru.Kral)
        {
            if (digerTasKontrol(sutun + 1, satir))
            {
                return true;
            }

            else if (digerTasKontrol(sutun - 1, satir))
            {
                return true;
            }

            else if (digerTasKontrol(sutun, satir + 1))
            {
                return true;
            }

            else if (digerTasKontrol(sutun, satir - 1))
            {
                return true;
            }

            else if (digerTasKontrol(sutun + 1, satir + 1))
            {
                return true;
            }

            else if (digerTasKontrol(sutun - 1, satir - 1))
            {
                return true;
            }

            else if (digerTasKontrol(sutun - 1, satir + 1))
            {
                return true;
            }

            else if (digerTasKontrol(sutun + 1, satir - 1))
            {
                return true;
            }


        }
        else if (tasinTuru == TasTuru.Piyon)
        {
            if (digerTasKontrol(sutun + 1, satir))
            {
                return true;
            }

            else if (digerTasKontrol(sutun - 1, satir))
            {
                return true;
            }

            else if (digerTasKontrol(sutun, satir + 1))
            {
                return true;
            }

            else if (digerTasKontrol(sutun, satir - 1))
            {
                return true;
            }
        }
        else
        {
            for (int i = 0; i < en; i++)
            {
                if (digerTasKontrol(sutun + i, satir))
                {
                    return true;
                }

                else if (digerTasKontrol(sutun, satir + i))
                {
                    return true;
                }


                else if (digerTasKontrol(sutun - i, satir))
                {
                    return true;
                }


                else if (digerTasKontrol(sutun, satir - i))
                {
                    return true;
                }

            }

            for (int i = 0; i < en; i++)
            {
                if (digerTasKontrol(sutun + i, satir - i))
                {
                    return true;
                }


                else if (digerTasKontrol(sutun - i, satir - i))
                {
                    return true;
                }

                else if (digerTasKontrol(sutun + i, satir + i))
                {
                    return true;
                }


                else if (digerTasKontrol(sutun - i, satir + i))
                {
                    return true;
                }
            }
        }


        return false;


    }




    private IEnumerator TahtayiKaristir()
    {
        Debug.Log("Tahta Karişiyor");
        suankiDurum = OyunDurumu.bekle;
        yield return new WaitForSeconds(.5f);

        List<GameObject> yeniTahta = new List<GameObject>();

        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if (tumTaslar[i, j] != null && !bosAlanlar[i, j] && !buzFayans[i, j] && !sarmasikFayans[i, j])
                {
                    yeniTahta.Add(tumTaslar[i, j]);
                }
            }
        }

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if (tumTaslar[i, j] != null && !bosAlanlar[i, j] && !buzFayans[i, j] && !sarmasikFayans[i, j] )
                {
                    int kullanilacakTas = Random.Range(0, yeniTahta.Count);

                    int tavanYineleme = 0;
                    while (MatchesAt(i, j, yeniTahta[kullanilacakTas]) && tavanYineleme < 100)
                    {
                        kullanilacakTas = Random.Range(0, yeniTahta.Count);
                        tavanYineleme++;

                    }
                    M3Tas tas = yeniTahta[kullanilacakTas].GetComponent<M3Tas>();

                    tas.sutun = i;
                    tas.satir = j;
                    tumTaslar[i, j] = yeniTahta[kullanilacakTas];
                    yeniTahta.Remove(yeniTahta[kullanilacakTas]);
                }
            }

        }

        yield return new WaitForSeconds(.5f);


        //KilitlenmeKontrol();

        suankiDurum = OyunDurumu.hareket;
    }

    public void TahtayiKar()
    {
        StartCoroutine(TahtayiKaristir());
    }

}
