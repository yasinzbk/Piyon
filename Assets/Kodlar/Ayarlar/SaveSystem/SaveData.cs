using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData  //  Things that will save
{   
    public long lastUpdated;  // son kayit edilen profil icin gerekli, ona gore continue tusu calisiyor.
    //public Vector3 playerPosition;
    //public SerializableDictionary<string, bool> itemsCollected; // dictinory ornek
  


    public bool[,] aktifMi;  // bunu kaydetmiyor serialize etmiyor

    public Dil oyununDili;

    public int suankiSeviye;

    public SaveData()
    {
        //playerPosition = Vector3.zero;
        //itemsCollected = new SerializableDictionary<string, bool>();


        aktifMi = new bool[2, 100];
        oyununDili = Dil.Ingilizce;
        suankiSeviye = 1;

        for (int i = 0; i < aktifMi.GetLength(0); i++)
        {
          aktifMi[i, 0] = true;
        }
    }


}

