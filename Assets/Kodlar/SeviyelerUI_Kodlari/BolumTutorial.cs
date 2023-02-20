using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class Tutorial
{
    [Multiline(5)]
    public string metin;

    public int tutorialSeviye;
    public GameObject tutorialObje;
}

public class BolumTutorial : MonoBehaviour
{
    public TutorialSC[] tutorialSCs;

    //[Header("tutorial Ayar")]
    //public Tutorial[] tutorials;

    public GameObject tutorialPanel;

    public GameObject solanPanel;

    public GameObject TutorialArkaPlan;

    public GameObject tutorialOlusmaNoktasi;

    public TextMeshProUGUI tutorialMetin;

    M3Tahta tahta;

    private OyunSonuKosuluSM3 kosul;           // Sureli bolum icin uyari
    public Animator hedefPanelAnim;

    private void Start()
    {
        tahta = FindObjectOfType<M3Tahta>();
        seviyedeTutorialVarMi();
    }

    private void seviyedeTutorialVarMi()
    {
        foreach (TutorialSC item in tutorialSCs)
        {
            if(tahta.seviye == item.tutorial.tutorialSeviye)
            {
                //itemdaki Tutoriali Ayarla

                Debug.Log("Seviyede Tutorial VAR");

                GameObject a = Instantiate(item.tutorial.tutorialObje, tutorialOlusmaNoktasi.transform.position, Quaternion.identity);
                a.transform.SetParent(tutorialPanel.transform);
                a.transform.localScale = new Vector3(1, 1, 1);

                tutorialMetin.text = item.tutorial.metin;
            }

        }
        if(tutorialMetin.text == "")
        {
            BolumeGec();

        }
    }

    public void BolumeGec()
    {
        solanPanel.gameObject.SetActive(true);
        tutorialPanel.gameObject.SetActive(false);
        TutorialArkaPlan.gameObject.SetActive(false);
        BolumUyarilariniAyarla();
    }

    public void BolumUyarilariniAyarla()
    {
        if (tahta.alem != null)
        {
            if (tahta.seviye < tahta.alem.seviyeler.Length)
            {
                if (tahta.alem.seviyeler[tahta.seviye] != null)
                {
                    kosul = tahta.alem.seviyeler[tahta.seviye].oyunSonuKosulu;
                }
            }
        }

        if(kosul.kosulTuru == OyunKosulTuruSM3.sure)
        {
            hedefPanelAnim.SetBool("SureliMi", true);
        }

    }
}
