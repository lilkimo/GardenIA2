using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantaVetableConstructor : MonoBehaviour
{
    private string nombrePlanta;
    private string descripcionPlanta;
    private Sprite imagenPlanta;

    public string NombrePlanta { set => nombrePlanta = value;}
    public string DescripcionPlanta { set => descripcionPlanta = value;}
    public Sprite ImagenPlanta { set => imagenPlanta = value;}

    void Start()
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = nombrePlanta;
        // transform.GetChild(1).GetComponent<Image>().sprite =  imagenPlanta;
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =  descripcionPlanta;
    }
}
