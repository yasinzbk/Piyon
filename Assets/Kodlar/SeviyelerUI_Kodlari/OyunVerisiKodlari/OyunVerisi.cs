using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public enum Dil { Turkce, Ingilizce }

[Serializable]
public class VeriKaydet
{
    public bool[,] aktifMi;

    public Dil oyununDili = Dil.Ingilizce;
}



public class OyunVerisi : MonoBehaviour
{
    public static OyunVerisi oyunVerisi;
    public VeriKaydet veriKaydet;


    private void Awake()
    {
        if(oyunVerisi == null)
        {
            DontDestroyOnLoad(this.gameObject);
            oyunVerisi = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        Yukle();
    }

    private void Start()
    {
        //for (int i = 0; i < veriKaydet.aktifMi.GetLength(0); i++)
        //{
        //    for (int j = 0; j < veriKaydet.aktifMi.GetLength(1); j++)
        //    {
        //        veriKaydet.aktifMi[i, j] = true;
        //        veriKaydet.aktifMi[0, 9] = false;
        //    }
        //}
    }

    public void Kaydet()
    {
        BinaryFormatter bicimlendirici = new BinaryFormatter();
        FileStream dosya = File.Open(Application.persistentDataPath + "/oyuncu6.dat",FileMode.Create);
        _ = new VeriKaydet();
        VeriKaydet veri = veriKaydet;
        bicimlendirici.Serialize(dosya, veri);
        dosya.Close();
        Debug.Log("kaydedildi");
    }

    public void Yukle()
    {
        if(File.Exists(Application.persistentDataPath + "/oyuncu6.dat"))
        {
            BinaryFormatter bicimlendirici = new BinaryFormatter();
            FileStream dosya = File.Open(Application.persistentDataPath + "/oyuncu6.dat", FileMode.Open);
            veriKaydet = bicimlendirici.Deserialize(dosya) as VeriKaydet;
            dosya.Close();
            Debug.Log("Yuklendi");
        }
        else
        {
            veriKaydet = new VeriKaydet();
            //veriKaydet.aktifMi = new bool[100,10];
            veriKaydet.aktifMi = new bool[2,100];

            for (int i = 0; i < veriKaydet.aktifMi.GetLength(0); i++)
            {
                veriKaydet.aktifMi[i, 0] = true;
            }

            veriKaydet.oyununDili = Dil.Ingilizce;
            //veriKaydet.aktifMi[0, 0] = false;
        }

    }

    private void OnDisable()
    {
        Kaydet();
    }

    private void OnApplicationPause()
    {
        Kaydet();
    }
}
