using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraOlcekleyiciSM3 : MonoBehaviour
{
    private Tahta tahta;
    public float kameraUzantisi;
    public float enBoyOrani = 0.625f; // 9:16 oldugu icin 
    public float kenarBosluklari = 2;
    public float yFarki = 1;

    void Start()
    {
        tahta = FindObjectOfType<Tahta>();
        if (tahta != null)
        {
            KamerayiAyarla(tahta.en - 1, tahta.boy - 1);
        }
    }

    void KamerayiAyarla(float x, float y)
    {
        Vector3 gecici = new Vector3(x / 2, y / 2 + yFarki, kameraUzantisi);
        transform.position = gecici;
        if (tahta.en >= tahta.boy)
        {
            Camera.main.orthographicSize = (tahta.en / 2 + kenarBosluklari) / enBoyOrani;
        }
        else
        {
            Camera.main.orthographicSize = tahta.boy / 2 + kenarBosluklari;
        }
    }

}
