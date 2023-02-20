using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfektRenk : MonoBehaviour
{
    public ParticleSystem top;
    public ParticleSystem halka;


    public void RenkDegis(Color renk)
    {
        ParticleSystem.MainModule settings = top.GetComponent<ParticleSystem>().main;
        settings.startColor = new ParticleSystem.MinMaxGradient(renk);

        ParticleSystem.MainModule settingsHalka = halka.GetComponent<ParticleSystem>().main;
        settingsHalka.startColor = new ParticleSystem.MinMaxGradient(renk);
    }
}
