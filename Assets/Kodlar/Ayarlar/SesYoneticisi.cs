using UnityEngine.Audio;
using System;
using UnityEngine;

public class SesYoneticisi : MonoBehaviour
{

    public Ses[] sesler;

    public static SesYoneticisi orn;

    void Awake()
    {
        if (orn == null)
        {
            orn = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Ses ses in sesler)
        {
            ses.seskaynagi = gameObject.AddComponent<AudioSource>();
            ses.seskaynagi.clip = ses.klip;

            ses.seskaynagi.volume = ses.sesYuksekligi;
            ses.seskaynagi.pitch = ses.perde;
            ses.seskaynagi.loop = ses.tekrarla;
        
        }
    }

    public void Oynat(string ad) // Sesin nerede baslamasini istiyorsak o fonksiyonda bunu calistiracagiz
    {
        Ses s = Array.Find(sesler, Ses => Ses.ad == ad);
        if (s == null)
        {
            Debug.LogWarning("SES:" + ad + " bulunamadi");
            return;
        }
        s.seskaynagi.Play();
    }
}
