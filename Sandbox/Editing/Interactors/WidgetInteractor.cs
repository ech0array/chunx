using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed internal class WidgetInteractor : Interactor
{
    #region Values
    [Serializable]
    private class Data
    {
        [SerializeField]
        internal LayerMask castLayerMask;

        internal Widget prospectWidget;
        internal Widget currentWidget;

        internal bool actionDown;
        internal Module module;
        internal Mode mode;
    }
    [SerializeField]
    private Data _data;

    internal enum Mode
    {
        None,
        Interact
    }

    protected override ControllableCamera controllableCamera { get; set; }
    #endregion

    #region Unity Framework Entry Functions
    private void Update()
    {
        if (_data.mode == Mode.None)
            return;

        UpdateInteraction();
    } 
    #endregion

    internal void EnterMode(Mode mode, Module module)
    {
        _data.mode = mode;
        _data.module = module;
    }

    internal override void Register(ControllableCamera controllableCamera)
    {
        this.controllableCamera = controllableCamera;
    }


    internal void ActionDown()
    {
        if (_data.mode == Mode.None)
            return;
        _data.actionDown = true;
    }
    internal void ActionExit()
    {
        if (_data.mode == Mode.None)
            return;
        controllableCamera.EnterMode(ControllableCamera.Mode.Module, _data.module, _data.module.transform.position);
        WidgetHead.instance.HideAll();

    }

    private void UpdateInteraction()
    {
        _data.prospectWidget?.Exit();
        _data.prospectWidget = null;

        Ray ray = new Ray(base.transform.position, base.transform.forward);
        Vector3 hitPoint = Vector3.zero;
        if (_data.currentWidget == null)
        {
            RaycastHit raycastHit;
            bool hit = Physics.Raycast(ray, out raycastHit, Mathf.Infinity, _data.castLayerMask);
            if (!hit)
            {
                _data.actionDown = false;
                return;
            }

            hitPoint = raycastHit.point;
            GameObject gameObject = raycastHit.collider.gameObject;
            _data.prospectWidget = gameObject.GetComponent<Widget>();
        }

        if (_data.actionDown)
        {
            if (_data.currentWidget == null)
            {
                _data.currentWidget = _data.prospectWidget;
                _data.prospectWidget = null;
            }
            bool? wasSelected = _data.currentWidget?.Down(ray, hitPoint);
            if (wasSelected == false)
                _data.currentWidget = null;
        }
        else
        {
            _data.prospectWidget?.Hover();
        }

        _data.currentWidget?.TryUpdate(ray);

        _data.actionDown = false;
    }
}