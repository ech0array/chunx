using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

sealed internal class WUIIntergerValue : WUIValue
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TMP_InputField inputField;
    }
    [SerializeField]
    private Components _components;

    internal Action<int> onValueChanged;

    private void Awake()
    {
        BindEvents();
    }

    internal override void Bind(SandboxValue sandboxValue)
    {
        base.valueReference = sandboxValue;
        _components.inputField.text = ((int)sandboxValue.get()).ToString();
        wUITipable.Populate(sandboxValue.id, sandboxValue.description);
    }

    private void BindEvents()
    {
        _components.inputField.onValueChanged.AddListener((string text) => { base.valueReference.set.Invoke(int.Parse(text)); });
    }

    internal static bool IsCompatableTo(Type type)
    {
        return type == typeof(WUIIntergerValue) || type == typeof(int);
    }
}
