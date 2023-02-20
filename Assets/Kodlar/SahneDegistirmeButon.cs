using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SahneDegistirmeButon : MonoBehaviour
{

    public void SeviyeyiYukle(string yuklenecekSeviye)
    {
        SceneManager.LoadScene(yuklenecekSeviye);
    }

}
