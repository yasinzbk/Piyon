using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Seviye", menuName = "Seviye")]
public class Seviye : ScriptableObject
{
    [Header("Tahta Boyutları")]
    public int en;
    public int boy;

    [Header("Baslangic Fayans")]
    public FayansTipiYeri[] fayansYerlesim;

    [Header("Taslar")]
    public GameObject[] taslar;

    //[Header("Skor Hedefi")]
    //public int[] skorHedefleri;

    [Header("Oyun Sonu Kosullari")]
    public OyunSonuKosulu oyunSonuKosulu;
    public BiHedef[] seviyeHedefleri;


}
