using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DilKontrol : MonoBehaviour
{

    private OyunVerisi oyunVerisi;
    public DilDegistir dilDegistir;

    
    public Dil sahneDili = Dil.Turkce;

    private void Start()
    {
        oyunVerisi = FindObjectOfType<OyunVerisi>();
        dilDegistir = FindObjectOfType<DilDegistir>();


        Invoke("DilDegisim", 0.1f);
        
    }



    void DilDegisim()
    {
        if (dilDegistir != null && oyunVerisi != null)
        {
            if (sahneDili != oyunVerisi.veriKaydet.oyununDili)
            {
                Debug.Log("dilDegisim");
                dilDegistir.Cevir((int)oyunVerisi.veriKaydet.oyununDili);
            }

        }
    }


}
