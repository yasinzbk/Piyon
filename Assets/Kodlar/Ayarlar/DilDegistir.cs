using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;




public class DilDegistir : MonoBehaviour
{
    public List<TextMeshProUGUI> SahnelerdekiTextMeshler; //[HideInInspector]

    public List<SahnelerdekiCeviriler> Ceviriler;

    public OyunVerisi oyunVerisi;

    private void Start()
    {
        oyunVerisi = FindObjectOfType<OyunVerisi>();

    }


   

    public void Cevir(int DilNo)
    {
        SahnelerdekiTextMeshler.Clear(); //bunu koydum ama bilmiyorum
        foreach (TextMeshProUGUI item in Resources.FindObjectsOfTypeAll(typeof(TextMeshProUGUI)))   // Sahnedeki Cevirilecek textleri bulur.
        {
            SahnelerdekiTextMeshler.Add(item);
        }
        if (oyunVerisi != null) {
            switch ((Dil)DilNo)
            {
                case Dil.Turkce:
                    for (int i = 0; i < Ceviriler.Count; i++)
                    {
                        for (int j = 0; j < SahnelerdekiTextMeshler.Count; j++)
                        {

                            if (Ceviriler[i].Ingilizcesi == SahnelerdekiTextMeshler[j].text)
                            {
                                SahnelerdekiTextMeshler[j].text = Ceviriler[i].Turkcesi;

                            }

                        }
                    }
                    if (oyunVerisi.veriKaydet.oyununDili != Dil.Turkce)
                    {
                        oyunVerisi.veriKaydet.oyununDili = Dil.Turkce;

                    }
                        Debug.Log("TURKCEEEEEEEEEEEEEEEEEEEEEEEE");

                    break;
                case Dil.Ingilizce:
                    for (int i = 0; i < Ceviriler.Count; i++)
                    {
                        for (int j = 0; j < SahnelerdekiTextMeshler.Count; j++)
                        {
                            if (Ceviriler[i].Turkcesi == SahnelerdekiTextMeshler[j].text)
                            {
                                SahnelerdekiTextMeshler[j].text = Ceviriler[i].Ingilizcesi;

                            }

                            //Debug.Log("|" + SahnelerdekiTextMeshler[j].text + "|");
                        }

                    }
                    if (oyunVerisi.veriKaydet.oyununDili != Dil.Ingilizce)
                    {
                        oyunVerisi.veriKaydet.oyununDili = Dil.Ingilizce;
                    }
                        Debug.Log("IngILIZCEEEEEEEEEEEEEEEE");

                    break;
                default:
                    break;

            }
        }

        //for (int i = 0; i < Ceviriler.Count; i++)
        //{
        //    for (int j = 0; j < SahnelerdekiTextMeshler.Count; j++)
        //    {
        //        if (CevirilecekDil == Dil.Turkce)
        //        {
        //            if (Ceviriler[i].Ingilizcesi == SahnelerdekiTextMeshler[j].text)
        //            {
        //                SahnelerdekiTextMeshler[j].text = Ceviriler[i].Turkcesi;

        //                OyunDili = Dil.Turkce;
        //            }
        //        }else if (CevirilecekDil == Dil.Ingilizce)
        //        {
        //            if (Ceviriler[i].Turkcesi == SahnelerdekiTextMeshler[j].text)
        //            {
        //                SahnelerdekiTextMeshler[j].text = Ceviriler[i].Ingilizcesi;
        //                OyunDili = Dil.Ingilizce;
        //            }
        //        }
        //    }
        //}
    }


    [System.Serializable]
    public class SahnelerdekiCeviriler
    {
        [Multiline(5)]
        public string Turkcesi;
        [Multiline(5)]
        public string Ingilizcesi;

        public SahnelerdekiCeviriler(string turkcesi,string ingilizcesi)
        {
            Turkcesi = turkcesi;
            Ingilizcesi = ingilizcesi;
        }
    }
    
}
