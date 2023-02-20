using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EslesmeBulSM3 : MonoBehaviour
{
    private Tahta tahta;
    public List<GameObject> suankiEslesmeler = new List<GameObject>();
   
    void Start()
    {
        tahta = FindObjectOfType<Tahta>();
    }


    public void TumEslestirmeleriBul()
    {
        StartCoroutine(TumEslestirmeleriBulCo());
    }

    private List<GameObject> BitisikBombaMi(Tas tas1,Tas tas2,Tas tas3)
    {
        List<GameObject> suankiTaslar = new List<GameObject>();
        if (tas1.bitisikBombasiMi)
            suankiTaslar.Union(BitisikTaslariAl(tas1.sutun,tas1.satir));

        if (tas2.bitisikBombasiMi)
            suankiTaslar.Union(BitisikTaslariAl(tas2.sutun,tas2.satir));

        if (tas3.bitisikBombasiMi)
            suankiTaslar.Union(BitisikTaslariAl(tas3.sutun,tas3.satir));


        return suankiTaslar;

    }

    private List<GameObject> SatirBombasiMi(Tas tas1, Tas tas2, Tas tas3)
    {
        List<GameObject> suankiTaslar = new List<GameObject>();
        if (tas1.satirBombasiMi)
            suankiTaslar.Union(SatirdakiTaslar(tas1.satir));

        if (tas2.satirBombasiMi)
            suankiTaslar.Union(SatirdakiTaslar(tas2.satir));

        if (tas3.satirBombasiMi)
            suankiTaslar.Union(SatirdakiTaslar(tas3.satir));


        return suankiTaslar;
    }

    private List<GameObject> SutunBombasiMi(Tas tas1, Tas tas2, Tas tas3)
    {
        List<GameObject> suankiTaslar = new List<GameObject>();
        if (tas1.sutunBombasiMi)
            suankiTaslar.Union(SutundakiTaslar(tas1.sutun));

        if (tas2.sutunBombasiMi)
            suankiTaslar.Union(SutundakiTaslar(tas2.sutun));

        if (tas3.sutunBombasiMi)
            suankiTaslar.Union(SutundakiTaslar(tas3.sutun));


        return suankiTaslar;
    }

    private void ListeyeEkleVeEslestir(GameObject tas)
    {
        if (!suankiEslesmeler.Contains(tas))
        {
            suankiEslesmeler.Add(tas);
        }
        tas.GetComponent<Tas>().eslestiMi = true;
    }

    private void YakindakiTaslarAl(GameObject tas1, GameObject tas2, GameObject tas3)
    {
        ListeyeEkleVeEslestir(tas1);
        ListeyeEkleVeEslestir(tas2);
        ListeyeEkleVeEslestir(tas3);

    }

    private IEnumerator TumEslestirmeleriBulCo()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < tahta.en; i++)
        {
            for (int j = 0; j < tahta.boy; j++)
            {
                GameObject suankiTas = tahta.tumTaslar[i, j];


                if(suankiTas != null)
                {
                    Tas suankiTasTas = suankiTas.GetComponent<Tas>();

                    if (i>0 && i<tahta.en-1)
                    {
                        GameObject solTas = tahta.tumTaslar[i - 1, j];                       
                        GameObject sagTas = tahta.tumTaslar[i + 1, j];

                        if (solTas != null && sagTas != null)
                        {
                            Tas solTasTas = solTas.GetComponent<Tas>();
                            Tas sagTasTas = sagTas.GetComponent<Tas>();

                            if (solTas.tag == suankiTas.tag && sagTas.tag == suankiTas.tag)
                            {

                                suankiEslesmeler.Union(SatirBombasiMi(solTasTas, suankiTasTas, sagTasTas));
                                suankiEslesmeler.Union(SutunBombasiMi(solTasTas, suankiTasTas, sagTasTas));
                                suankiEslesmeler.Union(BitisikBombaMi(solTasTas, suankiTasTas, sagTasTas));

                                YakindakiTaslarAl(solTas, suankiTas, sagTas);

                            }
                        }

                    }


                    if (j > 0 && j < tahta.boy - 1)
                    {
                        GameObject yukariTas = tahta.tumTaslar[i , j + 1];
                        GameObject asagiTas = tahta.tumTaslar[i , j - 1];
                        if (yukariTas != null && asagiTas != null)
                        {
                            Tas yukariTasTas = yukariTas.GetComponent<Tas>();
                            Tas asagiTasTas = asagiTas.GetComponent<Tas>();
                            if (yukariTas.tag == suankiTas.tag && asagiTas.tag == suankiTas.tag)
                            {
                                suankiEslesmeler.Union(SutunBombasiMi(yukariTasTas, suankiTasTas, asagiTasTas));

                                suankiEslesmeler.Union(SatirBombasiMi(suankiTasTas, yukariTasTas, asagiTasTas));

                                suankiEslesmeler.Union(BitisikBombaMi(suankiTasTas, yukariTasTas, asagiTasTas));


                                YakindakiTaslarAl(yukariTas, suankiTas, asagiTas);

                            }
                        }

                    }
                }

            }

        }
    }

    public void RenkEsleme(string color)
    {
        for (int i = 0; i < tahta.en; i++)
        {
            for (int j = 0; j < tahta.boy; j++)
            {
                if(tahta.tumTaslar[i,j] != null)
                {
                    if (tahta.tumTaslar[i, j].tag == color)
                    {
                        tahta.tumTaslar[i, j].GetComponent<Tas>().eslestiMi = true;
                    }
                }
            }
        }
    }

    List<GameObject> BitisikTaslariAl(int sutun, int satir)
    {
        List<GameObject> taslar = new List<GameObject>();
        for (int i = sutun - 1; i <= sutun + 1; i++)
        {
            for (int j = satir - 1; j <= satir + 1; j++)
            {
                if(i >= 0 && i < tahta.en && j >= 0 && j < tahta.boy)
                {
                    if (tahta.tumTaslar[i, j] != null)
                    {
                        taslar.Add(tahta.tumTaslar[i, j]);
                        tahta.tumTaslar[i, j].GetComponent<Tas>().eslestiMi = true;
                    }
                }
            }
        }
        return taslar;
    }

    List<GameObject> SutundakiTaslar(int sutun)
    {
        List<GameObject> taslar = new List<GameObject>();
        for (int i = 0; i < tahta.boy; i++)
        {
            if (tahta.tumTaslar[sutun, i] != null)
            {
                Tas tas = tahta.tumTaslar[sutun, i].GetComponent<Tas>();
                if (tas.satirBombasiMi)
                {
                    taslar.Union(SatirdakiTaslar(i).ToList());
                }

                taslar.Add(tahta.tumTaslar[sutun, i]);
                tas.eslestiMi = true;
            }
        }

        return taslar;
    }


    List<GameObject> SatirdakiTaslar(int satir)
    {
        List<GameObject> taslar = new List<GameObject>();
        for (int i = 0; i < tahta.en; i++)
        {
            if (tahta.tumTaslar[i, satir] != null)
            {
                Tas tas = tahta.tumTaslar[i , satir].GetComponent<Tas>();
                if (tas.sutunBombasiMi)
                {
                    taslar.Union(SutundakiTaslar(i).ToList());
                }

                taslar.Add(tahta.tumTaslar[i, satir]);
                tas.eslestiMi = true;
            }
        }

        return taslar;
    }


    public void BombaKontrol()
    {
        if (tahta.suankiTas != null)
        {
            if (tahta.suankiTas.eslestiMi)
            {
                tahta.suankiTas.eslestiMi = false;

                int bombaTuru = Random.Range(0, 100);
                if (bombaTuru < 50)  // Burayı rasgele degil de switchAngle a gore degistirmis
                {
                    tahta.suankiTas.SatirBombasiYap();
                }
                else if(bombaTuru >= 50)
                {
                    tahta.suankiTas.SutunBombasiYap(); 
                }
            }
            else if (tahta.suankiTas.digerTas != null)
            {
                Tas digerTas = tahta.suankiTas.digerTas.GetComponent<Tas>();

                if (digerTas.eslestiMi)
                {
                    digerTas.eslestiMi = false;

                    int bombaTuru = Random.Range(0, 100);
                    if (bombaTuru < 50)
                    {
                        digerTas.SatirBombasiYap();
                    }
                    else if (bombaTuru >= 50)
                    {
                        digerTas.SutunBombasiYap();
                    }
                }
            }
        }
    }

}
