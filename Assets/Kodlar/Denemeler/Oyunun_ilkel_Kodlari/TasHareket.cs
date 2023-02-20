using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasHareket : MonoBehaviour
{
    public GameObject seciliSimgesi;

    public bool taslarGeriGitsinMi = true;

    private bool taslarHareketEtsinMi = false;

    private bool taslarHareketEttiMi = false;

    public bool taslarHareketEdiyorMu = false;

    Vector2 tiklanan1PozTut;
    Vector2 tiklanan2PozTut;

    /*[HideInInspector]*/
    public GameObject tiklanan1, tiklanan2;

    //private List<GameObject> TumKutucukKoordinatlari;
    //private List<GameObject> BosKutucukKoordinatlari;
    //public List<GameObject> ButunTaslar;
    //private bool kutucukDoluMu = false;


    private void Update()
    {
        TaslarinYerDegistirmesi();
        //BosKutucuklariDoldur();
    }

    public void TasHareketKontrol() //Tum Satranc taslari icin bu fonksiyonun degismesi gerekiyor gerisi ayni.
    {
        Debug.Log("farklı objeye tıklandı");
        float gelenX = Mathf.Abs(tiklanan1.transform.position.x - tiklanan2.transform.position.x);
        float gelenY = Mathf.Abs(tiklanan1.transform.position.y - tiklanan2.transform.position.y);

        if ((gelenX <= 1 && gelenY == 0) || (gelenX == 0 && gelenY <= 1))
        {
            tiklanan1PozTut = tiklanan1.transform.position;
            tiklanan2PozTut = tiklanan2.transform.position;
            taslarHareketEtsinMi = true;
            taslarHareketEdiyorMu = true;

        }
        else
        {
            taslarHareketEtsinMi = false;
            tiklanan1 = null;
            tiklanan2 = null;
            Debug.Log("cok uzak mesafe");
        }

        seciliSimgesi.SetActive(false);


    }


    private void TaslarinYerDegistirmesi()
    {
        if (taslarHareketEtsinMi)
        {
            tiklanan1.transform.position = Vector2.MoveTowards(tiklanan1.transform.position, tiklanan2PozTut, 2f * Time.deltaTime);
            tiklanan2.transform.position = Vector2.MoveTowards(tiklanan2.transform.position, tiklanan1PozTut, 2f * Time.deltaTime);

            if (new Vector2(tiklanan1.transform.position.x, tiklanan1.transform.position.y) == tiklanan2PozTut)
            {
                taslarGeriGitsinMi = true;

                foreach (GameObject tumObjeler in FindObjectsOfType(typeof(GameObject)) as GameObject[])
                {
                    if (tumObjeler.GetComponent<TaslarTiklamaAyniMiKontrol>() != null)
                    {
                        tumObjeler.GetComponent<TaslarTiklamaAyniMiKontrol>().TaslarAyniMiKontrol();

                    }
                }

                taslarHareketEttiMi = true;
                

                if (!taslarGeriGitsinMi)
                {
                    tiklanan1 = null;
                    tiklanan2 = null;
                    taslarHareketEdiyorMu = false;
                    Debug.Log("Taslar Yok oldu");
                    //BosKutucuklariDoldur();
                }
                Debug.Log("g");
                taslarHareketEtsinMi = false;

            }
        }

        if (taslarGeriGitsinMi && taslarHareketEttiMi)
        {
            tiklanan1.transform.position = Vector2.MoveTowards(tiklanan1.transform.position, tiklanan1PozTut, 2f * Time.deltaTime);
            tiklanan2.transform.position = Vector2.MoveTowards(tiklanan2.transform.position, tiklanan2PozTut, 2f * Time.deltaTime);

            if (new Vector2(tiklanan1.transform.position.x, tiklanan1.transform.position.y) == tiklanan1PozTut)
            {
                tiklanan1 = null;
                tiklanan2 = null;

                taslarHareketEttiMi = false;
                taslarGeriGitsinMi = true;
                taslarHareketEdiyorMu = false;
            }

        }


    }

    public void SeciliSimgesiOlustur()
    {
        seciliSimgesi.SetActive(true);

        seciliSimgesi.transform.position = tiklanan1.transform.position;
    }

    //public void BosKutucuklariDoldur()
    //{

    //        //BosKutucuklariBul();

    //        foreach (GameObject tumObjeler in FindObjectsOfType(typeof(GameObject)) as GameObject[])
    //        {
    //            if (tumObjeler.GetComponent<TaslarTiklamaAyniMiKontrol>() != null)
    //            {

    //                    ButunTaslar.Add(tumObjeler);

                    
    //            }

    //        }

            
    //        //for (int i = 0; i < TumKutucukKoordinatlari.Count; i++)
    //        //{
    //        //    kutucukDoluMu = false;

    //        //    for (int j = 0; j < ButunTaslar.Count; j++)
    //        //    {
    //        //        if (TumKutucukKoordinatlari[i] == ButunTaslar[j].transform)
    //        //        {
    //        //            kutucukDoluMu = true;
    //        //        }

    //        //    }

    //        //    if (!kutucukDoluMu)
    //        //    {
    //        //        BosKutucukKoordinatlari.Add(TumKutucukKoordinatlari[i]);
    //        //    }
    //        //}


    //        //for (int i = 0; i < BosKutucukKoordinatlari.Count; i++)
    //        //{
    //        //    Debug.Log("  " + BosKutucukKoordinatlari.Count);
    //        //}

        

    //}

    //private void BosKutucuklariBul()
    //{
    //    foreach (GameObject tumObjeler in FindObjectsOfType(typeof(GameObject)) as GameObject[])
    //    {
    //        if (tumObjeler.name == "kareKutucuk(Clone)")
    //        {

    //            TumKutucukKoordinatlari.Add(tumObjeler);

    //        }

    //    }

    //}

}
