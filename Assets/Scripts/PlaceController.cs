using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using System;

[RequireComponent(typeof(CameraController))]
// Esta weá debería llamarse InputManager
public class PlaceController : MonoBehaviour
{   
    [SerializeField]
    private ARPlacePlant aRPlacePlant;
    [SerializeField]
    private EaglePlacePlant eaglePlacePlant;
    [SerializeField]
    private VirtualGardenManager vGM;
    
    private GameObject plant;

    private CameraController cameraController;

    private void Awake()
    {
        cameraController = GetComponent<CameraController>();

        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
    }

    public void EnablePlaceMode()
    {
        EnhancedTouch.Touch.onFingerDown += PlacePlant;
        EnhancedTouch.Touch.onFingerMove -= onFingerMove;
        
        EnhancedTouch.Touch.onFingerDown -= SelectPlant;
        EnhancedTouch.Touch.onFingerUp -= DeselectOrDeletePlant;
        
        EnhancedTouch.Touch.onFingerMove -= itWasNotATap;
        // EnhancedTouch.Touch -= RotatePlant;
        // EnhancedTouch.Touch -= ScalePlant;
    }

    public void DisablePlaceMode()
    {
        EnhancedTouch.Touch.onFingerDown -= PlacePlant;
        EnhancedTouch.Touch.onFingerMove += onFingerMove;

        EnhancedTouch.Touch.onFingerDown += SelectPlant;
        EnhancedTouch.Touch.onFingerUp += DeselectOrDeletePlant;
        
        EnhancedTouch.Touch.onFingerMove += itWasNotATap;
        // EnhancedTouch.Touch += RotatePlant;
        // EnhancedTouch.Touch += ScalePlant;
    }

    public void SetPlant(GameObject desiredPlant)
    {
        Debug.Log($"Planta cambiada a {desiredPlant}");
        Debug.Log($"Consumo de la planta: {desiredPlant.transform.GetComponent<ModelData>().Consumo}");
        plant = desiredPlant;
    }

    private SelectedObject? selectedPlant = null;
    private bool isTap;
    public void SelectPlant(EnhancedTouch.Finger finger)
    {
        isTap = true;

        if (cameraController.aRMode)
            selectedPlant = aRPlacePlant.GetPlant(finger);
        else
            selectedPlant = eaglePlacePlant.GetPlant(finger);

        if (selectedPlant.HasValue)
            Debug.Log($"Selected plant: {selectedPlant}");
    }

    public void itWasNotATap(EnhancedTouch.Finger _) => isTap = false;

    public void DeselectOrDeletePlant(EnhancedTouch.Finger _)
    {
        sameTouch = 0;
        
        if (selectedPlant.HasValue)
        {
            Debug.Log($"Deselected plant: {selectedPlant}");
            if (isTap)
                Destroy(selectedPlant.Value.Object);
                vGM.removePlant(plant.transform.GetComponent<ModelData>().Consumo);
        }

        selectedPlant = null;
    }

    public void PlacePlant(EnhancedTouch.Finger finger)
    {
        Pose? plantPose;
        if (cameraController.aRMode)
            plantPose = aRPlacePlant.PlacePlant(finger);
        else
            plantPose = eaglePlacePlant.PlacePlant(finger);
        
        if (plantPose.HasValue)
            Instantiate(plant, plantPose.Value.position, plantPose.Value.rotation, cameraController.virtualGarden.transform);
            vGM.addPlant(plant.transform.GetComponent<ModelData>().Consumo);
    }

    private void onFingerMove(EnhancedTouch.Finger finger)
    {
        if (!selectedPlant.HasValue)
        {
            if (!cameraController.aRMode)
                eaglePlacePlant.MoveCamera(finger.screenPosition - finger.touchHistory[1].screenPosition);
        }
        else
            ScaleRotatePlant(finger);
    }

    int sameTouch = 0;
    private void ScaleRotatePlant(EnhancedTouch.Finger finger)
    {
        sameTouch = Math.Clamp(sameTouch + 1, 0, 64);
        if (!selectedPlant.HasValue)
            return;

        Vector2 deltaScreenPosition = finger.screenPosition - finger.touchHistory[new int[] {finger.touchHistory.Count, sameTouch, 3}.Min()].screenPosition;
        
        // Vector2 plantScreenPosition;
        // if (cameraController.aRMode)
        //     plantScreenPosition = aRPlacePlant.GetPlantScreenPosition(selectedPlant.Value);
        // else
        //     plantScreenPosition = eaglePlacePlant.GetPlantScreenPosition(selectedPlant.Value);
        // Vector2 relativePosition = finger.screenPosition - plantScreenPosition;
        // float angle = Mathf.Abs(Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg);
        // if (angle > 90)
        //     angle = 180 - angle;
        // Debug.Log($"{angle} {finger.screenPosition} {plantScreenPosition}");
        
        float angle = Mathf.Abs(Mathf.Atan2(deltaScreenPosition.y, deltaScreenPosition.x) * Mathf.Rad2Deg);
        if (angle > 90)
            angle = 180 - angle;
        
        //Debug.Log($"{angle} {deltaScreenPosition}");
        
        if (angle <= 10)
            RotatePlant(deltaScreenPosition.x);
        else if (angle >= 80)
            ScalePlant(deltaScreenPosition.y);
    }

    private void ScalePlant(float magnitude)
    {
        // Aquí vvvv vamos a tener que dividir deltaScreenPosition por un múltiplo de la pantalla para
        // que en todos los dispositivos funcione igual.
        Vector3 deltaVector = Vector3.one*Mathf.Clamp(magnitude/100, -1, 1);
        
        // Si la selectedPlant no parte con escala (1, 1, 1) esta weá va a explotar a la mierda.
        // Para arreglarlo hay que guardar la escala original de <selectedPlant> y en <deltaVector>
        // en vez de multiplicar por Vector3.one vamos a tener que multiplicar por la escala original
        // y seguramente clampear el resultado por valores más chicos como +-.1f. Y luego hacer la pedazo
        // de condición corte (selectedPlant.localScale + deltaVector).x <= originalScale.x &&
        // (selectedPlant.localScale + deltaVector).y <= originalScale.y && ...
        Vector3 newScale = selectedPlant.Value.Object.transform.localScale + deltaVector;
        for (int i = 0; i < 3; i++)
            newScale[i] = Mathf.Clamp(newScale[i], 1, 4);

        selectedPlant.Value.Object.transform.localScale = newScale;
    }


    private void RotatePlant(float magnitude)
    {
        Debug.Log("Rotate");
        // Aquí vvvv vamos a tener que dividir <magnitude> por un múltiplo de la pantalla para
        // que en todos los dispositivos funcione igual.
        selectedPlant.Value.Object.transform.localRotation = Quaternion.Euler(selectedPlant.Value.Object.transform.localRotation.eulerAngles + new Vector3(0, magnitude/2, 0));
    }
}
