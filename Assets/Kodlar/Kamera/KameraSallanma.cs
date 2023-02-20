using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraSallanma : MonoBehaviour
{
        Vector3 orjinalPoz;


    private void Start()
    {
        orjinalPoz = transform.localPosition;
    }

    public IEnumerator Salla(float surec, float buyukluk)
    {

        float gecenZaman = 0.0f;

        while (gecenZaman < surec)
        {
            float x = Random.Range(-1f, 1f) * buyukluk;
            float y = Random.Range(-1f, 1f) * buyukluk;

            transform.localPosition = new Vector3(orjinalPoz.x +x, orjinalPoz.y + y, orjinalPoz.z);

            gecenZaman += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = orjinalPoz;
    }
}
