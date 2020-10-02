using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class ConnectionInteractor : Interactor
{
    #region Values
    protected override ControllableCamera controllableCamera { get; set; }

    [Serializable]
    private class Components
    {
        internal Module module;
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Data
    {
        internal Mode mode;
        internal float spoofDistance;
    }
    [SerializeField]
    private Data _data = new Data(); 
    #endregion

    internal enum Mode
    {
        None,
        Weld,
        Unweld,
        Link,
        Unlink
    }

    internal override void Register(ControllableCamera controllableCamera)
    {
        this.controllableCamera = controllableCamera;
    }


    private void Update()
    {
        if (_data.mode == Mode.None)
            return;
        UpdateSpoofConnection();
    }

    private void UpdateSpoofConnection()
    {
        if (_data.mode == Mode.Link || _data.mode == Mode.Weld)
            ModuleHead.instance.ShowSpoofConnection(_components.module, controllableCamera.transform.position + (controllableCamera.transform.forward * _data.spoofDistance));
    }

    internal void ActionClick()
    {
        switch (_data.mode)
        {
            case Mode.None:
                return;
            case Mode.Weld:
                TryWeld();
                return;
            case Mode.Unweld:
                TryUnweld();
                return;
            case Mode.Link:
                TryLink();
                return;
            case Mode.Unlink:
                TryUnlink();
                return;
        }
    }

    internal void ActionExit()
    {
        if (_data.mode == Mode.None)
            return;
        controllableCamera.EnterMode(ControllableCamera.Mode.Module);
    }

    internal void EnterMode(Mode mode, Vector3 cursorPosition, Module module)
    {
        ModuleHead.instance.HideSpoofConnection();
        _data.spoofDistance = Vector3.Distance(cursorPosition, controllableCamera.transform.position);
        _data.mode = mode;
        _components.module = module;
    }

    private void TryWeld()
    {
        RaycastHit raycastHit;
        Ray ray = new Ray(base.transform.position, base.transform.forward);
        bool hit = Physics.Raycast(ray, out raycastHit, Mathf.Infinity);
        if (hit)
        {
            Module module = raycastHit.collider.gameObject.GetComponent<Module>();
            if (module == null)
                return;
            if (module.passive)
                return;
            if (module == _components.module)
                return;
            Type type = module.GetType();
            if (type == typeof(ModuleLogicLink))
                return;

            Module parentModules = module.data.parentId == -1 ? null : EditorHead.instance.editable.GetModuleByIdRecursive(module.data.parentId);
            if (parentModules != null && _components.module.IsConnectedTo(parentModules))
                return;

            _components.module.SetParent(module);
            Connection connection = EditorHead.instance.editable.Connect(_components.module, module, ConnectionType.Weld);
            controllableCamera.EnterMode(ControllableCamera.Mode.Module);
        }
    }
    private void TryUnweld()
    {
        RaycastHit raycastHit;
        Ray ray = new Ray(base.transform.position, base.transform.forward);
        bool hit = Physics.Raycast(ray, out raycastHit, Mathf.Infinity);
        if (hit)
        {
            Module module = raycastHit.collider.gameObject.GetComponent<Module>();
            if (module == null)
                return;
            if (module.passive)
                return;
            if (module == _components.module)
                return;

            if (_components.module.IsConnectedTo(module))
            {
                _components.module.SetParent(null);
                _components.module.BreakConnectionTo(module);
                controllableCamera.EnterMode(ControllableCamera.Mode.Module);
            }
        }
    }

    private void TryLink()
    {
        RaycastHit raycastHit;
        Ray ray = new Ray(base.transform.position, base.transform.forward);
        bool hit = Physics.Raycast(ray, out raycastHit, Mathf.Infinity);
        if (hit)
        {
            Module module = raycastHit.collider.gameObject.GetComponent<Module>();
            if (module == null)
                return;
            if (module.passive)
                return;
            if (module == _components.module)
                return;

            if (module.IsConnectedTo(_components.module))
                return;
            Connection connection = EditorHead.instance.editable.Connect(_components.module, module, ConnectionType.Link);
            controllableCamera.EnterMode(ControllableCamera.Mode.Module);
        }
    }
    private void TryUnlink()
    {
        RaycastHit raycastHit;
        Ray ray = new Ray(base.transform.position, base.transform.forward);
        bool hit = Physics.Raycast(ray, out raycastHit, Mathf.Infinity);
        if (hit)
        {
            Module module = raycastHit.collider.gameObject.GetComponent<Module>();
            if (module == null)
                return;
            if (module.passive)
                return;
            if (module == _components.module)
                return;

            if (_components.module.IsConnectedTo(module))
            {
                _components.module.BreakConnectionTo(module);
                controllableCamera.EnterMode(ControllableCamera.Mode.Module);
            }
        }
    }
}
