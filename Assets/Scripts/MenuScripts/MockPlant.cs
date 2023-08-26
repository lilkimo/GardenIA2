using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MockPlant : ScriptableObject
{
    public string Name;
    public Sprite Image;
    public string Description;
    public string Placement;
    public GameObject Model;
    public int Price;
    public int Consumption;
    public string Density;
    public string Maintenance;
    public string Resilience;
    public string Origin;
    public string Temperature;
    public string Soil;
    public List<string> Conflicts;
}
