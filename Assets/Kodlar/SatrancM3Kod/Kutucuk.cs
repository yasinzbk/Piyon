using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kutucuk : MonoBehaviour
{
    public GameObject YesilKutu;
    public GameObject SiyahKutu;



    public void YesilKutuYak()
    {
        YesilKutu.SetActive(true);
    }
    public void SiyahKutuYak()
    {
        SiyahKutu.SetActive(true);
    }

    public void SiyahKutuSondur()
    {
        SiyahKutu.SetActive(false);
    }
    public void YesilKutuSondur()
    {
        YesilKutu.SetActive(false);
    }

}
