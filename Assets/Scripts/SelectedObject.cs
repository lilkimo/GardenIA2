using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SelectedObject
{
    public GameObject Object;
    public Vector3 HitPoint;
    public Vector3 OriginalScale;

    public SelectedObject(Transform selectedObject, Vector3 hitPoint)
    {
        Object = selectedObject.gameObject;
        HitPoint = hitPoint;
        OriginalScale = selectedObject.localScale;
    }
}