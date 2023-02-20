using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkaPlanFayans : MonoBehaviour
{
    public int vurusNoktasi;
    private SpriteRenderer resim;
    private HedefYoneticisi hedefYoneticisi;

    private void Start()
    {
        hedefYoneticisi = FindObjectOfType<HedefYoneticisi>();
        resim = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (vurusNoktasi <= 0)
        {
            if (hedefYoneticisi != null)
            {
                hedefYoneticisi.HedefiKarsilastir("Kirilabilir");
            }
            Destroy(this.gameObject);
        }
    }

    public void HasarAl(int hasar)
    {
        vurusNoktasi -= hasar;
        Sondur();
    }

    void Sondur()
    {
        Color color = resim.color;

        float yeniAlfa = color.a * .5f;
        resim.color = new Color(color.r, color.g, color.b, yeniAlfa);
    }
}
