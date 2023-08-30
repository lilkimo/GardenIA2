using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class PlantDisplay: MonoBehaviour
{
    public Plant plant { get; private set; }
    public int currGrowthStage { get; private set; }
    private GameObject model;
    
    void Start() {
        // Initialize(TEST_plant, transform.localPosition + Vector3.forward, transform.localRotation, transform.localScale);
        Serialize();
    }

    public PlantDisplay Initialize(Plant plant, Vector3 position, Quaternion rotation, Vector3 scale, int currGrowthStage = 0)
    {
        this.plant = plant;
        transform.localPosition = position;
        transform.localRotation = rotation;
        transform.localScale = scale;
        this.currGrowthStage = currGrowthStage;
        model = Instantiate(plant.GrowthStages[currGrowthStage], transform);
        return this;
    }

    public PlantDisplay Initialize(SerializablePlant plant)
    {
        Plant plantObj = Resources.LoadAll<Plant>("Plants").Single(resource => resource.arObjectIdentifier == plant.plant);
        Initialize(plantObj, plant.position, plant.rotation, plant.scale);
        return this;
    }

    public void ChangeGrowthStage(int stage)
    {
        currGrowthStage = stage;        
        Destroy(model);
        model = Instantiate(plant.GrowthStages[stage], transform);
    }

    public void Serialize()
    {
        string json = JsonUtility.ToJson(new SerializablePlant(14, transform.localPosition, transform.localRotation, transform.localScale));
        Debug.Log(json);
        SerializablePlant info = JsonUtility.FromJson<SerializablePlant>(json);
        // Debug.Log(info.plant);
        // Debug.Log(info.position);
        // Debug.Log(info.rotation);
        // Debug.Log(info.scale);
    }
}
