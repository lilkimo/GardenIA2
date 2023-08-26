using UnityEngine;

public class DeleteJardin : MonoBehaviour
{
    [SerializeField] private GameObject jardinGenerado;

    public void BorrarJardin(){
        foreach(Transform child in jardinGenerado.transform)
        {
           Destroy(child.gameObject);
        }
    }
}
