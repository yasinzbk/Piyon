using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YokEtme : MonoBehaviour
{

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
