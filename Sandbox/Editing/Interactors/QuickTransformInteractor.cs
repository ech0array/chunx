using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
internal sealed class QuickTransformInteractor : Interactor
{

    protected override ControllableCamera controllableCamera { get; set; }
    internal enum Mode
    {
        None,
        QuickMove,
        QuickRotate,
        QuickScale,
        MimicPosition,
        MimicRotation,
        MimicScale,
        MimicAll
    }
    [Serializable]
    private class Data
    {
        [SerializeField]
        internal float interactionDistance = 8f;

        internal bool queuedClick = false;

        internal Mode mode;
        internal Module module;

        internal Vector3 startOffset;
        internal float startDistanceToCursor;
        internal float startDistanceFromCameraToEditable;
        internal Vector3 startDirection;

        internal TransformData editableStartTransformData;
    }
    [SerializeField]
    private Data _data;

    #region Unity Framework Entry Functions
    private void Update()
    {
        if (_data.mode == Mode.None)
            return;

        if (_data.mode == Mode.QuickMove)
            UpdateQuickMove();
        if (_data.mode == Mode.QuickRotate)
            UpdateQuickRotate();
        if (_data.mode == Mode.QuickScale)
            UpdateQuickScale();

        _data.module.OnTransformed();
    }
    #endregion

    internal void EnterMode(Mode mode, Vector3 cursorPosition, Module module)
    {
        _data.mode = mode;
        _data.module = module;
        WidgetHead.instance.ShowForwardOf(null, false);
        if (mode == Mode.None)
            return;
        if (mode == Mode.QuickRotate)
            WidgetHead.instance.ShowForwardOf(module, true);

        _data.startDirection = (module.transform.position - base.transform.position).normalized;
        _data.startDistanceToCursor = Vector3.Distance(cursorPosition, base.transform.position);
        _data.startDistanceFromCameraToEditable = Vector3.Distance(base.transform.position, module.transform.position);
        _data.editableStartTransformData = new TransformData(module.transform);
        _data.startOffset = module.transform.position - cursorPosition;
    }

    internal override void Register(ControllableCamera controllableCamera)
    {
        this.controllableCamera = controllableCamera;
    }

    internal void ActionClick()
    {
        if (_data.mode == Mode.MimicPosition)
            MimicPosition();
        if (_data.mode == Mode.MimicRotation)
            MimicRotation();
        if (_data.mode == Mode.MimicScale)
            MimicScale();
        if (_data.mode == Mode.MimicAll)
            MimicAll();
    }

    internal void ActionExit()
    {
        if (_data.mode == Mode.None)
            return;

        if (_data.mode == Mode.None)
            return;
        controllableCamera.EnterMode(ControllableCamera.Mode.Module);
    }

    private void UpdateQuickMove()
    {
        _data.module.transform.position = base.transform.position + (base.transform.forward * _data.startDistanceToCursor) + _data.startOffset;

        Vector3 position = _data.module.transform.position;
        position.x = (float)System.Math.Round((double)position.x, 1);
        position.y = (float)System.Math.Round((double)position.y, 1);
        position.z = (float)System.Math.Round((double)position.z, 1);
        _data.module.transform.position = position;
    }
    private void UpdateQuickRotate()
    {
        Vector3 target = base.transform.position + (base.transform.forward * _data.interactionDistance);

        _data.module.transform.LookAt(target);
        _data.module.transform.eulerAngles = SnapRotation(_data.module.transform.eulerAngles, 5f);
    }
    private void UpdateQuickScale()
    {
        float distance = Vector3.Distance(_data.module.transform.position, base.transform.position);
        float scale = distance - _data.startDistanceFromCameraToEditable;
        _data.module.transform.localScale = _data.editableStartTransformData.scale + (Vector3.one * scale);
    }

    private Vector3 SnapRotation(Vector3 vector3, float increments)
    {
        vector3.x -= vector3.x % increments;
        vector3.y -= vector3.y % increments;
        vector3.z -= vector3.z % increments;
        return vector3;
    }


    private void MimicPosition()
    {
        Module module = CastForModule();
        if (module != null)
        {
            _data.module.transform.position = module.transform.position;
            _data.module.data.position = module.transform.position;
            _data.module.OnTransformed();
            controllableCamera.EnterMode(ControllableCamera.Mode.Module);
        }
    }
    private void MimicRotation()
    {
        Module module = CastForModule();
        if (module != null)
        {
            _data.module.transform.rotation = module.transform.rotation;
            _data.module.data.rotation = module.transform.eulerAngles;
            _data.module.OnTransformed();
            controllableCamera.EnterMode(ControllableCamera.Mode.Module);
        }
    }

    private void MimicScale()
    {
        Module module = CastForModule();
        if (module != null)
        {
            _data.module.transform.localScale = module.transform.localScale;
            _data.module.data.scale = module.transform.localScale;
            _data.module.OnTransformed();
            controllableCamera.EnterMode(ControllableCamera.Mode.Module);
        }
    }

    private void MimicAll()
    {
        Module module = CastForModule();
        if (module != null)
        {
            _data.module.transform.position = module.transform.position;
            _data.module.data.position = module.transform.position;
            _data.module.transform.rotation = module.transform.rotation;
            _data.module.data.rotation = module.transform.eulerAngles;
            _data.module.transform.localScale = module.transform.localScale;
            _data.module.data.scale = module.transform.localScale;
            _data.module.OnTransformed();
            controllableCamera.EnterMode(ControllableCamera.Mode.Module);
        }
    }

    private Module CastForModule()
    {
        Ray ray = new Ray(base.transform.position, base.transform.forward);
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(ray, out raycastHit, _data.interactionDistance);
        if(hit)
        {
            Module module = raycastHit.collider.gameObject.GetComponent<Module>();
            if (module != null)
                return module;
        }
        return null;
    }
}