using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Seviye", menuName = "SeviyeSM3")]
public class SeviyeSM3 : ScriptableObject
{
    [Header("Tahta Boyutları")]
    public int en;
    public int boy;

    [Header("Baslangic Fayans")]
    public Fayans[] fayansYerlesim;

    [Header("Taslar")]
    public GameObject[] taslar;

    //[Header("Skor Hedefi")]
    //public int[] skorHedefleri;

    [Header("Oyun Sonu Kosullari")]
    public OyunSonuKosuluSM3 oyunSonuKosulu;
    public BiHedefSM3[] seviyeHedefleri;

    [Header("Oyun Kurallari")]
    public bool turEslesmesiOlacakMi = true;
}
