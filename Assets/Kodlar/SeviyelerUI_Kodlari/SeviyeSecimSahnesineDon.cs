using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeviyeSecimSahnesineDon : MonoBehaviour // bu class gereksiz silebilirim
{
    private M3Tahta m3Tahta;
    private OnayPaneli onayPaneli;

    public void Tamam(string yuklenecekSeviye)
    {
        //if (yuklenecekSeviye != SceneManager.GetActiveScene().name)  // Oyunu Kazandiği anlamına geliyor. Ayni sahneyi Tekrar butonu yukluyor
        //{
        //    VeriGuncelle();
        //}  // buna gerek kalmadi

        SceneManager.LoadScene(yuklenecekSeviye);
    }

    public void VeriGuncelle()
    {
        if (SaveLoadManager.instance != null)
        {
            //if (m3Tahta.seviye + 2 >= oyunVerisi.veriKaydet.aktifMi.GetLength(1))  // array'in 2 katmanından 2.sinin sayısına bakıyor, bu da o alemdeki bolum sayisini veriyor 
            //{
            //    if (m3Tahta.alemNo + 2 >= PlayerPrefs.GetInt("SonDunyaSayisi")) // m3tahtaalemno + 1 mi 2 mi bak
            //    {
            //        Debug.Log("Oyunu Bitirdin Aloooooooooooooooooooo");
            //        oyunVerisi.veriKaydet.aktifMi[0, oyunVerisi.veriKaydet.aktifMi.GetLength(1) - 1] = true;
            //    }
            //    else
            //    {
            //        oyunVerisi.veriKaydet.aktifMi[0, m3Tahta.alemNo] = true;
            //        Debug.Log("Dünyayı Bitirdin"); // buraya dunya bitiimi paneli açtırabilirim.
            //    }
            //}
            //else
            //{
            //    oyunVerisi.veriKaydet.aktifMi[m3Tahta.alemNo, m3Tahta.seviye + 1] = true;
            //}


            if(m3Tahta.seviye + 2 >= m3Tahta.alem.seviyeler.Length)
            {
              //  SaveLoadManager.instance.gameData.aktifMi[1, 88] = true;   // Kullanılmayan 2. alemden rasgele bir bolum olan 88. bolumu aktif ettim (0, 1. alem oluyor) 
                onayPaneli.yuklencekSeviye = "AnaEkran";
            }
            else
            {
                //SaveLoadManager.instance.gameData.aktifMi[m3Tahta.alemNo, m3Tahta.seviye + 1] = true;
                SaveLoadManager.instance.gameData.suankiSeviye = m3Tahta.seviye + 2;
            }
        }
    }

    private void Start()
    {
        m3Tahta = FindObjectOfType<M3Tahta>();
        onayPaneli = m3Tahta.GetComponent<OnayPaneli>();
    }
}
