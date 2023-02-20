using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OyunDurumu2 { hareket, bekle, kazandin, kaybettin, durdur };

public enum FayansTuru { kirilabilir, bos, normal}

[System.Serializable]
public class FayansTipiYeri
{
    public int x;
    public int y;
    public FayansTuru fayTuru;
}

public class Tahta : MonoBehaviour
{

    [Header ("Scriptable Obje Seyleri")]
    public Alem alem;
    public int seviye;

    public OyunDurumu2 suankiDurum = OyunDurumu2.hareket;
    [Header("Tahta Boyutlari")]
    public int en;
    public int boy;
    public int ilkNokta;
    
    [Header("SeriUretimler")]
    public GameObject kareSeriUretim;
    public GameObject kirilabilirFayansSeriUretim;
    public GameObject[] taslar;

    [Header("Yerlesim")]
    public FayansTipiYeri[] tahtaYerlesim;
    private ArkaPlanKutucuklari[,] tumKareler;
    private bool[,] bosAlanlar;
    private ArkaPlanFayans[,] kirilabilirFayans;
    public GameObject[,] tumTaslar;
    public Tas suankiTas;
    private EslesmeBulSM3 eslestirmeBulucu;
    public int anaTasDegeri = 1;
    private int komboDegeri = 1;
    private SkorYoneticisi skorYoneticisi;
    public HedefYoneticisi hedefYoneticisi;
    public float dolmaSuresi = 0.5f;

    private Ipucu ipucu;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("SuankiSeviye"))
        {
            seviye = PlayerPrefs.GetInt("SuankiSeviye");
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
                    //skorHedefleri = alem.seviyeler[seviye].skorHedefleri;
                }
            }
        }
    }

    private void Start()
    {
        hedefYoneticisi = FindObjectOfType<HedefYoneticisi>();
        skorYoneticisi = FindObjectOfType<SkorYoneticisi>();
        kirilabilirFayans = new ArkaPlanFayans[en, boy];
        tumKareler = new ArkaPlanKutucuklari[en, boy];
        eslestirmeBulucu = FindObjectOfType<EslesmeBulSM3>();
        tumTaslar = new GameObject[en, boy];
        bosAlanlar = new bool[en, boy];
        TahtayiOlustur();
        suankiDurum = OyunDurumu2.durdur;

        ipucu = FindObjectOfType<Ipucu>();

    }

    public void BosFayanslariAyarla()
    {
        for (int i = 0; i < tahtaYerlesim.Length; i++)
        {
            if (tahtaYerlesim[i].fayTuru == FayansTuru.bos)
            {
                bosAlanlar[tahtaYerlesim[i].x, tahtaYerlesim[i].y] = true;
            }

        }
    }

    public void KirilabilirFayanslariAyarla()
    {
        for (int i = 0; i < tahtaYerlesim.Length; i++) 
        {
            if (tahtaYerlesim[i].fayTuru == FayansTuru.kirilabilir)
            {
                Vector2 geciciPoz = new Vector2(tahtaYerlesim[i].x, tahtaYerlesim[i].y);
                GameObject fayans = Instantiate(kirilabilirFayansSeriUretim, geciciPoz, Quaternion.identity);
                kirilabilirFayans[tahtaYerlesim[i].x, tahtaYerlesim[i].y] = fayans.GetComponent<ArkaPlanFayans>();
            }
        }
    }

    private void TahtayiOlustur()
    {
        BosFayanslariAyarla();
        KirilabilirFayanslariAyarla();
        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if (!bosAlanlar[i, j])
                {
                    Vector2 geciciPosizyon = new Vector2(i, j + ilkNokta);
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
                    tavanYineleme = 0;
                    GameObject tas = Instantiate(taslar[kullanilacakTas], geciciPosizyon, Quaternion.identity);
                    tas.GetComponent<Tas>().satir = j;
                    tas.GetComponent<Tas>().sutun = i;
                    tas.transform.parent = this.transform;
                    tas.name = "( " + i + ", " + j + " )";
                    tumTaslar[i, j] = tas;
                }
            }
        }

        if (KilitlenmeKontrol())
        {
            TahtayiKaristir();
            Debug.Log("Oyun Kilitlendi");
        }
    }

    private bool MatchesAt(int sutun, int satir, GameObject tas)
    {
        if (sutun > 1 && satir > 1)
        {
            if (tumTaslar[sutun - 1, satir] != null && tumTaslar[sutun - 2, satir] != null)
            {
                if (tumTaslar[sutun - 1, satir].tag == tas.tag && tumTaslar[sutun - 2, satir].tag == tas.tag)
                {
                    return true;
                }
            }
            if (tumTaslar[sutun, satir - 1] != null && tumTaslar[sutun, satir - 2] != null)
            {
                if (tumTaslar[sutun, satir - 1].tag == tas.tag && tumTaslar[sutun, satir - 2].tag == tas.tag)
                {
                    return true;
                }
            }

        }else if(sutun <=1 || satir <= 1)
        {
            if(satir > 1)
            {
                if (tumTaslar[sutun, satir - 1] != null && tumTaslar[sutun, satir - 2] != null)
                {
                    if (tumTaslar[sutun, satir - 1].tag == tas.tag && tumTaslar[sutun, satir - 2].tag == tas.tag)
                    {
                        return true;
                    }
                }
            }
            if (sutun > 1)
            {
                if (tumTaslar[sutun - 1, satir] != null && tumTaslar[sutun - 2, satir] != null)
                {
                    if (tumTaslar[sutun - 1, satir].tag == tas.tag && tumTaslar[sutun - 2, satir].tag == tas.tag)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private int SutunYaDaSatir()
    {
        List<GameObject> eslesmeKopyasi=eslestirmeBulucu.suankiEslesmeler as List<GameObject>;
        for (int i = 0; i < eslesmeKopyasi.Count; i++)
        {
            Tas buTas = eslesmeKopyasi[i].GetComponent<Tas>();

            int satir = buTas.satir;
            int sutun = buTas.sutun;
            int sutunEslenmeleri = 0;
            int satirEslenmeleri = 0;

            for (int j = 0; j < eslesmeKopyasi.Count; j++)
            {
                Tas sonrakiTas = eslesmeKopyasi[j].GetComponent<Tas>();
                if(sonrakiTas == buTas)
                {
                    continue;
                }
                if(sonrakiTas.sutun ==buTas.sutun && sonrakiTas.CompareTag(buTas.tag))
                {
                    sutunEslenmeleri++;
                }
                if (sonrakiTas.satir == buTas.satir && sonrakiTas.CompareTag(buTas.tag))
                {
                    satirEslenmeleri++;
                }
            }

            // return 3 satir sutun bombasi
            // return 2  etraf bombasi
            // return 1  renk bombasi

            if (sutunEslenmeleri == 4 || satirEslenmeleri == 4)
            {
                return 1;
            }
            if (sutunEslenmeleri == 2 || satirEslenmeleri == 2)
            {
                return 2;
            }
            if (sutunEslenmeleri == 3 || satirEslenmeleri == 3)
            {
                return 3;
            }
        }
        return 0;
      /*  int yataySayisi = 0;
        int dikeySayisi = 0;
        Tas ilkTas = eslestirmeBulucu.suankiEslesmeler[0].GetComponent<Tas>();
        if (ilkTas != null)
        {
            foreach(GameObject parca in eslestirmeBulucu.suankiEslesmeler)
            {
                Tas gtas = parca.GetComponent<Tas>();
                if (gtas.satir == ilkTas.satir)
                {
                    yataySayisi++;
                }
                if (gtas.sutun == ilkTas.sutun)
                {
                    dikeySayisi++;
                }
            }
        }

        return (yataySayisi == 5 || dikeySayisi == 5); */
    }
    private void YapilacakBombaKontrol()
    {
        if (eslestirmeBulucu.suankiEslesmeler.Count > 3)
        {
            int eslesmeTipi = SutunYaDaSatir();
            if (eslesmeTipi == 1)
            {
                if (suankiTas != null)
                {
                    if (suankiTas.eslestiMi)
                    {
                        if (!suankiTas.renkBombasiMi)
                        {
                            suankiTas.eslestiMi = false;
                            suankiTas.RenkBombasiYap();
                        }
                    }
                    else
                    {
                        if (suankiTas.digerTas != null)
                        {
                            Tas digerTas = suankiTas.digerTas.GetComponent<Tas>();
                            if (digerTas.eslestiMi)
                            {
                                if (!digerTas.renkBombasiMi)
                                {
                                    digerTas.eslestiMi = false;
                                    digerTas.RenkBombasiYap();
                                }
                            }
                        }
                    }
                }
            }
            else if(eslesmeTipi == 2)
            {
                if (suankiTas != null)
                {
                    if (suankiTas.eslestiMi)
                    {
                        if (!suankiTas.bitisikBombasiMi)
                        {
                            suankiTas.eslestiMi = false;
                            suankiTas.BitisikBombasiYap();
                        }
                    }
                    else
                    {
                        if (suankiTas.digerTas != null)
                        {
                            Tas digerTas = suankiTas.digerTas.GetComponent<Tas>();
                            if (digerTas.eslestiMi)
                            {
                                if (!digerTas.bitisikBombasiMi)
                                {
                                    digerTas.eslestiMi = false;
                                    digerTas.BitisikBombasiYap();
                                }
                            }
                        }
                    }
                }
            }
            else if(eslesmeTipi == 3)
            {
                eslestirmeBulucu.BombaKontrol();
            }
        }

      /*  if(eslestirmeBulucu.suankiEslesmeler.Count == 4 || eslestirmeBulucu.suankiEslesmeler.Count == 7)
        {
            eslestirmeBulucu.BombaKontrol();
        }
        if(eslestirmeBulucu.suankiEslesmeler.Count == 5 || eslestirmeBulucu.suankiEslesmeler.Count == 8)
        {
            if (SutunYaDaSatir())
            {
                if (suankiTas != null)
                {
                    if (suankiTas.eslestiMi)
                    {
                        if (!suankiTas.renkBombasiMi)
                        {
                            suankiTas.eslestiMi = false;
                            suankiTas.RenkBombasiYap();
                        }
                    }
                    else
                    {
                        if (suankiTas.digerTas != null)
                        {
                            Tas digerTas = suankiTas.digerTas.GetComponent<Tas>();
                            if (digerTas.eslestiMi)
                            {
                                if (!digerTas.renkBombasiMi)
                                {
                                    digerTas.eslestiMi = false;
                                    digerTas.RenkBombasiYap();
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                if (suankiTas != null)
                {
                    if (suankiTas.eslestiMi)
                    {
                        if (!suankiTas.bitisikBombasiMi)
                        {
                            suankiTas.eslestiMi = false;
                            suankiTas.BitisikBombasiYap();
                        }
                    }
                    else
                    {
                        if (suankiTas.digerTas != null)
                        {
                            Tas digerTas = suankiTas.digerTas.GetComponent<Tas>();
                            if (digerTas.eslestiMi)
                            {
                                if (!digerTas.bitisikBombasiMi)
                                {
                                    digerTas.eslestiMi = false;
                                    digerTas.BitisikBombasiYap();
                                }
                            }
                        }
                    }
                }

            }
        }
           */
    }

    private void EslesmeleriYokEt(int sutun,int satir)
    {
        if (tumTaslar[sutun, satir].GetComponent<Tas>().eslestiMi)
        {
            if (eslestirmeBulucu.suankiEslesmeler.Count >= 4 )
            {
                YapilacakBombaKontrol();
            }

            if (kirilabilirFayans[sutun, satir] != null)
            {
                kirilabilirFayans[sutun, satir].HasarAl(1);
                if (kirilabilirFayans[sutun, satir].vurusNoktasi <= 0)
                {
                    kirilabilirFayans[sutun, satir] = null;
                }
            }

            if (hedefYoneticisi != null)
            {
                hedefYoneticisi.HedefiKarsilastir(tumTaslar[sutun, satir].tag);
                hedefYoneticisi.HedefVerileriGuncelle();
            }


            Destroy(tumTaslar[sutun, satir]);
            skorYoneticisi.SkoruArttir(anaTasDegeri * komboDegeri);
            tumTaslar[sutun, satir] = null;
        }
    }

    public void TumEslestirmeleriYokEt()
    {
        ipucu.IpucuSuresiniDondur();
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
        eslestirmeBulucu.suankiEslesmeler.Clear();
        StartCoroutine(AsagaDusurCo2());
    }

    private IEnumerator AsagaDusurCo2()
    {
        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if(!bosAlanlar[i,j] && tumTaslar[i, j] == null)
                {
                    for (int k = j + 1; k < boy; k++)
                    {
                        if (tumTaslar[i, k] != null)
                        {
                            tumTaslar[i, k].GetComponent<Tas>().satir = j;
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
    private IEnumerator AsagaDusurCo()
    {
        int bosKareSayisi = 0;
        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if (tumTaslar[i, j] == null)
                {
                    bosKareSayisi++;
                }else if (bosKareSayisi > 0)
                {
                    tumTaslar[i, j].GetComponent<Tas>().satir -= bosKareSayisi;
                    tumTaslar[i, j] = null;
                }
            }
            bosKareSayisi = 0;
        }
        yield return new WaitForSeconds(dolmaSuresi * 0.5f);
        StartCoroutine(TahtayiDoldurCo());
    }

    private void TahtayiDoldur()
    {
        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if (tumTaslar[i, j] == null && !bosAlanlar[i,j])
                {
                    Vector2 geciciPosizyon = new Vector2(i, j + ilkNokta);
                    int kullanilacakTas = Random.Range(0, taslar.Length);
                    int tavYineleme = 0;

                    while (MatchesAt(i, j, taslar[kullanilacakTas]) && tavYineleme < 100)
                    {
                        tavYineleme++;
                        kullanilacakTas = Random.Range(0, taslar.Length);
                    }

                    tavYineleme = 0;
                    GameObject tas = Instantiate(taslar[kullanilacakTas], geciciPosizyon, Quaternion.identity);
                    tumTaslar[i, j] = tas;
                    tas.GetComponent<Tas>().satir = j;
                    tas.GetComponent<Tas>().sutun = i;

                }
            }
        }
    }

    private bool TahtadakiEslenmisler()
    {
        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if (tumTaslar[i, j] != null)
                {
                    if (tumTaslar[i, j].GetComponent<Tas>().eslestiMi)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }


    private IEnumerator TahtayiDoldurCo()
    {
        yield return new WaitForSeconds(dolmaSuresi);
        TahtayiDoldur(); // bu fonks ondeydi sona aldim sonraki hata olusturuyormus
        

        while (TahtadakiEslenmisler())
        {
            komboDegeri ++;
            TumEslestirmeleriYokEt();
            yield return new WaitForSeconds(2 * dolmaSuresi);
        }
        eslestirmeBulucu.suankiEslesmeler.Clear();
        suankiTas = null;

        if (KilitlenmeKontrol())
        {
           StartCoroutine(TahtayiKaristir());
            Debug.Log("Oyun Kilitlendi");
        }
        yield return new WaitForSeconds(dolmaSuresi);
        suankiDurum = OyunDurumu2.hareket;
        komboDegeri = 1;

    }


    private void ParcalariYerDegis(int sutun, int satir, Vector2 yon)
    {
        GameObject tut = tumTaslar[sutun + (int)yon.x, satir + (int)yon.y] as GameObject;
        tumTaslar[sutun + (int)yon.x, satir + (int)yon.y] = tumTaslar[sutun, satir];
        tumTaslar[sutun, satir] = tut;

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
                            if (tumTaslar[i + 1, j].tag == tumTaslar[i, j].tag && tumTaslar[i + 2, j].tag == tumTaslar[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }

                    if (j < boy - 2)
                    {
                        if (tumTaslar[i, j + 1] != null && tumTaslar[i, j + 2] != null)
                        {
                            if (tumTaslar[i, j + 1].tag == tumTaslar[i, j].tag && tumTaslar[i, j + 2].tag == tumTaslar[i, j].tag)
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

    public bool DegistiripKontrolEt(int sutun, int satir, Vector2 yon)
    {
        ParcalariYerDegis(sutun, satir, yon);
        if (EslesmeleriKontrolEt())
        {
            ParcalariYerDegis(sutun, satir, yon);
            return true;
        }
        ParcalariYerDegis(sutun, satir, yon);
        return false;
    }

    private bool KilitlenmeKontrol()
    {
        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if (tumTaslar[i, j] != null)
                {
                    if (i < en - 2)
                    {
                        if (DegistiripKontrolEt(i, j, Vector2.right))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (DegistiripKontrolEt(i, j, Vector2.left))
                        {
                            return false;
                        }
                    }

                    if (j < boy - 2)
                    {
                        if (DegistiripKontrolEt(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (DegistiripKontrolEt(i, j, Vector2.down))
                        {
                            return false;
                        }
                    }
                }
            }

        }
        return true;
    }

    private IEnumerator TahtayiKaristir()
    {
        yield return new WaitForSeconds(.5f);

        List<GameObject> yeniTahta = new List<GameObject>();

        for (int i = 0; i < en; i++)
        {
            for (int j = 0; j < boy; j++)
            {
                if (tumTaslar[i, j] != null)
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
                if (!bosAlanlar[i, j])
                {
                    int kullanilacakTas = Random.Range(0, yeniTahta.Count);
                    
                    int tavanYineleme = 0;
                    while (MatchesAt(i, j, yeniTahta[kullanilacakTas]) && tavanYineleme < 100)
                    {
                        kullanilacakTas = Random.Range(0, yeniTahta.Count);
                        tavanYineleme++;

                    }
                    Tas tas = yeniTahta[kullanilacakTas].GetComponent<Tas>();
                    tavanYineleme = 0;

                    tas.sutun = i;
                    tas.satir = j;
                    tumTaslar[i, j] = yeniTahta[kullanilacakTas];
                    yeniTahta.Remove(yeniTahta[kullanilacakTas]);
                }
            }

        }

        if (KilitlenmeKontrol())
        {
            TahtayiKaristir();
        }

    }


}
