using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantDisplay: MonoBehaviour
{
    public Plant plant;
    
    void Start() {
        // Initialize(TEST_plant, transform.localPosition + Vector3.forward, transform.localRotation, transform.localScale);
        Serialize();
    }

    public GameObject Initialize(Plant plant, Vector3 position, Quaternion rotation, Vector3 scale) {
        Debug.Log("Inicializando");
        this.plant = plant;
        transform.localPosition = position;
        transform.localRotation = rotation;
        transform.localScale = scale;
        return Instantiate(plant.GrowthStages[0], transform);
    }

    public GameObject Initialize(SerializablePlant plant) {
        Plant _plant = this.plant; // Aquí habría que buscar el <Plant> que matché <plant.plant>
        return Initialize(_plant, plant.position, plant.rotation, plant.scale);
    }

    public void Serialize() {
        string json = JsonUtility.ToJson(new SerializablePlant(14, transform.localPosition, transform.localRotation, transform.localScale));
        Debug.Log(json);
        SerializablePlant info = JsonUtility.FromJson<SerializablePlant>(json);
        Debug.Log(info.plant);
        Debug.Log(info.position);
        Debug.Log(info.rotation);
        Debug.Log(info.scale);
    }
}
