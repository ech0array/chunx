using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TouchInputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField]
    private TouchButton _type;

    private bool _down;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        _down = true;
    }
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        _down = false;
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        _down = false;
    }

    public void Update()
    {
        UpdateHold();
    }

    private void UpdateHold()
    {
        TouchInputNode.SetButtonState(_type, _down);
    }
}
