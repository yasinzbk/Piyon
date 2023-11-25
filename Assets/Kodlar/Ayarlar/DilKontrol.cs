using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DilKontrol : MonoBehaviour,ISaveData
{

    //private OyunVerisi oyunVerisi;
    public DilDegistir dilDegistir;

    
    public Dil sahneDili { get; private set; }

    private void Start()
    {
        //oyunVerisi = FindObjectOfType<OyunVerisi>();
        dilDegistir = FindObjectOfType<DilDegistir>();

        

        Invoke("DilDegisim", 0.1f);
        
    }



    void DilDegisim()
    {
        if (dilDegistir != null)
        {
            //if (sahneDili != oyunVerisi.veriKaydet.oyununDili)
            //{
                Debug.Log("dilDegisim");
                //dilDegistir.Cevir((int)oyunVerisi.veriKaydet.oyununDili);
                dilDegistir.Cevir((int)sahneDili);
            //}

        }
    }

    public void DilAyarla(Dil dil)
    {
        sahneDili = dil;
    }

    public void LoadData(SaveData data)
    {
        sahneDili = data.oyununDili;
    }

    public void SaveData(SaveData data)
    {
        data.oyununDili = sahneDili;
    }
}
