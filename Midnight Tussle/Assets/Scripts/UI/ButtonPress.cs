using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPress : MonoBehaviour
{
    [SerializeField]
    private Sprite spriteSwitch;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        GetComponent<Image>().sprite = spriteSwitch;
    }
}
