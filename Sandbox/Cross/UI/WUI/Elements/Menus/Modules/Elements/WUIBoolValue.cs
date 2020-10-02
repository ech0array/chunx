using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

sealed internal class WUIBoolValue : WUIValue
{
    #region Values
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal Button button;

        [SerializeField]
        internal TextMeshProUGUI label;
    }
    [SerializeField]
    private Components _components;

    internal Action<bool> onValueChanged;
    private bool _value;
    #endregion

    #region Unity Framework Entry Functions
    private void Awake()
    {
        BindEvents();
    }
    #endregion

    #region Functions
    internal override void Bind(SandboxValue sandboxValue)
    {
        base.valueReference = sandboxValue;
        _value = (bool)sandboxValue.get();
        wUITipable.Populate(sandboxValue.id, sandboxValue.description);
        Display();
    }

    private void BindEvents()
    {
        _components.button.onClick.AddListener(() =>
        {
            _value = !_value;
            base.valueReference.set(_value);
            Display();
        });
    }

    private void Display()
    {
        _components.label.text = _value ? "on" : "off";
    }
    internal static bool IsCompatableTo(Type type)
    {
        return type == typeof(WUIBoolValue) || type == typeof(bool);
    } 
    #endregion
}