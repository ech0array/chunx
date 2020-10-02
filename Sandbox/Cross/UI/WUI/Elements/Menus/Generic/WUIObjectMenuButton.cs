using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

internal sealed class WUIObjectMenuButton : MonoBehaviour
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI nameLabel;
    }
    [SerializeField]
    private Components _components;

    private class Data
    {
        internal Action<int> callback;
        internal int id;
    }
    private Data _data = new Data();

    internal void Populate(SandboxObjectData sandboxObjectData, Action<int> callback)
    {
        _data.callback = callback;
        _components.nameLabel.text = sandboxObjectData.name;
        _data.id = sandboxObjectData.id;
    }

    public void UE_Click()
    {
        _data.callback.Invoke(_data.id);
    }
}
