using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;


public class ARInteractionsManager : MonoBehaviour
{
    [SerializeField] private Camera aRCamera;
    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField] private GameObject planta; // -> Debería obtener una instancia del Item Bonsai

    // private GameObject aRPointer;
    // private GameObject item3DModel;
    // private GameObject itemSelected;

    // private bool isInitialPosition;
    // private bool isOverUI;
    // private bool isOver3DModel;

    // private Vector2 initialTouchPos;

    // Asigna de una forma rebuscada el modelo3D y la posición a la planta que spawnea
    // public GameObject Item3DModel
    // {
    //     set{
    //         item3DModel = value;
    //         item3DModel.transform.position = aRPointer.transform.position;
    //         item3DModel.transform.parent = aRPointer.transform;// -> Esto permite desplazar el modelo3D
    //         // isInitialPosition = true;// -> Esto es si lo hiciera spawnear con un botón. En el caso de spawnearlo con un touch, no es necesario.
    //     }
    // }
    // Start is called before the first frame update
    void Start()
    {
        // aRPointer = transform.GetChild(0).gameObject;// -> Creo que es necesario para chantar el modelo3D, creo
        Debug.Log("ARInteractionsManager enabled.");
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
        // GameManager.instance.OnMainMenu += SetItemPosition;// -> Esto fija al objeto
    }

    // Update is called once per frame
    // void Update()
    // {
    //     // Misma explicación que en la propiedad de Item3DModel
    //     // if(isInitialPosition){
    //     //     Vector2 middlePointScreen = new Vector2(Screen.width/2, Screen.height/2);
    //     //     aRRaycastManager.Raycast(middlePointScreen, hits, TrackableType.Planes);
    //     //     if(hits.Count>0){
    //     //         transform.position = hits[0].pose.position;
    //     //         transform.rotation = hits[0].pose.rotation;
    //     //         aRPointer.SetActive(true);
    //     //         isInitialPosition = false;
    //     //     }
    //     // }

    //     //Verificar si ha ocurrido un input
    //     if(Input.touchCount > 0){
    //         Touch touchOne = Input.GetTouch(0);

    //         // Si apreto la pantalla -> ¿Donde?
    //         if(touchOne.phase == TouchPhase.Began){
    //             var touchPosition = touchOne.position;
    //             isOverUI = isTapOverUI(touchPosition);
    //             isOver3DModel = isTapOver3DModel(touchPosition);
    //         }

    //         // Crea una instancia del modelo3D de la planta (extrae el valor de el SuscriptableObject)
    //         if(!isOver3DModel && !isOverUI && item3DModel == null){// Podría poner la condiciónd eque el dedo tenga que ser retirado
    //             Item3DModel = Instantiate(planta);  
    //         }
            
    //         //Si apreto un modelo 3D en la pantalla, paso a modo edición del mismo
    //         if (isOver3DModel && item3DModel == null && !isOverUI){
    //             // GameManager.instance.ARPosition(); -> Ya no necesito pasar a ARPosition();
    //             item3DModel = itemSelected;// -> COn la condición del if() ya me aseguro que hay un itemSelected
    //             itemSelected = null;
    //             // aRPointer.SetActive(true);
    //             // transform.position = item3DModel.transform.position;
    //             // item3DModel.transform.parent = aRPointer.transform;
    //             DeleteItem(); // -> Borro el modelo3D
    //         }

    //         // Si arrastro mi dedo sobre una superficie que sea detectable
    //         // if(touchOne.phase == TouchPhase.Moved){
    //         //     if(aRRaycastManager.Raycast(touchOne.position, hits, TrackableType.Planes)){
    //         //         Pose hitPose = hits[0].pose;
    //         //         if(!isOverUI && isOver3DModel){
    //         //             transform.position = hitPose.position;
    //         //         }
    //         //     }
    //         // }

    //         // Si arrastro 2 dedos, roto el objeto
    //         // if (Input.touchCount == 2){
    //         //     Touch touchTwo = Input.GetTouch(1);
    //         //     if (touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began){
    //         //         initialTouchPos = touchTwo.position - touchOne.position;
    //         //     }
    //         //     if (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved){
    //         //         Vector2 currentTouchPos = touchTwo.position - touchOne.position;
    //         //         float angle = Vector2.SignedAngle(initialTouchPos, currentTouchPos);
    //         //         item3DModel.transform.rotation = Quaternion.Euler(0, item3DModel.transform.eulerAngles.y - angle, 0);
    //         //         initialTouchPos = currentTouchPos;
    //         //     }
    //         // }
    //     }
    // }

    // private bool isTapOver3DModel(Vector2 touchPosition){
    //     Ray ray = aRCamera.ScreenPointToRay(touchPosition);
    //     if(Physics.Raycast(ray, out RaycastHit hit3DModel)){
    //         if (hit3DModel.collider.CompareTag("Item")){
    //             itemSelected = hit3DModel.transform.gameObject;
    //             return true;
    //         }
    //     }
    //     return false;
    // }

    // //Verificar si el touch fué sobre la UI
    // private bool isTapOverUI(Vector2 touchPosition){
    //     PointerEventData eventData = new PointerEventData(EventSystem.current);
    //     eventData.position = new Vector2(touchPosition.x, touchPosition.y);

    //     List<RaycastResult> result = new List<RaycastResult>();
    //     EventSystem.current.RaycastAll(eventData, result);

    //     return result.Count>0;
    // }

    // // Esta wea básicamente desengancha el modelo3D del pointer para que quede estático
    // private void SetItemPosition(){
    //     if (item3DModel != null){
    //         item3DModel.transform.parent = null;
    //         aRPointer.SetActive(false);
    //         item3DModel = null;        
    //     }
    // }

    // public void DeleteItem(){
    //     Destroy(item3DModel);
    //     aRPointer.SetActive(false);
    //     // GameManager.instance.MainMenu();    
    // }

    private void OnEnable(){
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable(){
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(EnhancedTouch.Finger finger){
        if(finger.index != 0) return;
        if(aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.Planes)){
            foreach (ARRaycastHit hit in hits){
                Pose pose = hit.pose;
                GameObject obj = Instantiate(planta, pose.position, pose.rotation);
            }
        }
    }
}
