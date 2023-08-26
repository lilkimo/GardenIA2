using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera aRCamera;
    [SerializeField]
    private Camera eagleCamera;
    [SerializeField]
    public bool aRMode;
    
    [SerializeField]
    public VirtualGardenManager virtualGarden;
    
    void Start()
    {
        SwitchCamera(aRMode);
    }

    void SwitchCamera(bool isAREnabled)
    {
        aRMode = isAREnabled;

        virtualGarden.terrain.SetActive(!aRMode);

        aRCamera.enabled = aRMode;
        eagleCamera.enabled = !aRMode;
    }

    public void SwitchCamera()
    {
        SwitchCamera(!aRMode);
    }
}
