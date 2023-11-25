using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OyunBitti : MonoBehaviour  // gereksiz bi class oyunu guncellenince duzeltilmeli
{

    public int sonBolum;

    public GameObject oyunSonuPaneli;


    
    private void Start()
    {

        //PlayerPrefs.SetInt("SonDunyaSayisi", sonAlemNo);

    }

    void Update()
    {

        //if (SaveLoadManager.instance.gameData.aktifMi[1, 88])
        //{
        //    oyunSonuPaneli.SetActive(true);
        //}
    }


    public bool OyunBittiKontrol(int seviye)
    {

        if (seviye>=sonBolum)
        {
            oyunSonuPaneli.SetActive(true);
            return true;
        }

        return false;
    }
}
