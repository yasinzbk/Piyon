using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AyarlarButon : MonoBehaviour
{
    private bool AyarlariAc = false;
    public Animator ayarlar;

    public void AyarlariAcKapat()
    {
        AyarlariAc = !AyarlariAc;

        if (AyarlariAc)
        {
            ayarlar.SetBool("AyarlarAc", true);
        }
        else
        {
            ayarlar.SetBool("AyarlarAc", false);
        }
    }

}
