using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(Camera))]
public class EaglePlacePlant : MonoBehaviour
{
    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    public Pose? PlacePlant(EnhancedTouch.Finger finger)
    {
        Debug.Log("Eagle PlacePlant call");
        if(finger.index == 0) {
            Ray ray = camera.ScreenPointToRay(finger.screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 50f))
                return new Pose(hit.point, Quaternion.Euler(Vector3.zero));
        }
        return null;
    }

    public SelectedObject? GetPlant(EnhancedTouch.Finger finger)
    {
        if(finger.index == 0)
        {
            Ray ray = camera.ScreenPointToRay(finger.screenPosition);
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
        return camera.WorldToScreenPoint(plant.HitPoint);
    }

    public void MoveCamera(Vector2 movement)
    {
        movement /= 4;
        Transform offset = camera.transform.parent;
        Vector3 rotation = offset.localRotation.eulerAngles;

        if (rotation.x > 180)
            rotation.x -= 360;
        rotation.x = Mathf.Clamp(rotation.x + movement.y, -45, 0);
        rotation.y = rotation.y + movement.x;
        
        offset.localRotation = Quaternion.Euler(rotation);
    }
}
