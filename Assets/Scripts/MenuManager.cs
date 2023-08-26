using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PlaceController))]
public class MenuManager : MonoBehaviour
{
    private PlaceController placeController;
    public event Action OnPlantas;
    public event Action OnMain;

    void Awake()
    {
        placeController = GetComponent<PlaceController>();
    }
    void Start() => Main();

    public void Main()
    {
        placeController.DisablePlaceMode();
        OnMain?.Invoke();
    }
    public void Plantas()
    {
        OnPlantas?.Invoke();
        placeController.EnablePlaceMode();
    }
}
