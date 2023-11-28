using TMPro;
using UnityEngine;

public class SeviyeAyarla : MonoBehaviour, ISaveData
{
    public TextMeshProUGUI seviyeText;
    public int seviye = 1;
    public int alemNo = 0;

    private DilKontrol dilKontrol;
    public GameObject onayPaneli;

    public GameObject oyunSonuPaneli;
    public int sonBolum;

    private void Start()
    {
        dilKontrol = FindObjectOfType<DilKontrol>();
        SeviyeyiGoster();

        if (OyunBittiKontrol(seviye))
        {
            seviye = sonBolum;  // son bolumun ardina gidilmesin diye yapilmis geicci onlem
            onayPaneli.GetComponent<OnayPaneli>().seviye = sonBolum;

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

    public bool OyunBittiKontrol(int seviye)
    {

        if (seviye >= sonBolum)
        {
            oyunSonuPaneli.SetActive(true);
            return true;
        }

        return false;
    }
}
