using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

internal sealed class WUITagListItem : WUIElement
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TMP_InputField stringInput;
    }
    [SerializeField]
    private Components _components;

    private class Data
    {
        internal Action<WUITagListItem> removalCallback;
    }
    private Data _data = new Data();

    internal string tag;

    internal void Populate(string value, Action<WUITagListItem> removalCallback, Action onChange)
    {
        tag = value;
        _data.removalCallback = removalCallback;
        _components.stringInput.text = value;
        _components.stringInput.onValueChanged.AddListener((string text) => { tag = text; onChange.Invoke(); });
    }

    public void UE_Remove()
    {
        _data.removalCallback.Invoke(this);
    }
}
