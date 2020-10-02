using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

sealed internal class WUITextValue : WUIValue
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TMP_InputField inputField;
    }
    [SerializeField]
    private Components _components;


    private void Awake()
    {
        BindEvents();
    }

    internal override void Bind(SandboxValue sandboxValue)
    {
        base.valueReference = sandboxValue;
        _components.inputField.text = (string)sandboxValue.get();
        wUITipable.Populate(sandboxValue.id, sandboxValue.description);
    }

    private void BindEvents()
    {
        _components.inputField.onValueChanged.AddListener((string text) => { valueReference.set(text); });
    }

    internal static bool IsCompatableTo(Type type)
    {
        return type == typeof(WUITextValue) || type == typeof(string);
    }
}
