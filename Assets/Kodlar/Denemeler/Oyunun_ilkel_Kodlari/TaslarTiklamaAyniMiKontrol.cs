using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaslarTiklamaAyniMiKontrol : MonoBehaviour
{
    [HideInInspector]public GameObject parcaOlusturucu;
    public int numara;
    public List<GameObject> ButunTaslar;
    public List<GameObject> TemasliObjelerX,TemasliObjelerY;
    //public List<GameObject> AyniSiradakiAlcakTaslar, AyniSiradakiYuksekTaslar;

    public bool tasDussunMu = false;

    private float tasTutY;

    //private float enAlcakTas = 1000;

    bool altindaTasVarMi = false;
    public int sira;

    //public float asagiDusmeBirimi;

    private void Awake()
    {
        parcaOlusturucu = GameObject.Find("ParcaOlusturucu");
        //asagiDusmeBirimi = transform.position.y;
    }

    private void Start()
    {
        tasTutY = transform.position.y - 1f;   
    }
    private void Update()
    {
        //AsagiyaDus(asagiDusmeBirimi);
        BirKareAsagiyaDus();
    }
    

    public void TaslarAyniMiKontrol()
    {
        foreach (GameObject tumObjeler in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (tumObjeler.GetComponent<TaslarTiklamaAyniMiKontrol>() != null)
            {
                if (this.sira != tumObjeler.GetComponent<TaslarTiklamaAyniMiKontrol>().sira)
                {
                   
                    ButunTaslar.Add(tumObjeler);

                }

            }
            
        }

        for(int i =0; i < ButunTaslar.Count; i++)
        {
            float x = Mathf.Abs(this.transform.position.x - ButunTaslar[i].transform.position.x);
            float y = Mathf.Abs(this.transform.position.y - ButunTaslar[i].transform.position.y);

            if (x > 0 && x < 2 && this.numara == ButunTaslar[i].GetComponent<TaslarTiklamaAyniMiKontrol>().numara)
            {
                if (y == 0)
                {
                    TemasliObjelerX.Add(ButunTaslar[i]);
                   
                }
            }
            if (y > 0 && y < 2 && this.numara == ButunTaslar[i].GetComponent<TaslarTiklamaAyniMiKontrol>().numara)
            {
                if (x == 0)
                {
                    TemasliObjelerY.Add(ButunTaslar[i]);
                    
                }
            }

        }

        
        //for (int k = 0; k < ButunTaslar.Count; k++)
        //{
        //    if(ButunTaslar[k].transform.position.y < enAlcakTas)
        //    {
        //        enAlcakTas = ButunTaslar[k].transform.position.y;
        //    }
            

        //}

        //Debug.Log("En alcak tas Y konumu: " + enAlcakTas);

        TemaslilariSil();


    }

    private void TemaslilariSil()
    {
        Debug.Log("TemaslilarSiliniyor");

        if (TemasliObjelerX.Count == 2 && TemasliObjelerY.Count < 2)
        {
            foreach (GameObject item in TemasliObjelerX)
            {
                Destroy(item);
            }
            parcaOlusturucu.GetComponent<TasHareket>().taslarGeriGitsinMi = false;
            PatlamaDusmeKontrolX();
            Destroy(this.gameObject);


        }
        else if(TemasliObjelerY.Count == 2 && TemasliObjelerX.Count < 2)
        {
            foreach (GameObject item in TemasliObjelerY)
            {
                Destroy(item);
            }
            parcaOlusturucu.GetComponent<TasHareket>().taslarGeriGitsinMi = false;

            //float EnBuyukY = 0f;
            //float EnKucukY = 1000f;

            //for (int i = 0; i < ButunTaslar.Count; i++)
            //{
            //    if(transform.position.x == ButunTaslar[i].transform.position.x && transform.position.y < (ButunTaslar[i].transform.position.y -1))
            //    {
            //        AyniSiradakiYuksekTaslar.Add(ButunTaslar[i]);

            //        for (int k = 0; k < ButunTaslar.Count; k++)
            //        {

            //            if(ButunTaslar[k].transform.position.y < ButunTaslar[i].transform.position.y  && ButunTaslar[k].transform.position.x == ButunTaslar[i].transform.position.x)
            //            {
            //                AyniSiradakiAlcakTaslar.Add(ButunTaslar[k]);
            //            }

            //        }

                    

            //        for (int j = 0; j < AyniSiradakiAlcakTaslar.Count; j++)
            //        {
            //            if(EnBuyukY > AyniSiradakiAlcakTaslar[j].transform.position.y)
            //            EnBuyukY = AyniSiradakiAlcakTaslar[j].transform.position.y;
            //        }


            //    }
            //}

            //for (int i = 0; i < AyniSiradakiYuksekTaslar.Count; i++)
            //{
            //    if (AyniSiradakiYuksekTaslar[i].transform.position.y < EnKucukY)
            //    {
            //        EnKucukY = AyniSiradakiYuksekTaslar[i].transform.position.y;
            //    }

            //}

            //for (int j = 0; j < AyniSiradakiYuksekTaslar.Count; j++)
            //{
            //    AyniSiradakiYuksekTaslar[j].GetComponent<TaslarTiklamaAyniMiKontrol>().asagiDusmeBirimi = AyniSiradakiYuksekTaslar[j].transform.position.y - (EnKucukY - EnBuyukY);
            //}

            Destroy(this.gameObject);

            
        }
        else
        {
            //parcaOlusturucu.GetComponent<ParcaOlusturma>().taslarGeriGitsinMi = true;
            TemasliObjelerX.Clear();
            TemasliObjelerY.Clear();
            ButunTaslar.Clear();
        }
    }

    //public void AsagiyaDus(float kacBirim)
    //{
        
    //    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, kacBirim), 2f * Time.deltaTime);

    //    if(transform.position.y == kacBirim)
    //    {
    //        asagiDusmeBirimi = transform.position.y;
    //    }
    //}

    public void AsagiyaDusmeKontrol()
    {
        if (transform.position.y != 0f)
        {           

            for (int k = 0; k < ButunTaslar.Count; k++)
            {

                if ((transform.position.y - 0.5f) > ButunTaslar[k].transform.position.y && (transform.position.y - 1.5f) < ButunTaslar[k].transform.position.y && transform.position.x == ButunTaslar[k].transform.position.x)
                {
                    altindaTasVarMi = true;
                }

            }

            if (!altindaTasVarMi)
            {
                tasDussunMu = true;
                
            }

            altindaTasVarMi = false;
        }

    }

    private void BirKareAsagiyaDus()
    {
        if (tasDussunMu)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, tasTutY), 5f * Time.deltaTime);

            if (transform.position.y == tasTutY)
            {
                tasDussunMu = false;
                //AsagiyaDusmeKontrol();
                tasTutY = transform.position.y - 1.0f;
            }
        }
    }


    public void PatlamaDusmeKontrolX()
    {
        for (int i = 0; i < ButunTaslar.Count; i++)
        {
            ButunTaslar[i].GetComponent<TaslarTiklamaAyniMiKontrol>().AsagiyaDusmeKontrol();

        }
    }

    private void OnMouseDown()
    {
        if (!parcaOlusturucu.GetComponent<TasHareket>().taslarHareketEdiyorMu)
        {
           if (parcaOlusturucu.GetComponent<TasHareket>().tiklanan1 == null)
           {
            parcaOlusturucu.GetComponent<TasHareket>().tiklanan1 = this.gameObject;

            parcaOlusturucu.GetComponent<TasHareket>().SeciliSimgesiOlustur();
           }
           else
           {
            parcaOlusturucu.GetComponent<TasHareket>().tiklanan2 = this.gameObject;
            parcaOlusturucu.GetComponent<TasHareket>().TasHareketKontrol();
            
           } 

        }
    }



}
