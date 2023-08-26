using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualGardenManager : MonoBehaviour
{
    [SerializeField] private GameObject agua;
    [SerializeField] public GameObject cifra;
    private int totalHidricConsumption;
    private int estadoTap = 1;
    private int THC {
        get {
            return totalHidricConsumption;
        }
        set{
            totalHidricConsumption = value;
            switch(estadoTap%3) 
            {
            case 1:
                cifra.GetComponent<Text>().text = $"{THC.ToString()}ml/d";
                break;
            case 2:
                cifra.GetComponent<Text>().text = $"{(THC*7).ToString()}ml/s";
                break;
            case 0:
                cifra.GetComponent<Text>().text = $"{(THC*30).ToString()}ml/m";
                break;
            }
        }
    }

    void Start(){
        THC = 0;
    }

    [SerializeField]
    public GameObject terrain;

    public void addPlant(int Consumo){
        THC += Consumo;
        Debug.Log("Se agregó una planta.\nNuevo consumo hídrico del Jardín Virtual: " + totalHidricConsumption);
    }
    public void removePlant(int Consumo){
        THC -= Consumo;
        Debug.Log($"Se quitó una planta (conusmo = {Consumo}).\nNuevo consumo hídrico del Jardín Virtual: " + totalHidricConsumption);
    }
    public int getConsumo(){
        return THC;
    }
    public void Show(){
        //card.GetComponent<RectTransform>() = new Vector3(90,0,0);
    }

    public void cambiarTiempo(){
        estadoTap += 1;
        Debug.Log($"Apretaste el agua {estadoTap} veces");
        switch(estadoTap%3) 
        {
        case 1:
            cifra.GetComponent<Text>().text = $"{THC.ToString()}ml/d";
            break;
        case 2:
            cifra.GetComponent<Text>().text = $"{(THC*7).ToString()}ml/s";
            break;
        case 0:
            cifra.GetComponent<Text>().text = $"{(THC*30).ToString()}ml/m";
            break;
        }
    }
}
