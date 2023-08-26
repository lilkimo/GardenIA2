using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    // Aquí van las distintas interfaces para el usuario, 
    // así como esceneas y wea
    public event Action OnMainMenu;
    // public event Action OnItemsMenu;
    // public event Action OnARPosition;
    // public event Action On3DModel
    // public event Action OnWaterCons
    // ...
    public static GameManager instance; // -> Permite que cualquier script acceda a GameManager

    private void Awake(){
        if (instance!=null && instance!=this){
            Destroy(gameObject);
        }
        else{
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        MainMenu();
    }

    // Aquí están las funciones que corresponden a las interfaces o eventos

    public void MainMenu(){
        OnMainMenu?.Invoke();
        Debug.Log("Main Menu Activated");
    }
    // public void ItemsMenu(){
    //     OnItemsMenu?.Invoke();
    //     Debug.Log("Items Menu Activated");
    // }
    // public void ARPosition(){
    //     OnARPosition?.Invoke();
    //     Debug.Log("ARPosition Activated");
    // }
    // public void CloseAPP(){
    //     Application.Quit();
    // }
}
