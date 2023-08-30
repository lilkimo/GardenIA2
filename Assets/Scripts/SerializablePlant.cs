using System;
using UnityEngine;

[Serializable]
public class SerializablePlant
{
    public int plant;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public SerializablePlant(int plant, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        this.plant = plant;
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }
}