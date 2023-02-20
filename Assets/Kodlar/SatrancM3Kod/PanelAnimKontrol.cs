using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimKontrol : MonoBehaviour
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
        M3Tahta tahta = FindObjectOfType<M3Tahta>();
        tahta.suankiDurum = OyunDurumu.hareket;
    }
}
