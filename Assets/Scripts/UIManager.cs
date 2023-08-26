using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] public GameObject Main;
    [SerializeField] public GameObject Plantas;

    [SerializeField] public MenuManager Instance;

    void Start()
    {
        if (Instance == null){
            Debug.Log("UIManager no puede acceder a MenuManager");
        }
        Instance.OnMain += ActivateMainMenu;
        Instance.OnPlantas += ActivateItemsMenu;
    }

    private void ActivateMainMenu(){
        Main.transform.GetChild(0).transform.DOScale(new Vector3(1,1,1),0.3f);
        
        Plantas.transform.GetChild(0).transform.DOScale(new Vector3(0,0,0),0.5f);
        Plantas.transform.GetChild(1).transform.DOScale(new Vector3(0,0,0),0.3f);
        Plantas.transform.GetChild(1).transform.DOMoveY(300, 0.3f);
    }
    private void ActivateItemsMenu(){
        Main.transform.GetChild(0).transform.DOScale(new Vector3(0,0,0),0.3f);
        
        Plantas.transform.GetChild(0).transform.DOScale(new Vector3(1,1,1),0.5f);
        Plantas.transform.GetChild(1).transform.DOScale(new Vector3(1,1,1),0.3f);
        Plantas.transform.GetChild(1).transform.DOMoveY(300, 0.3f);
        
    }
}
