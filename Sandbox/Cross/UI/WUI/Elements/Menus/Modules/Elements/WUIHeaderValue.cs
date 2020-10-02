using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

sealed internal class WUIHeaderValue : WUIValue
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI label;
    }
    [SerializeField]
    private Components _components;

    internal override void Bind(SandboxValue sandboxValue)
    {
    }

    internal void SetText(string value)
    {
        _components.label.text = value;
    }
}
