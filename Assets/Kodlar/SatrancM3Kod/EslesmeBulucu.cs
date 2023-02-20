using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EslesmeBulucu : MonoBehaviour
{
    private M3Tahta tahta;
    public List<GameObject> suankiEslesmeler = new List<GameObject>();
    public List<GameObject> gecEslesmeler = new List<GameObject>();

    void Start()
    {
        tahta = FindObjectOfType<M3Tahta>();
    }


    public void TumEslestirmeleriBul()
    {
        if(tahta.suankiDurum == OyunDurumu.hareket)  //14.07.22 kazandin kaybettin durumu icin
        {
           tahta.suankiDurum = OyunDurumu.bekle;
        }
        StartCoroutine(TumEslestirmeleriBulCo());
    }
    private List<GameObject> RenkBombasiMi(M3Tas tas1, M3Tas tas2, M3Tas tas3)
    {
        List<GameObject> suankiTaslar = new List<GameObject>();
        if (tas1.renkBombasiMi && tas1.gameObject != tahta.tiklanan1 && tas1.gameObject != tahta.tiklanan2)
            suankiTaslar.Union(RenkTurdekiTaslariAl(tas1.renkBombasiRengi, tas1.tasinTuru));

        if (tas2.renkBombasiMi && tas2.gameObject != tahta.tiklanan1 && tas2.gameObject != tahta.tiklanan2)
            suankiTaslar.Union(RenkTurdekiTaslariAl(tas2.renkBombasiRengi, tas2.tasinTuru));

        if (tas3.renkBombasiMi && tas3.gameObject != tahta.tiklanan1 && tas3.gameObject != tahta.tiklanan2)
            suankiTaslar.Union(RenkTurdekiTaslariAl(tas3.renkBombasiRengi, tas3.tasinTuru));


        return suankiTaslar;

    }

    private List<GameObject> BitisikBombaMi(M3Tas tas1, M3Tas tas2, M3Tas tas3)
    {
        List<GameObject> suankiTaslar = new List<GameObject>();
        if (tas1.bitisikBombasiMi)
            suankiTaslar.Union(BitisikTaslariAl(tas1.sutun, tas1.satir));

        if (tas2.bitisikBombasiMi)
            suankiTaslar.Union(BitisikTaslariAl(tas2.sutun, tas2.satir));

        if (tas3.bitisikBombasiMi)
            suankiTaslar.Union(BitisikTaslariAl(tas3.sutun, tas3.satir));


        return suankiTaslar;

    }

    private List<GameObject> SatirBombasiMi(M3Tas tas1, M3Tas tas2, M3Tas tas3)
    {
        List<GameObject> suankiTaslar = new List<GameObject>();
        if (tas1.satirBombasiMi)
        {
            suankiTaslar.Union(SatirdakiTaslar(tas1.sutun, tas1.satir));
            //tahta.BombaSatirEtkisi(tas1.satir);
        }


        if (tas2.satirBombasiMi)
        {
            suankiTaslar.Union(SatirdakiTaslar(tas2.sutun, tas2.satir));
            //tahta.BombaSatirEtkisi(tas2.satir);
        }

        if (tas3.satirBombasiMi)
        {
            suankiTaslar.Union(SatirdakiTaslar(tas3.sutun, tas3.satir));
            //tahta.BombaSatirEtkisi(tas3.satir);

        }


        return suankiTaslar;
    }

    private List<GameObject> SutunBombasiMi(M3Tas tas1, M3Tas tas2, M3Tas tas3)
    {
        List<GameObject> suankiTaslar = new List<GameObject>();
        if (tas1.sutunBombasiMi)
        {
            suankiTaslar.Union(SutundakiTaslar(tas1.sutun, tas1.satir));
            //tahta.BombaSutunEtkisi(tas1.sutun);
        }

        if (tas2.sutunBombasiMi)
        {
            suankiTaslar.Union(SutundakiTaslar(tas2.sutun, tas2.satir));
            //tahta.BombaSutunEtkisi(tas2.sutun);
        }

        if (tas3.sutunBombasiMi)
        {
            suankiTaslar.Union(SutundakiTaslar(tas3.sutun, tas3.satir));
            //tahta.BombaSutunEtkisi(tas3.sutun);
        }


        return suankiTaslar;
    }

    private void ListeyeEkleVeEslestir(GameObject tas)
    {
        if (!suankiEslesmeler.Contains(tas))
        {
            suankiEslesmeler.Add(tas);
        }
        tas.GetComponent<M3Tas>().eslestiMi = true;
    }

    private void YakindakiTaslarAl(GameObject tas1, GameObject tas2, GameObject tas3)
    {
        ListeyeEkleVeEslestir(tas1);
        ListeyeEkleVeEslestir(tas2);
        ListeyeEkleVeEslestir(tas3);

    }

    private IEnumerator TumEslestirmeleriBulCo()
    {
        //yield return new WaitForSeconds(.2f);
        yield return null;
        for (int i = 0; i < tahta.en; i++)
        {
            for (int j = 0; j < tahta.boy; j++)
            {
                GameObject suankiTas = tahta.tumTaslar[i, j];
                if (suankiTas != null)
                {
                    M3Tas suankiTasTas = suankiTas.GetComponent<M3Tas>();

                    if (i > 0 && i < tahta.en - 1)
                    {
                        GameObject solTas = tahta.tumTaslar[i - 1, j];
                        GameObject sagTas = tahta.tumTaslar[i + 1, j];
                        if (solTas != null && sagTas != null)
                        {
                            M3Tas solTasTas = solTas.GetComponent<M3Tas>();
                            M3Tas sagTasTas = sagTas.GetComponent<M3Tas>();

                            if (suankiTas.GetComponent<M3Tas>().EslesmeKontrol(solTas.GetComponent<M3Tas>(), sagTas.GetComponent<M3Tas>()))
                            {
                                suankiEslesmeler.Union(SatirBombasiMi(solTasTas, suankiTasTas, sagTasTas));
                                suankiEslesmeler.Union(SutunBombasiMi(solTasTas, suankiTasTas, sagTasTas));
                                suankiEslesmeler.Union(BitisikBombaMi(solTasTas, suankiTasTas, sagTasTas));
                                suankiEslesmeler.Union(RenkBombasiMi(solTasTas, suankiTasTas, sagTasTas));

                                YakindakiTaslarAl(solTas, suankiTas, sagTas);

                            }
                        }

                    }

                    if (j > 0 && j < tahta.boy - 1)
                    {
                        GameObject yukariTas = tahta.tumTaslar[i, j + 1];
                        GameObject asagiTas = tahta.tumTaslar[i, j - 1];
                        if (yukariTas != null && asagiTas != null)
                        {
                            M3Tas yukariTasTas = yukariTas.GetComponent<M3Tas>();
                            M3Tas asagiTasTas = asagiTas.GetComponent<M3Tas>();

                            if (suankiTas.GetComponent<M3Tas>().EslesmeKontrol(yukariTas.GetComponent<M3Tas>(), asagiTas.GetComponent<M3Tas>()))
                            {
                                suankiEslesmeler.Union(SutunBombasiMi(yukariTasTas, suankiTasTas, asagiTasTas));
                                suankiEslesmeler.Union(SatirBombasiMi(suankiTasTas, yukariTasTas, asagiTasTas));
                                suankiEslesmeler.Union(BitisikBombaMi(suankiTasTas, yukariTasTas, asagiTasTas));
                                suankiEslesmeler.Union(RenkBombasiMi(yukariTasTas, suankiTasTas, asagiTasTas));

                                YakindakiTaslarAl(yukariTas, suankiTas, asagiTas);

                            }
                        }

                    }
                }

            }

        }
    }


    List<GameObject> SutundakiTaslar(int sutun , int satir)
    {
        List<GameObject> taslar = new List<GameObject>();
        for (int i = 0; i < tahta.boy; i++)
        {
            if (tahta.tumTaslar[sutun, i] != null)
            {
                M3Tas tas = tahta.tumTaslar[sutun, i].GetComponent<M3Tas>();
                if (tas.satirBombasiMi)
                {
                    gecEslesmeler.Union(SatirdakiTaslar(sutun,i).ToList());
                    //tahta.BombaSatirEtkisi(i);
                }
                if (tas.sutunBombasiMi && i != satir)
                {
                    tas.sutunBombasiMi = false;
                    tas.satirBombasiMi = true;
                    gecEslesmeler.Union(SatirdakiTaslar(sutun, i).ToList());
                }
                if (tas.bitisikBombasiMi)
                    gecEslesmeler.Union(BitisikTaslariAl(tas.sutun, tas.satir));

                if (tas.renkBombasiMi)
                    RenkEsleme(tas.tasinRengi, tas.tasinTuru);

                taslar.Add(tahta.tumTaslar[sutun, i]);
                tas.eslestiMi = true;
            }
        }

        return taslar;
    }


    List<GameObject> SatirdakiTaslar(int sutun, int satir)
    {
        List<GameObject> taslar = new List<GameObject>();
        for (int i = 0; i < tahta.en; i++)
        {
            if (tahta.tumTaslar[i, satir] != null)
            {
                M3Tas tas = tahta.tumTaslar[i, satir].GetComponent<M3Tas>();
                if (tas.sutunBombasiMi)
                {
                    gecEslesmeler.Union(SutundakiTaslar(i,satir).ToList());
                    //tahta.BombaSutunEtkisi(i);
                }
                if(tas.satirBombasiMi && i != sutun)
                {
                    tas.sutunBombasiMi = true;
                    tas.satirBombasiMi = false;
                    gecEslesmeler.Union(SutundakiTaslar(i, satir).ToList());
                }

                if (tas.bitisikBombasiMi)
                    gecEslesmeler.Union(BitisikTaslariAl(tas.sutun, tas.satir));

                if (tas.renkBombasiMi)
                    RenkEsleme(tas.tasinRengi, tas.tasinTuru);

                taslar.Add(tahta.tumTaslar[i, satir]);
                tas.eslestiMi = true;
            }
        }

        return taslar;
    }

    public void RenkEsleme(TasRengi renk, TasTuru tur)
    {
        for (int i = 0; i < tahta.en; i++)
        {
            for (int j = 0; j < tahta.boy; j++)
            {
                if (tahta.tumTaslar[i, j] != null)
                {
                    if (tahta.tumTaslar[i, j].GetComponent<M3Tas>().tasinRengi == renk || ( tahta.tumTaslar[i, j].GetComponent<M3Tas>().tasinTuru == tur && tahta.turEslesmesiOlacakMi))
                    {
                        tahta.tumTaslar[i, j].GetComponent<M3Tas>().eslestiMi = true;
                        //bombaEslesmelerindeBombaKontrol(tahta.tumTaslar[i, j].GetComponent<M3Tas>()); //Bunu test et stackoverflow odlu
                    }
                }
            }
        }
    }

    List<GameObject> RenkTurdekiTaslariAl(TasRengi renk, TasTuru tur)
    {
        List<GameObject> taslar = new List<GameObject>();
        for (int i = 0; i < tahta.en; i++)
        {
            for (int j = 0; j < tahta.boy; j++)
            {
                if (tahta.tumTaslar[i, j] != null)
                {
                    if (tahta.tumTaslar[i, j].GetComponent<M3Tas>().tasinRengi == renk || (tahta.tumTaslar[i, j].GetComponent<M3Tas>().tasinTuru == tur && tahta.turEslesmesiOlacakMi))
                    {
                        taslar.Add(tahta.tumTaslar[i, j]);
                        tahta.tumTaslar[i, j].GetComponent<M3Tas>().eslestiMi = true;
                        //bombaEslesmelerindeBombaKontrol(tahta.tumTaslar[i, j].GetComponent<M3Tas>()); //Bunu test et
                    }
                }
            }
        }
        return taslar;
    }

    List<GameObject> BitisikTaslariAl(int sutun, int satir)
    {
        tahta.tumTaslar[sutun, satir].GetComponent<M3Tas>().bitisikBombasiMi = false;

        List<GameObject> taslar = new List<GameObject>();
        for (int i = sutun - 1; i <= sutun + 1; i++)
        {
            for (int j = satir - 1; j <= satir + 1; j++)
            {
                if (i >= 0 && i < tahta.en && j >= 0 && j < tahta.boy)
                {
                    if (tahta.tumTaslar[i, j] != null)
                    {
                        taslar.Add(tahta.tumTaslar[i, j]);
                        tahta.tumTaslar[i, j].GetComponent<M3Tas>().eslestiMi = true;
                        if(i==sutun && j == satir)
                        {
                            continue;
                        }
                        else
                        {
                            bombaEslesmelerindeBombaKontrol(tahta.tumTaslar[i, j].GetComponent<M3Tas>());   // Burda orn 2 tane bitisik bomba yan yana bi tanesi parladığında diğeri bu bombayı sonsuz kere alıyor gibi
                        }
                        
                    }
                }
            }
        }
        tahta.tumTaslar[sutun, satir].GetComponent<M3Tas>().bitisikBombasiMi = true; // Bunu yok etmede kullanmak icin yeniden ekledim. Fonksiyonun basinda false yapma sebebim bombayi tekrar aramsin diye 16.07
        return taslar;
    }

    //public void BombaKontrol(EslesmeTipi eslenenTip)
    //{
    //    if (tahta.tiklanan1 != null)
    //    {
    //        if (tahta.tiklanan1.eslestiMi && (tahta.tiklanan1.tasinRengi == eslenenTip.tasinRengi || tahta.tiklanan1.tasinTuru == eslenenTip.tasinTuru))
    //        {
    //            tahta.tiklanan1.eslestiMi = false;

    //            int bombaTuru = Random.Range(0, 100);
    //            if (bombaTuru < 50)  // Burayı rasgele degil de switchAngle a gore degistirmis
    //            {
    //                tahta.tiklanan1.SatirBombasiYap();
    //            }
    //            else if (bombaTuru >= 50)
    //            {
    //                tahta.tiklanan1.SutunBombasiYap();
    //            }
    //        }
    //        else if (tahta.tiklanan2 != null)
    //        {
    //            M3Tas digerTas = tahta.tiklanan2.GetComponent<M3Tas>();

    //            if (digerTas.eslestiMi && (tahta.tiklanan2.tasinRengi == eslenenTip.tasinRengi || tahta.tiklanan2.tasinTuru == eslenenTip.tasinTuru))
    //            {
    //                digerTas.eslestiMi = false;

    //                int bombaTuru = Random.Range(0, 100);
    //                if (bombaTuru < 50)
    //                {
    //                    digerTas.SatirBombasiYap();
    //                }
    //                else if (bombaTuru >= 50)
    //                {
    //                    digerTas.SutunBombasiYap();
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        M3Tas dusmeTas = suankiEslesmeler[eslenenTip.eslesenBombaSirasi].GetComponent<M3Tas>();
    //        if (dusmeTas.eslestiMi && dusmeTas.tasinRengi == eslenenTip.tasinRengi && dusmeTas.tasinTuru == eslenenTip.tasinTuru)
    //        {
    //            dusmeTas.eslestiMi = false;
    //            dusmeTas.BitisikBombasiYap();
    //        }
    //    }
    //}


    private void bombaEslesmelerindeBombaKontrol(M3Tas eslenmisObje)
    {
        if (eslenmisObje.bitisikBombasiMi)
            suankiEslesmeler.Union(BitisikTaslariAl(eslenmisObje.sutun, eslenmisObje.satir));

        if (eslenmisObje.satirBombasiMi)
            suankiEslesmeler.Union(SatirdakiTaslar(eslenmisObje.sutun,eslenmisObje.satir));

        if (eslenmisObje.sutunBombasiMi)
            suankiEslesmeler.Union(SutundakiTaslar(eslenmisObje.sutun,eslenmisObje.satir));

        if (eslenmisObje.renkBombasiMi)
            RenkEsleme(eslenmisObje.tasinRengi, eslenmisObje.tasinTuru);  // Bİtisik bomba patladığında alanda Renk bombası var ise
    }

    public void bombaEslesmelerindeTasZatenBomba(M3Tas eslenmisObje)
    {
        if (eslenmisObje.bitisikBombasiMi)
        {
            suankiEslesmeler.Union(BitisikTaslariAl(eslenmisObje.sutun, eslenmisObje.satir));
            if (eslenmisObje.transform.GetChild(0))
            {
                eslenmisObje.transform.GetChild(0).gameObject.SetActive(false);

                GameObject bombaEfekti = Instantiate(tahta.bitisikBombasiEfektiSU, eslenmisObje.transform.position, Quaternion.identity);

                ParticleSystem.TrailModule settings = bombaEfekti.transform.GetChild(0).GetComponent<ParticleSystem>().trails;
                settings.colorOverTrail = new ParticleSystem.MinMaxGradient(eslenmisObje.GetComponent<SpriteRenderer>().color);

                Destroy(bombaEfekti, 2f);
            }

        }

        if (eslenmisObje.satirBombasiMi)
        {
            suankiEslesmeler.Union(SatirdakiTaslar(eslenmisObje.sutun, eslenmisObje.satir));

            if (eslenmisObje.transform.GetChild(0))
            {
                eslenmisObje.transform.GetChild(0).gameObject.SetActive(false);

                GameObject satirEfekti = Instantiate(tahta.satirBombasiEfektiSU, eslenmisObje.transform.position, Quaternion.identity);

                ParticleSystem.TrailModule settings = satirEfekti.transform.GetChild(0).GetComponent<ParticleSystem>().trails;
                settings.colorOverTrail = new ParticleSystem.MinMaxGradient(eslenmisObje.GetComponent<SpriteRenderer>().color);

                ParticleSystem.TrailModule settings2 = satirEfekti.transform.GetChild(1).GetComponent<ParticleSystem>().trails;
                settings2.colorOverTrail = new ParticleSystem.MinMaxGradient(eslenmisObje.GetComponent<SpriteRenderer>().color);

                Destroy(satirEfekti, 2f);
            }

        }

        if (eslenmisObje.sutunBombasiMi)
        {
            suankiEslesmeler.Union(SutundakiTaslar(eslenmisObje.sutun, eslenmisObje.satir));

            if (eslenmisObje.transform.GetChild(0))
            {
                eslenmisObje.transform.GetChild(0).gameObject.SetActive(false);

                GameObject sutunEfekti = Instantiate(tahta.sutunBombasiEfektiSU, eslenmisObje.transform.position, Quaternion.identity);

                ParticleSystem.TrailModule settings = sutunEfekti.transform.GetChild(0).GetComponent<ParticleSystem>().trails;
                settings.colorOverTrail = new ParticleSystem.MinMaxGradient(eslenmisObje.GetComponent<SpriteRenderer>().color);

                ParticleSystem.TrailModule settings2 = sutunEfekti.transform.GetChild(1).GetComponent<ParticleSystem>().trails;
                settings2.colorOverTrail = new ParticleSystem.MinMaxGradient(eslenmisObje.GetComponent<SpriteRenderer>().color);

                Destroy(sutunEfekti, 2f);
            }
        }

        if (eslenmisObje.renkBombasiMi)
        {
            RenkEsleme(eslenmisObje.tasinRengi, eslenmisObje.tasinTuru);
            eslenmisObje.transform.GetChild(0).gameObject.SetActive(false);

        }

        eslenmisObje.eslestiMi = false;
        eslenmisObje.renkBombasiMi = false;
        eslenmisObje.satirBombasiMi = false;
        eslenmisObje.sutunBombasiMi = false;
        eslenmisObje.bitisikBombasiMi = false;
    }
}
