using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantaJardinConstructor : MonoBehaviour
{
    private Sprite imagen;
    private string nombre;
    private string cantidad;
    private string ubicacion;
    private string precio;
    private string consumo;
    private string conflictos;

    public Sprite Imagen {set => imagen = value;}
    public string Nombre { set => nombre = value;}
    public string Cantidad { set => cantidad = value;}
    public string Ubicacion { set => ubicacion = value;}
    public string Precio { set => precio = value;}
    public string Consumo { set => consumo = value;}
    public string Conflictos { set => conflictos = value;}

    void Start()
    {
        transform.GetChild(0).GetComponent<Image>().sprite = imagen;
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = nombre;
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = cantidad;
        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = ubicacion;
        transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "$"+precio;
        transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = consumo+"[ml/d]";
        transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = conflictos;
    }
}
