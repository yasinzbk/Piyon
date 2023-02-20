using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ipucu : MonoBehaviour
{
    private Tahta tahta;
    public float ipucuGecikme;
    public float ipucuGecikmeSaniyesi;
    public GameObject ipucuParcacigi;
    public GameObject suankiIpucu;

    // Start is called before the first frame update
    void Start()
    {
        tahta = FindObjectOfType<Tahta>();
        ipucuGecikmeSaniyesi = ipucuGecikme;
    }

    // Update is called once per frame
    void Update()
    {
        if(ipucuGecikmeSaniyesi > 0)
        {
            ipucuGecikmeSaniyesi -= Time.deltaTime;
        }
        if (ipucuGecikmeSaniyesi <= 0 && suankiIpucu == null)
        {
            IpucuOlustur();
           // ipucuGecikmeSaniyesi = ipucuGecikme;
        }
    }

    List<GameObject> TumEslemeleriBul()
    {
        List<GameObject> olasiEslesmeler = new List<GameObject>();
        for (int i = 0; i < tahta.en; i++)
        {
            for (int j = 0; j < tahta.boy; j++)
            {
                if (tahta.tumTaslar[i, j] != null)
                {
                    if (i < tahta.en - 2)
                    {
                        if (tahta.DegistiripKontrolEt(i, j, Vector2.right))
                        {
                            olasiEslesmeler.Add(tahta.tumTaslar[i, j]);
                        }
                    }
                    else
                    {
                        if (tahta.DegistiripKontrolEt(i, j, Vector2.left))
                        {
                            olasiEslesmeler.Add(tahta.tumTaslar[i, j]);
                        }
                    }

                    if (j < tahta.boy - 2)
                    {
                        if (tahta.DegistiripKontrolEt(i, j, Vector2.up))
                        {
                            olasiEslesmeler.Add(tahta.tumTaslar[i, j]);
                        }
                    }
                    else
                    {
                        if (tahta.DegistiripKontrolEt(i, j, Vector2.down))
                        {
                            olasiEslesmeler.Add(tahta.tumTaslar[i, j]);
                        }
                    }
                }
            }

        }
        return olasiEslesmeler;
    }

    GameObject RasgeleEslesebilenTasSec()
    {
        List<GameObject> olasiEslesmeler = new List<GameObject>();
        olasiEslesmeler = TumEslemeleriBul();
        if (olasiEslesmeler.Count > 0)
        {
            int kullanilacakTas = Random.Range(0, olasiEslesmeler.Count);
            return olasiEslesmeler[kullanilacakTas];
        }

        return null;
    }

    private void IpucuOlustur()
    {
        GameObject hareket = RasgeleEslesebilenTasSec();
        if (hareket != null)
        {
            suankiIpucu = Instantiate(ipucuParcacigi, hareket.transform.position, Quaternion.identity);

        }
    }

    public void IpucunuKaldir()
    {
        if (suankiIpucu != null)
        {
            Destroy(suankiIpucu);
            suankiIpucu = null;
            ipucuGecikmeSaniyesi = ipucuGecikme;

        }
    }


    public void IpucuSuresiniDondur()
    {
        ipucuGecikmeSaniyesi = ipucuGecikme;
    }
}
