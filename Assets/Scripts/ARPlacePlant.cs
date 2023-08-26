using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class ARPlacePlant : MonoBehaviour
{
    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    [SerializeField]
    private Camera aRCamera;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
    }

    public Pose? PlacePlant(EnhancedTouch.Finger finger)
    {
        Debug.Log("AR PlacePlant call");
        if(finger.index == 0)
            if(aRRaycastManager.Raycast(finger.screenPosition, hits, TrackableType.Planes))
                return hits[0].pose;
        return null;
    }

    public SelectedObject? GetPlant(EnhancedTouch.Finger finger)
    {
        if(finger.index == 0)
        {
            Ray ray = aRCamera.ScreenPointToRay(finger.screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 50f))
            {
                Transform obj = hit.transform;
                if (obj.CompareTag("Plant"))
                    return new SelectedObject(obj, hit.point);
            }
        }
        return null;
    }

    public Vector2 GetPlantScreenPosition(SelectedObject plant)
    {
        return aRCamera.WorldToScreenPoint(plant.HitPoint);
    }
}
