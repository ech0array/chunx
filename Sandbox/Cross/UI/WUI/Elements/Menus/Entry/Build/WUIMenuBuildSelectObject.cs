using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class WUIMenuBuildSelectObject : WUIMenu
{

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal GameObject moveToButton;
    }
    [SerializeField]
    private Components _components;

    private class Data
    {
        internal Action onEditCallback;
        internal Action onDeleteCallback;
        internal Action onDuplicateCallback;
        internal Action<string> onRenameCallback;
        internal Action onMoveToCallback;
        internal string name;
    }
    private Data _data = new Data();

    internal override void Unstack()
    {
        base.Unstack();
    }
    internal void Inspect(Action onEditCallback, string name, Action<string> onRenameCallback, Action onDeleteCallback, Action onDuplicateCallback, Action onMoveToCallback = null)
    {
        _data.name = name;
        _data.onEditCallback = onEditCallback;
        _data.onRenameCallback = onRenameCallback;
        _data.onDeleteCallback = onDeleteCallback;
        _data.onMoveToCallback = onMoveToCallback;
        _data.onDuplicateCallback = onDuplicateCallback;
        _components.moveToButton.SetActive(onMoveToCallback != null);
        Stack();
    }

    public void UE_Rename()
    {
        wUI.Edit(_data.name, _data.onRenameCallback);
    }

    public void UE_Edit()
    {
        Unstack();

        _data.onEditCallback.Invoke();
    }

    public void UE_Delete()
    {
        wUI.Confirm("WARNING! YOU ARE ABOUT TO DELETE AN OBJECT. YOU CANNOT UNDO THIS. PLEASE RECONSIDER BEFORE CONTINUING.", () =>
        {
            Unstack();
            _data.onDeleteCallback.Invoke();
        });
    }

    public void UE_Move()
    {
        Unstack();
        _data.onMoveToCallback.Invoke();
    }

    public void UE_Duplicate()
    {
        Unstack();
        _data.onDuplicateCallback.Invoke();
    }
}
