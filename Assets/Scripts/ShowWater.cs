using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShowWater : MonoBehaviour
{
    // [SerializeField] private GameObject card;
    [SerializeField] public GameObject jardin;
    [SerializeField] public GameObject cifra;
    public void Show(){
        //card.GetComponent<RectTransform>() = new Vector3(90,0,0);
    }
    private int estadoTap = 1;
    public void cambiarTiempo(){
        estadoTap += 1;
        Debug.Log($"Apretaste el agua {estadoTap} veces");
        int consumo = jardin.GetComponent<VirtualGardenManager>().getConsumo();
        switch(estadoTap%3) 
        {
        case 1:
            cifra.GetComponent<Text>().text = $"{consumo.ToString()}/d";
            break;
        case 2:
            cifra.GetComponent<Text>().text = $"{(consumo*7).ToString()}/s";
            break;
        case 0:
            cifra.GetComponent<Text>().text = $"{(consumo*30).ToString()}/m";
            break;
        }
    }
}
