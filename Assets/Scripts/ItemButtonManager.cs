using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]

public class ItemButtonManager : MonoBehaviour
{
    public Toggle toggle;
    public Image warning;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    public void Init(ToggleGroup toggleGroup, string name, string consumption, Sprite image)
    {
        toggle.group = toggleGroup;
        transform.GetChild(0).GetComponent<Text>().text = name;
        transform.GetChild(1).GetComponent<RawImage>().texture = image.texture;
        transform.GetChild(2).GetComponent<Text>().text = $"Consumo [ml/d]: {consumption}";
    }
}
