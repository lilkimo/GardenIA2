using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GenJardinManager : MonoBehaviour
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

    [SerializeField] private List<MockPlant> todasPlantas = new List<MockPlant>();

    // Constructor de tarjetas de plantas para jardín generado y el contenedor de estas:
    [SerializeField] private PlantaJardinConstructor plantaJardinConstructor;
    [SerializeField] private GameObject VistaPlantasJardin;

    // void GenerarJardin():
    // La función recupera todos los criterios ingresados por el usuario y 
    // luego ejecuta la función que genera el listado de plantas a colocar.
    public void GenerarJardin(){
        int tamañoJardin = 200; //En m^2

        float costo_v = costo.GetComponentInChildren<Slider>().value * 300000;
        float consumo_v = consumo.GetComponentInChildren<Slider>().value * tamañoJardin * 20000; //Paso de m^2 a cm^3 == ml
        string densidad_v = GetSelectedToggle(densidad).GetComponentInChildren<Text>().text;
        bool mantencion_v = mantencion.GetComponentInChildren<Toggle>().isOn;
        string flujo_v = GetSelectedToggle(flujo).GetComponentInChildren<Text>().text;
        List<string> blacklist_v = GetAllBlacklist(blacklist);
        
        Dictionary<string, List<string>> plantasParaJardin = PickPlants(costo_v, consumo_v, densidad_v, mantencion_v, flujo_v, blacklist_v);

        Debug.Log("PickPlants finalizado.");

        foreach (var plant in plantasParaJardin)
        {
            Debug.Log(String.Format("Se creará la siguiente planta:\nNombre: {0}\nCantidad: {1}\nUbicación: {2}\nPrecio: {3}\nConsumo: {4}[ml/s]",plant.Key, plant.Value[0], plant.Value[1], plant.Value[2], plant.Value[3]));
            PlantaJardinConstructor constructor;
            constructor = Instantiate(plantaJardinConstructor, VistaPlantasJardin.transform);
            constructor.Nombre = plant.Key;
            constructor.Cantidad = plant.Value[0];
            constructor.Ubicacion = plant.Value[1];
            constructor.Precio = plant.Value[2];
            constructor.Consumo = plant.Value[3];
            Debug.Log(String.Format("Los conflictos de {0} son: {1}",plant,plant.Value[4]));
            if(plant.Value[4] != "" && plant.Value[4] != null) {
                if(plant.Value[4][0] == ','){plant.Value[4] = plant.Value[4].Remove(0,2);}
                constructor.Conflictos = "Tiene conflictos con: " + plant.Value[4].Remove(0,2);
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
    

    // Dictionary<string, List<string>> PickPlants():
    // La función selecciona plantas en un bucle limitado por 2 cantidades: el saldo del costo del jardín y 
    // el saldo del consumo de agua del jardín. Debido a que aún está en etapa inicial, por ahora recomienda 
    // plantas aleatorias que cumplan con los criterios del usuario y los de la ubicación:
    // - Temperatura y tipo de suelo son condiciones booleanas
    // - Conflicto interespecie agrega una advertencia por ahora. Más adelante determianra un radio que las 
    //   plantas conflictivas no podrán sobreponer.
    // - Flora local aumenta el peso de la planta para la selección aleatoria.
    public Dictionary<string, List<string>> PickPlants(float costoMax, float consumoMax, string densidad, bool mantencion, string flujo, List<string> banned){

        Debug.Log("PickPlants en proceso...");

        float saldoCosto = costoMax;
        float saldoConsumo = consumoMax;
        Dictionary<string, List<string>> plantasJardin = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> allPlants = new Dictionary<string, List<string>>(); // parseXml.infoPlantas;
        
        string planta = WeightedRandPlant(allPlants.Keys.ToList());
        if (planta != null) Debug.Log("Se escogió una planta aleatoria: "+ planta);

        while((saldoCosto - int.Parse(allPlants[planta][2]) >= 0) && (saldoConsumo - int.Parse(allPlants[planta][3]) >= 0) && TotalPlantas(plantasJardin) <= 10){
            Debug.Log("Se inicia el proceso de validación...");

            if (IsDensityApropiate(allPlants[planta][4], densidad) // Si la densidad es adecuada [cambiar a futuro]
            && !(allPlants[planta][5] == "Si" && mantencion) // Si la planta no requiere manetnción y se especificó una baja mantención
            && IsFlowApropiate(allPlants[planta][6], flujo) // Si el flujo al que la planta está acostrumbrada es apropiado
            && !banned.Contains(planta) // Si la planta no fue excluida manualmente
            && allPlants[planta][8] == locationManager.LocationData[1] // Si a planta es adecuada a la temperatura de la ubicación del usuario
            && allPlants[planta][9] == locationManager.LocationData[2] // Si la palnta es adecuada al tipo de suelo de la ubicación del usuario
            )
            {
                Debug.Log("La planta "+planta+" cumple con los criterios.");

                if (plantasJardin.ContainsKey(planta)){
                    plantasJardin[planta][0] = (int.Parse(plantasJardin[planta][0]) + 1).ToString();
                    Debug.Log("La planta "+planta+" ya se había escogido previamente. Ahora hay "+plantasJardin[planta][0]+".");
                }
                else {
                    Debug.Log("La planta "+planta+" no había sido escogida anteriormente. Agregando...");

                    List<string> conflictsFound = CheckConflict(allPlants[planta][10].Split(',').ToList(), plantasJardin.Keys.ToList());

                    plantasJardin.Add(planta, 
                    new List<string>{
                        "1",
                        allPlants[planta][1],// Ubicacion
                        allPlants[planta][2],// Precio
                        allPlants[planta][3],// Consumo
                        string.Join(", ", conflictsFound)// Plantas presentes con las que tiene conflicto
                    });
                    
                    foreach (string conflict in conflictsFound)
                    {
                        plantasJardin[conflict][4] += ", "+planta;
                    }
                }
            }
            else Debug.Log($@"La planta {planta} no cumple con los criterios:
            Densidad escogida: {densidad}. Densidad de la planta: {allPlants[planta][4]}
            Mantención: {mantencion}. Mantención de la planta: {allPlants[planta][5]}
            Flujo escogido: {flujo}. Flujo de la planta: {allPlants[planta][6]}
            Temperatura del usuario: {locationManager.LocationData[1]}. Temperatura de la planta: {allPlants[planta][8]}
            Tipo de suelo del usuario: {locationManager.LocationData[2]}. Tiopo de suelo de la planta: {allPlants[planta][9]}
            ");

            planta = WeightedRandPlant(allPlants.Keys.ToList());
            if (planta != null) Debug.Log("Se escogió otra planta aleatoria: "+ planta);
        }
        return plantasJardin;
    }
    string WeightedRandPlant(List<string> allPlantas){
        System.Random rand = new System.Random();
        List<string> weightedPlants = new List<string>();
        Dictionary<string, List<string>> allPlants = new Dictionary<string, List<string>>();

        foreach (KeyValuePair<string, List<string>> p in allPlants)// parseXml.infoPlantas)
        {
            if(p.Value[7] == locationManager.LocationData[0]) weightedPlants.Add(p.Key);
        }
        weightedPlants.AddRange(allPlantas);
        string randWPlant = weightedPlants[rand.Next(weightedPlants.Count)];
        return randWPlant;
    }
    // Corrobora que sea correcta la densidad.
    bool IsDensityApropiate(string plantaValue, string condition){
        if (condition == "Baja" && (plantaValue == "Media" || plantaValue == "Alta")) return false;
        else if (condition == "Media" && plantaValue == "Alta") return false;
        else return true;
    }
    // Corrobora que la planta sea apta para el flujo
    bool IsFlowApropiate(string plantaValue, string condition){
        if (condition == "Alta" && (plantaValue == "Media" || plantaValue == "Baja")) return false;
        else if (condition == "Media" && plantaValue == "Baja") return false;
        else return true;
    }
    // List<string> CheckConflict(conlfictos, seeccionadas):
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


    public void ListWLPlants(){
        Dictionary<string, List<string>> allPlants = new Dictionary<string, List<string>>();
        foreach (var planta in allPlants)// parseXml.infoPlantas)
        {
            PlantaVetableConstructor cons;
            cons = Instantiate(plantaVetableConstructor, whitelist.transform);
            cons.NombrePlanta = planta.Key;
            cons.DescripcionPlanta = planta.Value[0];
            // cons.ImagenPlanta = planta.Value[n];
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

    int TotalPlantas(Dictionary<string, List<string>> dict){
        int total = 0;
        foreach (var par in dict)
        {
            total += int.Parse(par.Value[0]);
        }
        return total;
    }
}
