using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirisPanelKontrolcusu : MonoBehaviour
{
    public Animator solukPanelAnim;
    public Animator hedefBilgisiAnim;

    public void Tamam()
    {
        if (solukPanelAnim != null && hedefBilgisiAnim != null)
        {
            solukPanelAnim.SetBool("CiksinMi", true);
            hedefBilgisiAnim.SetBool("CiksinMi", true);
            StartCoroutine(OyunuBaslatCo());
        }
       
    }

    public void OyunBitti()
    {
        solukPanelAnim.SetBool("CiksinMi", false);
        solukPanelAnim.SetBool("OyunBittiMi", true);
    }

    IEnumerator OyunuBaslatCo()
    {
        yield return new WaitForSeconds(1f);
        Tahta tahta = FindObjectOfType<Tahta>();
        tahta.suankiDurum = OyunDurumu2.hareket;
    }
}
