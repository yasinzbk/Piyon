using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraOlcekleyici : MonoBehaviour
{
    private M3Tahta tahta;
    public float kameraUzantisi;
    public float enBoyOrani = 0.625f; // 9:16 oldugu icin 
    public float kenarBosluklari = 2;

    public GameObject arkaPlan;
    private float kameraBaslangicBoyutu;

    void Awake()
    {
        Debug.Log(Screen.width + " uzunlugu da : " + Screen.height);
        enBoyOrani =(float)Screen.width /Screen.height;
        kameraBaslangicBoyutu = Camera.main.orthographicSize;
        tahta = FindObjectOfType<M3Tahta>();
        if (tahta != null)
        {
            KamerayiAyarla(tahta.en - 1, tahta.boy - 1);
        }
    }

    void KamerayiAyarla(float x, float y)
    {
        Vector3 gecici = new Vector3(x / 2, y / 2, kameraUzantisi);
        transform.position = gecici;
        if (tahta.en >= tahta.boy) 
        {
            Camera.main.orthographicSize = (tahta.en / 2.5f + kenarBosluklari) / enBoyOrani;
        }
        else
        {
            Camera.main.orthographicSize = (tahta.boy / 3 + kenarBosluklari + 0.5f) / enBoyOrani;  //tahta.boy/3 idi 
        }
        arkaPlan.transform.localScale = arkaPlan.transform.localScale * (Camera.main.orthographicSize / kameraBaslangicBoyutu);
        arkaPlan.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, arkaPlan.transform.position.z);
    }


}
