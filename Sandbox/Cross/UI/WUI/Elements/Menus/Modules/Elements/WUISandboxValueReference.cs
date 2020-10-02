using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal sealed class WUISandboxValueReference : WUIValue
{

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI objectNameLabel;
    }
    [SerializeField]
    private Components _components;


    internal SandboxValueReference sandboxValueReference;
    internal Module rootModule;
    internal List<Type> types = new List<Type>();
    internal override void Bind(SandboxValue sandboxValue)
    {
        base.valueReference = sandboxValue;
        // Defines the value on the end connection
        sandboxValueReference = (SandboxValueReference)sandboxValue.get();
        wUITipable.Populate(sandboxValue.id, sandboxValue.description);
        rootModule = (Module)sandboxValue.meta[0];
        types = (List<Type>)sandboxValue.meta[1];

        if (sandboxValueReference.valueID == null)
            _components.objectNameLabel.text = rootModule.connections.Count > 0 ? "EMPTY" : "THERE ARE NO CONNECTIONS";
        else
        {
            SandboxValue connectionSandboxValue = null;
            foreach (Connection connection in rootModule.connections)
            {
                Module connectedModule = connection.GetOtherTo(rootModule);
                if(connectedModule.data.id == sandboxValueReference.moduleID)
                {
                    connectionSandboxValue = connectedModule.sandboxValuesById[sandboxValueReference.valueID];
                    break;
                }
            }
            if(sandboxValueReference.moduleID != 0 && connectionSandboxValue == null)
            {
                Debug.LogError($"Stale reference found for deleted module [ {sandboxValueReference.moduleID} ], can cause unwanted references to new objects, module ref needs to be cleared on connection break in root module...");
            }
            if(connectionSandboxValue != null)
                _components.objectNameLabel.text = $"{connectionSandboxValue.module.data.name} : {connectionSandboxValue.id}";
        }
    }

    internal void Set(SandboxValue sandboxValue)
    {
        _components.objectNameLabel.text = $"{sandboxValue.module.data.name} : {sandboxValue.id}";
        sandboxValueReference.moduleID = sandboxValue.module.data.id;
        sandboxValueReference.valueID = sandboxValue.id;
    }

    public void UE_Click()
    {
        if (rootModule.connections.Count == 0)
            return;
        wUI.Edit(this);
    }
}
