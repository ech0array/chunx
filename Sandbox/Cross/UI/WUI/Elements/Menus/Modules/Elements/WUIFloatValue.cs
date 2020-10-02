using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

sealed internal class WUIFloatValue : WUIValue
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TMP_InputField inputField;
    }
    [SerializeField]
    private Components _components;

    internal Action<float> onValueChanged;

    private void Awake()
    {
        BindEvents();
    }

    internal override void Bind(SandboxValue sandboxValue)
    {
        base.valueReference = sandboxValue;
        _components.inputField.text = ((float)sandboxValue.get()).ToString();
        wUITipable.Populate(sandboxValue.id, sandboxValue.description);
    }

    private void BindEvents()
    {
        _components.inputField.onValueChanged.AddListener((string text) => { base.valueReference.set.Invoke(float.Parse(text)); });
    }

    public static bool IsCompatableTo(Type type)
    {
        return type == typeof(WUIFloatValue) || type == typeof(WUIRangeValue) || type == typeof(float) || type == typeof(WUIIntergerValue) || type == typeof(int);
    }
}