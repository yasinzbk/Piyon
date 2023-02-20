using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Ses
{
    public string ad;

    public AudioClip klip;

    [Range(0f, 1f)]
    public float sesYuksekligi;
    [Range(.1f, 3f)]
    public float perde;

    public bool tekrarla;

    [HideInInspector]
    public AudioSource seskaynagi;
}
