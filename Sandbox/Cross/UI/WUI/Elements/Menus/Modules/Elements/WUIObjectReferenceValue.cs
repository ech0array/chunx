using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

sealed internal class WUIObjectReferenceValue : WUIValue
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI objectNameLabel;
    }
    [SerializeField]
    private Components _components;

    internal override void Bind(SandboxValue sandboxValue)
    {
        base.valueReference = sandboxValue;
        int id = ((int)sandboxValue.get());
        if (id != -1)
        {
            SandboxObjectData sandboxObjectData = ModuleHead.GetSandboxObjectDataById(GameHead.instance.universeData, id);
            _components.objectNameLabel.text = sandboxObjectData.name;
        }
        else
        {
            _components.objectNameLabel.text = "EMPTY";
        }
        wUITipable.Populate(sandboxValue.id, sandboxValue.description);
    }


    internal static bool IsCompatableTo(Type type)
    {
        return type == typeof(WUIObjectReferenceValue);
    }

    internal void Set(int id)
    {
        SandboxObjectData sandboxObjectData = ModuleHead.GetSandboxObjectDataById(GameHead.instance.universeData, id);
        _components.objectNameLabel.text = sandboxObjectData.name;
        base.valueReference.set(id);
    }

    public void UE_Click()
    {
        wUI.Edit(this);
    }
}
