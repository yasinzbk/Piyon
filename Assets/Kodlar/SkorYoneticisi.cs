using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkorYoneticisi : MonoBehaviour
{
    public TextMeshProUGUI skorText;
    public int skor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        skorText.text = "" + skor;
    }

    public void SkoruArttir(int arttirilacakMiktar)
    {
        skor += arttirilacakMiktar;
    }
}
