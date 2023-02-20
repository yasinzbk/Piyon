using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SallanmaDeneme : MonoBehaviour
{
    public KameraSallanma kameraSallanma;

    public float sallanmaSuresi = .15f;
    public float sallanmaMiktari = .4f;

    public GameObject efekObjesi;
    public GameObject efekObjesi2;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(kameraSallanma.Salla(sallanmaSuresi, sallanmaMiktari));

            efekObjesi.transform.position = new Vector2( 0,0);
            efekObjesi2.transform.position = new Vector2(0,0);

        }


        efekObjesi.transform.position = new Vector2(efekObjesi.transform.position.x, efekObjesi.transform.position.y + Time.deltaTime * 8f);
        efekObjesi2.transform.position = new Vector2(efekObjesi2.transform.position.x, efekObjesi2.transform.position.y - Time.deltaTime * 8f);
    }
}
