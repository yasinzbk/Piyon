using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HedefPaneli : MonoBehaviour
{
    public Image buImage;
    public Sprite buSprite;
    public TextMeshProUGUI buText;
    public string buString;
    public Color buRenk;
    public GameObject tamamlandiIsareti;

    // Start is called before the first frame update
    void Start()
    {
        Kur();
    }

    void Kur()
    {
        buImage.sprite = buSprite;
        buImage.color = buRenk; // burayi ac satrancta iken
        buText.text = buString;
    }
}
