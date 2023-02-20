using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirilabilirFayans : MonoBehaviour
{
    public int vurusNoktasi;
    private SpriteRenderer resim;
    private HedefYoneticisiSM3 hedefYoneticisi;
    public FayansTur Tip;

    public bool olebilir = true;

    private void Start()
    {
        hedefYoneticisi = FindObjectOfType<HedefYoneticisiSM3>();
        resim = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //if (vurusNoktasi <= 0)
        //{
        //    if (gameObject.GetComponent<Animator>())
        //    {               
        //        gameObject.GetComponent<Animator>().SetBool("Kiril", true);
        //    }

        //    if (hedefYoneticisi != null)
        //    {
        //        hedefYoneticisi.FayansiKarsilastir(Tip);
        //    }
        //    Oldur(olebilir);
        //}
    }

    public void HasarAl(int hasar)
    {
        vurusNoktasi -= hasar;
        Sondur();

        if (vurusNoktasi <= 0)
        {
            if (gameObject.GetComponent<Animator>())
            {
                gameObject.GetComponent<Animator>().SetBool("Kiril", true);
            }

            if (hedefYoneticisi != null)
            {
                hedefYoneticisi.FayansiKarsilastir(Tip);
            }
            Oldur(olebilir);
        }
    }

    void Sondur()
    {
        Color color = resim.color;

        float yeniAlfa = color.a * .5f;
        resim.color = new Color(color.r, color.g, color.b, yeniAlfa);
    }


    public void Olebilir()
    {
        olebilir = true;
        if (olebilir)
        {
            Destroy(this.gameObject);
        }
    }

    private void Oldur(bool olebilir)
    {
        if (olebilir)
        {
            Destroy(this.gameObject);
        }
    }

}
