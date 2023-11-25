using TMPro;
using UnityEngine;

public class SeviyeAyarla : MonoBehaviour, ISaveData
{
    public TextMeshProUGUI seviyeText;
    public int seviye = 1;
    public int alemNo = 0;

    private DilKontrol dilKontrol;
    public GameObject onayPaneli;

    public OyunBitti oyunBittiKontrol;

    private void Start()
    {
        dilKontrol = FindObjectOfType<DilKontrol>();
        //VeriyiYukle();
        SeviyeyiGoster();

        if (oyunBittiKontrol.OyunBittiKontrol(seviye))
        {
            seviye = oyunBittiKontrol.sonBolum;  // son bolumun ardina gidilmesin diye yapilmis geicci onlem
            onayPaneli.GetComponent<OnayPaneli>().seviye = oyunBittiKontrol.sonBolum;

            if (dilKontrol.sahneDili == Dil.Turkce)
            {
                seviyeText.text = "bonus seviye";
            }
            else
            {
                seviyeText.text = "bonus level";
            }

        }
    }

    public void SeviyeyiGoster()
    {
        if (dilKontrol.sahneDili == Dil.Turkce)
        {
            seviyeText.text = "seviye " + seviye;
        }
        else
        {
            seviyeText.text = "level " + seviye;
        }
    }


    //void VeriyiYukle(SaveData data)
    //{
    //    int i = 0;
    //    if (data != null)
    //    {

    //        while (data.aktifMi[alemNo, i])
    //        {
    //            i++;
    //        }

    //        seviye = i;
    //    }

    //    onayPaneli.GetComponent<OnayPaneli>().seviye = seviye;
    //}

    public void LoadData(SaveData data)
    {
        //VeriyiYukle(data);

        seviye = data.suankiSeviye;
        onayPaneli.GetComponent<OnayPaneli>().seviye = seviye;
    }

    public void SaveData(SaveData data)
    {
        data.suankiSeviye = seviye;
    }
}
