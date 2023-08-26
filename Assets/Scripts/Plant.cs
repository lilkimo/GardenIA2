using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Plant : ScriptableObject
{
    public bool ItemSol; // si = planta necesita sol, no = no sol
    public Levels ItemTamano; // 1 = pequeña, 2 = mediana, 3 = grande
    public string ItemName; // nombre planta
    public Sprite ItemImage; // imagen q se muestra en UI
    public string ItemDescription;
    public GameObject[] GrowthStages; // almacena los modelos 3D
    public int ItemPrecio;
    public int ItemConsumoH2O;
    public bool ItemMantencion;
    public Levels ItemDensidad;
    public Levels ItemResistencia;
    public string ItemOrigen; // ingresar string con nombre región
    public Levels ItemTemp; // 1 = baja, 2 = media, 3 = alta
    public Suelos ItemSuelo;
    public string[] ItemConflictos; // ingresar nombre de plantas

    public enum Levels
    {
        Level1,
        Level2,
        Level3,
    };
    public enum Suelos
    {
        Alcalino,
        Acido,
    };
}