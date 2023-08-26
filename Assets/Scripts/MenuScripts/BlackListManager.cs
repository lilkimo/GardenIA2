using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackListManager : MonoBehaviour
{
    [SerializeField] private GameObject whiteList;
    [SerializeField] private GameObject blackList;
    [SerializeField] private PlantaVetableConstructor constructor;

    public void BanPlant(){
        // Itero sobre cada planta en la whitelist
        foreach(Transform child in whiteList.transform)
        {
            if (child.GetComponent<Toggle>().isOn) {
                string nombre = child.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
                string descripcion = child.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;

                Debug.Log("Planta baneada: "+nombre);

                child.GetComponent<Toggle>().isOn = false;
                child.gameObject.SetActive(false);

                PlantaVetableConstructor cons;
                cons = Instantiate(constructor, blackList.transform);
                cons.NombrePlanta = nombre;
                cons.DescripcionPlanta = descripcion;
            }
        }

    }

    public void UnbanPlant(){
        foreach(Transform child in blackList.transform){
            if(!child.GetComponent<Toggle>().isOn) continue;
            
            string nombre = child.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            foreach(Transform plant in whiteList.transform){
                if(plant.GetChild(0).GetComponent<TextMeshProUGUI>().text == nombre){
                    plant.gameObject.SetActive(true);
                    Destroy(child.gameObject);
                }
            }
        }
    }
}