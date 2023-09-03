using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    private Plant[] plants;// = new List<Plant>();
    [SerializeField]
    private ToggleGroup buttonContainer;
    [SerializeField]
    private ItemButtonManager itemPrefab;

    [SerializeField]
    private MenuManager menuManager;
    [SerializeField]
    private PlaceController placeController;

    [SerializeField] 
    private LocationManager locationManager;
    [SerializeField]
    private Button changeGrowthButton;
    [SerializeField]
    private Button changeModelSeasonButton;

    [SerializeField]
    private Transform virtualGarden;

    void Awake()
    {
        plants = Resources.LoadAll<Plant>("Plants");
    }
    void Start()
    {
        if(menuManager == null)
            Debug.Log("DataManager no puede acceder a MenuManager.");
        menuManager.OnPlantas += CreateButtons;
        changeGrowthButton.onClick.AddListener(ChangeGrowthPlants);
        changeModelSeasonButton.onClick.AddListener(ChangeModelSeason);
    }

    private void CreateButtons()
    {
        List<Plant> localPlants = new();
        List<Plant> otherPlants = new();
        foreach (Plant plant in plants)
        {
            if (plant.ItemOrigen.Contains(locationManager.UserRegion))
                localPlants.Add(plant);
            else
                otherPlants.Add(plant);
        }
        localPlants.AddRange(otherPlants);

        foreach (var item in localPlants)
        {
            ItemButtonManager itemButton = Instantiate(itemPrefab, buttonContainer.transform);
            itemButton.Init(buttonContainer, item.ItemName, item.ItemConsumoH2O.ToString(), item.ItemImage);
            itemButton.toggle.onValueChanged.AddListener( value => {
                if (value)
                    placeController.SetPlant(item);
            });
            if (item.ItemTemp != locationManager.Temperatura || item.ItemSuelo != locationManager.Suelo)
                itemButton.warning.enabled = true;
        }
        menuManager.OnPlantas -= CreateButtons;
    }

    private void ChangeGrowthPlants()
    {
        PlantDisplay plant;
        foreach (Transform child in virtualGarden)
        {
            plant = child.GetComponent<PlantDisplay>();
            if (plant == null)
                continue;
            plant.ChangeGrowthStage((plant.currGrowthStage + 1) % 3);
        }
    }

    private void ChangeModelSeason()
    {
        PlantDisplay plant;
        foreach (Transform child in virtualGarden)
        {
            plant = child.GetComponent<PlantDisplay>();
            if (plant == null)
                continue;
            plant.ChangeModelBySeason((plant.currSeasonModel + 1) % 4);
        }
    }
}
