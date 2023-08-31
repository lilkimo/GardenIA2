using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEditor.MemoryProfiler;
using Unity.VisualScripting;

public class SOGenJardinManager : MonoBehaviour
{
     // Valores de la localización del usuario
    [SerializeField] private LocationManager locationManager;

    // Criterios definidos por el usuario:
    [SerializeField] private GameObject costo;
    [SerializeField] private GameObject consumo;
    [SerializeField] private GameObject densidad;
    [SerializeField] private GameObject mantencion;
    [SerializeField] private GameObject flujo;
    [SerializeField] private GameObject blacklist;
    [SerializeField] private GameObject whitelist;
    [SerializeField] private PlantaVetableConstructor plantaVetableConstructor;

    // Data de todas las plantas:
    [SerializeField] private List<Plant> allPlants = new List<Plant>();

    // Constructor de tarjetas de plantas para jardín generado y el contenedor de estas:
    [SerializeField] private PlantaJardinConstructor plantaJardinConstructor;
    [SerializeField] private GameObject VistaPlantasJardin;

    // Struct para almacenar las plantas escogidas para el jardín (panta, cantidad, conflictos presentes)
    private class PlantaInfo
    {
        public Plant data;
        public int cantidad;
        public List<string> conflictos;

        public PlantaInfo(Plant data, List<string> conflictos){
            this.data = data;
            this.cantidad = 1;
            this.conflictos = conflictos;
        }
    }

    // void GenerarJardin():
    // La función recupera todos los criterios ingresados por el usuario y 
    // luego ejecuta la función que genera el listado de plantas a colocar.
    public void GenerarJardin(){
        int tamañoJardin = 200; //En m^2

        float costo_v = costo.GetComponentInChildren<Slider>().value * 300000;
        float consumo_v = consumo.GetComponentInChildren<Slider>().value * tamañoJardin * 20000; //Paso de m^2 a cm^3 == ml
        Plant.Levels densidad_v = ParseToLevel(GetSelectedToggle(densidad).GetComponentInChildren<Text>().text);
        bool mantencion_v = mantencion.GetComponentInChildren<Toggle>().isOn;
        Plant.Levels flujo_v = ParseToLevel(GetSelectedToggle(flujo).GetComponentInChildren<Text>().text);
        List<string> blacklist_v = GetAllBlacklist(blacklist);
        
        Dictionary<string, PlantaInfo> plantasParaJardin = PickPlants(costo_v, consumo_v, densidad_v, mantencion_v, flujo_v, blacklist_v);

        Debug.Log("PickPlants finalizado.");

        foreach (var plant in plantasParaJardin)
        {
            Debug.Log(String.Format("Se creará la siguiente planta:\nNombre: {0}\nCantidad: {1}\nUbicación: {2}\nPrecio: {3}\nConsumo: {4}[ml/s]",plant.Key, plant.Value.cantidad, plant.Value.data.ItemDescription, plant.Value.data.ItemPrecio, plant.Value.data.ItemConsumoH2O));
            PlantaJardinConstructor constructor;
            constructor = Instantiate(plantaJardinConstructor, VistaPlantasJardin.transform);
            constructor.Imagen = plant.Value.data.ItemImage;
            constructor.Nombre = plant.Key;
            constructor.Cantidad = plant.Value.cantidad.ToString();
            constructor.Ubicacion = plant.Value.data.ItemDescription;
            constructor.Precio = plant.Value.data.ItemPrecio.ToString();
            constructor.Consumo = plant.Value.data.ItemConsumoH2O.ToString();

            Debug.Log(String.Format("Los conflictos de {0} son: ",plant));
            plant.Value.conflictos.ForEach(x => Debug.Log(x));
            if(plant.Value.conflictos.Any()) {
                constructor.Conflictos = "Tiene conflictos con: " + String.Join(", ",plant.Value.conflictos);
            }
            else constructor.Conflictos = "";
        }
    }
    // Devuelve el toggle encendido dentro de un grupo.
    Toggle GetSelectedToggle(GameObject parent) {
        Toggle[] toggles = parent.GetComponentsInChildren<Toggle>();
        foreach (var t in toggles)
        if (t.isOn) return t;  //returns selected toggle
        return null;           // if nothing is selected return null
    }
    // Devuelve los nombres de todas las platnas betadas manualmente por el usuario.
    List<string> GetAllBlacklist(GameObject container){
        List<string> nombres = new List<string>();
        foreach(Transform child in container.transform){
            nombres.Add(child.GetChild(0).GetComponent<TextMeshProUGUI>().text);
        }
        return nombres;
    }
    Plant.Levels ParseToLevel(string adj){
        switch (adj.Remove(adj.Length - 1, 1))
        {
            case "Baj":
                return Plant.Levels.Level1;
            case "Medi":
                return Plant.Levels.Level2;
            case "Alt":
                return Plant.Levels.Level3;
            default:
                return Plant.Levels.Level1;
        }
    }

    // Dictionary<string, List<string>> PickPlants():
    // La función selecciona plantas en un bucle limitado por 2 cantidades: el saldo del costo del jardín y 
    // el saldo del consumo de agua del jardín. Debido a que aún está en etapa inicial, por ahora recomienda 
    // plantas aleatorias que cumplan con los criterios del usuario y los de la ubicación:
    // - Temperatura y tipo de suelo son condiciones booleanas
    // - Conflicto interespecie agrega una advertencia por ahora. Más adelante determianra un radio que las 
    //   plantas conflictivas no podrán sobreponer.
    // - Flora local aumenta el peso de la planta para la selección aleatoria.
    private Dictionary<string, PlantaInfo> PickPlants(float costoMax, float consumoMax, Plant.Levels densidad, bool mantencion, Plant.Levels flujo, List<string> banned){

        Debug.Log("PickPlants en proceso...");

        float saldoCosto = costoMax;
        float saldoConsumo = consumoMax;

        System.Random rand = new System.Random();
        Dictionary<string, PlantaInfo> plantasJardin = new Dictionary<string, PlantaInfo>();

        if(!allPlants.Any()) Debug.Log("allPlants vacio.");
        List<int> weightedPlants = Enumerable.Range(0, allPlants.Count).ToList();
        int i = 0;
        while(i < allPlants.Count){
            if(allPlants[i].ItemOrigen.Contains(locationManager.UserRegion)) weightedPlants.Add(i);
            i++;
        }

        Plant planta = allPlants[weightedPlants[rand.Next(weightedPlants.Count)]];
        if (planta != null) Debug.Log("Se escogió una planta aleatoria: "+ planta.ItemName);

        int cont = 20;
        while((saldoCosto - planta.ItemPrecio >= 0) && (saldoConsumo - planta.ItemConsumoH2O >= 0) && plantasJardin.Sum(x => x.Value.cantidad) <= 10 && cont > 0){
            Debug.Log("Se inicia el proceso de validación...");

            if (planta.ItemDensidad <= densidad // Si la densidad es adecuada [cambiar a futuro]
            && !(planta.ItemMantencion && mantencion) // Si la planta no requiere manetnción y se especificó una baja mantención
            && planta.ItemResistencia >= flujo // Si el flujo al que la planta está acostrumbrada es apropiado
            && !banned.Contains(planta.ItemName) // Si la planta no fue excluida manualmente
            && planta.ItemTemp == locationManager.Temperatura // Si a planta es adecuada a la temperatura de la ubicación del usuario
            && planta.ItemSuelo == locationManager.Suelo // Si la palnta es adecuada al tipo de suelo de la ubicación del usuario
            )
            {
                Debug.Log("La planta "+planta.ItemName+" cumple con los criterios.");


                if (plantasJardin.ContainsKey(planta.ItemName)){
                    plantasJardin[planta.ItemName].cantidad++;
                    Debug.Log("La planta "+planta.ItemName+" ya se había escogido previamente. Ahora hay "+plantasJardin[planta.ItemName].cantidad+".");
                }
                else {
                    Debug.Log("La planta "+planta.ItemName+" no había sido escogida anteriormente. Agregando...");

                    List<string> conflictsFound = CheckConflict(planta.ItemConflictos.ToList(), plantasJardin.Keys.ToList());
                    plantasJardin.Add(planta.ItemName, new PlantaInfo(planta, conflictsFound));
                    
                    foreach (string conflict in conflictsFound)
                    {
                        plantasJardin[conflict].conflictos.Add(planta.ItemName);
                    }
                }
            }
            else Debug.Log($@"La planta {planta} no cumple con los criterios:
            Densidad escogida: {densidad}. Densidad de la planta: {planta.ItemDensidad}
            Mantención: {mantencion}. Mantención de la planta: {planta.ItemMantencion}
            Flujo escogido: {flujo}. Flujo de la planta: {planta.ItemResistencia}
            Temperatura del usuario: {locationManager.Temperatura}. Temperatura de la planta: {planta.ItemTemp}
            Tipo de suelo del usuario: {locationManager.Suelo}. Tiopo de suelo de la planta: {planta.ItemSuelo}
            ");

            planta = allPlants[weightedPlants[rand.Next(weightedPlants.Count)]];
            if (planta != null) Debug.Log("Se escogió una planta aleatoria: "+ planta.ItemName);

            cont--;
        }
        return plantasJardin;
    }
    // // Corrobora que sea correcta la densidad.
    // bool IsDensityApropiate(string plantaValue, string condition){
    //     if (condition == "Baja" && (plantaValue == "Media" || plantaValue == "Alta")) return false;
    //     else if (condition == "Media" && plantaValue == "Alta") return false;
    //     else return true;
    // }
    // // Corrobora que la planta sea apta para el flujo
    // bool IsFlowApropiate(string plantaValue, string condition){
    //     if (condition == "Alta" && (plantaValue == "Media" || plantaValue == "Baja")) return false;
    //     else if (condition == "Media" && plantaValue == "Baja") return false;
    //     else return true;
    // }

    // List<string> CheckConflict(conlfictos, seleccionadas):
    // Busca si plantas con las que la panta seleccionada 
    // tiene conflictos también fueron seleccionadas y las 
    // retorna.
    List<string> CheckConflict(List<string> conflictos, List<string> seleccionadas){
        Debug.Log("Buscando conflictos ...");
        List<string> conflictsFound = new List<string>();
        foreach (string plantaConflicto in conflictos)
        {
            if(seleccionadas.Contains(plantaConflicto)){
                conflictsFound.Add(plantaConflicto);
            }
        }
        if (!conflictos.Any()) Debug.Log("No hay conflictos");
        else Debug.Log("Se encontraron "+conflictsFound.Count+" conflictos");
        return conflictsFound;
    }

    private void Start() {
        foreach (var planta in allPlants)
        {
            PlantaVetableConstructor cons;
            cons = Instantiate(plantaVetableConstructor, whitelist.transform);
            cons.NombrePlanta = planta.ItemName;
            cons.DescripcionPlanta = planta.ItemDescription;
            cons.ImagenPlanta = planta.ItemImage;
        }
    }
    public void ListWLPlants(){
        foreach (var planta in allPlants)
        {
            PlantaVetableConstructor cons;
            cons = Instantiate(plantaVetableConstructor, whitelist.transform);
            cons.NombrePlanta = planta.ItemName;
            cons.DescripcionPlanta = planta.ItemDescription;
            cons.ImagenPlanta = planta.ItemImage;
        }
    }
    public void DeleteWLPlants(){
        foreach(Transform child in whitelist.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in blacklist.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
