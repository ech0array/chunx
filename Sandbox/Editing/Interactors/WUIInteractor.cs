using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

sealed internal class WUIInteractor : Interactor
{
    #region Values
    protected override ControllableCamera controllableCamera { get; set; }

    internal enum Mode
    {
        None,
        Selection,
        Input
    }

    [Serializable]
    private class Data
    {
        [SerializeField]
        internal float interactionDistance = 8f;

        internal bool queuedClick = false;
        
        internal Selectable deltaSelectable;
        internal Mode mode;
    }
    [SerializeField]
    private Data _data;

    internal WUI wUI
    {
        get
        {
            if (GameHead.instance.gameState == GameState.Editor)
                return controllableCamera.user.editorWUI;
            if (GameHead.instance.gameState == GameState.Entry)
                return UIHead.instance.entryWUI;
            if (GameHead.instance.isPreviewOrRuntime)
                return controllableCamera.user.runtimeWUI;
            return null;
        }
    }
    #endregion

    #region Unity Framework Entry Functions
    private void LateUpdate()
    {
        if (_data.mode == Mode.None)
            return;

        Interact();
    }
    #endregion

    internal void EnterMode(Mode mode, Module module = null)
    {
        _data.mode = mode;
    }

    internal void ActionClick()
    {
        if (_data.mode == Mode.None)
            return;

        wUI.onClick.Invoke();
        _data.queuedClick = true;
    }
    internal void ActionExit()
    {
        if (_data.mode == Mode.None)
            return;

        controllableCamera.user.editorWUI.HideTip();
        if (GameHead.instance.gameState == GameState.Entry)
        {
            UIHead.instance.entryWUI.UnstackCurrent();
        }
        else if (GameHead.instance.gameState == GameState.Editor)
        {
            bool allMenusClosed = controllableCamera.user.editorWUI.UnstackCurrent();
            if (allMenusClosed)
                controllableCamera.EnterMode(ControllableCamera.Mode.Module, null);
        }
        else if (GameHead.instance.isPreviewOrRuntime)
        {
            bool allMenusClosed = controllableCamera.user.runtimeWUI.UnstackCurrent();
            if (allMenusClosed)
                controllableCamera.EnterMode(ControllableCamera.Mode.Runtime, null);
        }
    }

    internal override void Register(ControllableCamera controllableCamera)
    {
        this.controllableCamera = controllableCamera;
    }


    private void Interact()
    {
        controllableCamera.user.hUD.SetReticlePassive();
        controllableCamera.user.editorWUI.HideTip();

        Ray ray = new Ray(base.transform.position, base.transform.forward);
        wUI.Traverse(ray);

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = GetPointerPosition();

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            RaycastResult raycastResult = raycastResults[0];
            if (raycastResults.Count > 1)
            {
                for (int i = 1; i < raycastResults.Count; i++)
                {
                    if (raycastResults[i].index < raycastResult.index)
                        raycastResult = raycastResults[i];
                }
            }


            Selectable selectable = raycastResult.gameObject.GetComponentInParent<Selectable>();
            if (selectable != null)
            {
                if (_data.queuedClick)
                    Click(raycastResult, selectable, pointerEventData);
                else if (selectable == _data.deltaSelectable)
                    Stay(raycastResult, selectable, pointerEventData);
                else
                    Enter(selectable, pointerEventData);
            }
            else
            {
                Exit(_data.deltaSelectable);
            }
        }
        else
        {
            Exit(_data.deltaSelectable);
        }
        _data.queuedClick = false;
    }
    private void Enter(Selectable selectable, PointerEventData pointerEventData)
    {
        Exit(_data.deltaSelectable);

        _data.deltaSelectable = selectable;

        selectable.OnPointerEnter(pointerEventData);
    }
    private void Exit(Selectable selectable)
    {
        if (selectable == null)
            return;
        selectable.OnPointerExit(new PointerEventData(EventSystem.current));
        _data.deltaSelectable = null;
    }
    private void Stay(RaycastResult raycastResult, Selectable selectable, PointerEventData pointerEventData)
    {
        _data.deltaSelectable = selectable;

        controllableCamera.user.hUD.SetReticleHighlight(selectable.colors.normalColor);

        WUITipable wUITipable = selectable.gameObject.GetComponentInParent<WUITipable>();
        if (wUITipable != null)
            controllableCamera.user.editorWUI.ShowTip(wUITipable);

        Type selectableType = selectable.GetType();
        if (selectableType.BaseType == typeof(WUIChartable))
        {
            WUIChartable wUIChartable = ((WUIChartable)selectable);
            wUIChartable.ChartPreview((raycastResult.distance * base.transform.forward) + base.transform.position);
        }
    }
    private void Click(RaycastResult raycastResult, Selectable selectable, PointerEventData pointerEventData)
    {
        _data.queuedClick = false;
        _data.deltaSelectable = selectable;

        if (selectable is Button)
        {
            Button button = (Button)selectable;
            if (button.interactable)
                button.onClick.Invoke();
        }
        else if (selectable is TMP_InputField)
        {
            selectable.GetComponentInParent<WUI>().Edit((TMP_InputField)selectable);
        }
        else if (selectable is WUIChartable)
        {
            ((WUIChartable)selectable).Chart((raycastResult.distance * base.transform.forward) + base.transform.position);
        }
        else
        {
            selectable.OnSelect(pointerEventData);
        }
    }

    private Vector2 GetPointerPosition()
    {
        Vector2 size = new Vector2(Screen.width, Screen.height);
        Vector2 position = size / 2f;
        position += position * new Vector2(controllableCamera.camera.rect.x, controllableCamera.camera.rect.y);
        return position;
    }
    internal float GetDistanceToWUI(Vector3 value, float offset = 0f)
    {
        Ray ray = new Ray(controllableCamera.transform.position, value);
        (bool hit, float distance) = wUI.CastPlane(ray, offset);
        return distance;
    }
}