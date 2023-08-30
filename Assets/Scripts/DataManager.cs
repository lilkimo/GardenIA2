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
    private List<Plant> plants = new List<Plant>();
    [SerializeField]
    private GameObject buttonContainer;
    [SerializeField]
    private ItemButtonManager itemButtonManager;

    [SerializeField]
    private MenuManager menuManager;
    [SerializeField]
    private PlaceController placeController;

    [SerializeField] 
    private GameObject wrap;

    [SerializeField] 
    private LocationManager locationManager;

    // Start is called before the first frame update
    void Start()
    {
        if(menuManager == null)
        {
            Debug.Log("DataManager no puede acceder a MenuManager.");
        }
        menuManager.OnPlantas += CreateButtons;
        placeController.prefab = wrap;
    }

    private void CreateButtons()
    {
        // Habría que hacer que automáticamente se seleccione la primera planta de la lista.
        // (Es más fácil que programar qué pasaría si quieres poner una planta pero no has
        // ninguna seleccionada)
        placeController.SetPlant(plants[0]);
        List<Plant> localPlants = new List<Plant>();
        List<Plant> otherPlants = new List<Plant>();
        foreach (Plant p in plants)
        {
            if(p != null) Debug.Log("Hay una planta: "+p.ItemOrigen[0]);
            if (p.ItemOrigen.Contains(locationManager.UserRegion)) localPlants.Add(p);
            else otherPlants.Add(p);
        }
        localPlants.AddRange(otherPlants);

        foreach (var item in localPlants)
        {
            ItemButtonManager itemButton = Instantiate(itemButtonManager, buttonContainer.transform);

            Debug.Log($"Se creará el bottón de nombre {item.ItemName}, con un consumo de {item.ItemConsumoH2O}.");
            if(item.ItemImage != null) Debug.Log($"Hay imagen {item.ItemImage}.");

            itemButton.Init(item.ItemName, item.ItemConsumoH2O.ToString(), item.ItemImage);
            itemButton.GetComponentInChildren<Toggle>().group = buttonContainer.GetComponent<ToggleGroup>();
            itemButton.button.onClick.AddListener( () => {
                placeController.SetPlant(item);
            });
            if(item.ItemTemp != locationManager.Temperatura || item.ItemSuelo != locationManager.Suelo){
                itemButton.transform.GetChild(4).GetComponent<Image>().enabled = true;
            }
        }
        menuManager.OnPlantas -= CreateButtons;
    }
}
