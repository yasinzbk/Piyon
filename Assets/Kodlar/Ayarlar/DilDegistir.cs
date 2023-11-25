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

    public DilKontrol dilKontrol;

    Dil dil;
    //public OyunVerisi oyunVerisi;

    //private void Start()
    //{
    //    oyunVerisi = FindObjectOfType<OyunVerisi>();

    //}

    private void Start()
    {
        dilKontrol = FindObjectOfType<DilKontrol>();
    }


    public void Cevir(int DilNo)
    {
        if (dilKontrol == null)
        {
            dilKontrol = FindObjectOfType<DilKontrol>();
        }

        SahnelerdekiTextMeshler.Clear(); //bunu koydum ama bilmiyorum
        foreach (TextMeshProUGUI item in Resources.FindObjectsOfTypeAll(typeof(TextMeshProUGUI)))   // Sahnedeki Cevirilecek textleri bulur.
        {
            SahnelerdekiTextMeshler.Add(item);
        }
        //if (oyunVerisi != null) {
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
                    //if (dil != Dil.Turkce)
                    //{
                    //    dil = Dil.Turkce;

                    //}
                        Debug.Log("TURKCEEEEEEEEEEEEEEEEEEEEEEEE");

                    dilKontrol.DilAyarla(Dil.Turkce);


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
                    //if (dil != Dil.Ingilizce)
                    //{
                    //    dil = Dil.Ingilizce;
                    //}
                    Debug.Log("IngILIZCEEEEEEEEEEEEEEEE");

                    dilKontrol.DilAyarla(Dil.Ingilizce);


                    break;
                default:
                    break;

            }
    }

    //public void LoadData(SaveData data)
    //{
    //    dil = data.oyununDili;
    //}

    //public void SaveData(SaveData data)
    //{
    //    data.oyununDili = dil;
    //}

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
