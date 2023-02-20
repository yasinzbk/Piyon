using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParcaOlusturma : MonoBehaviour
{
    public int yukseklik;
    public int en;
    public List<GameObject> parcalar;
    public GameObject kutucuk;

    int sira;
    private void Awake()
    {
        ParcaOlustur();
    }


    public void ParcaOlustur()
    {
        for (int y = 0; y < yukseklik; y++)
        {
            for (int x = 0; x < en; x++)
            {
                Instantiate(kutucuk, new Vector2(x, y), Quaternion.identity);
                GameObject gelenObje = Instantiate(parcalar[Random.Range(0, parcalar.Count)], new Vector2(x, y), Quaternion.identity);
                sira++;
                gelenObje.GetComponent<TaslarTiklamaAyniMiKontrol>().sira = sira;
            }
        }
    }



}
