using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

sealed internal class WUIRangeValue : WUIValue
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal WUISlider wUISlider;
        [SerializeField]
        internal TextMeshProUGUI text;
    }
    [SerializeField]
    private Components _components;

    internal Action<float> onValueChanged;
    private float _minValue;
    private float _maxValue;
    private bool _round;

    private void Awake()
    {
        BindEvents();
    }

    private void BindEvents()
    {
        _components.wUISlider.onValueChanged += (float value) =>
        {
            value = Mathf.Lerp(_minValue, _maxValue, value);
            value = _round ? Mathf.Round(value) : (float)Math.Round(value, 2);
            _components.text.text = $"{value}";
            base.valueReference.set.Invoke(value);
        };
    }


    internal override void Bind(SandboxValue sandboxValue)
    {
        base.valueReference = sandboxValue;
        float value = (float)sandboxValue.get();
        _round = (bool)sandboxValue.meta[0];
        _minValue = (float)sandboxValue.meta[1];
        _maxValue = (float)sandboxValue.meta[2];

        float absoluteMin = Mathf.Abs(_minValue);
        float ratio = 0f;
        if (_minValue == 0)
            ratio = value / _maxValue;
        else
            ratio = value == 0 ? (absoluteMin / (absoluteMin + _maxValue)) : 0.5f + (value / (absoluteMin + _maxValue));

        _components.wUISlider.Chart(ratio);


        _components.text.text = $"{value}";

        wUITipable.Populate(sandboxValue.id, sandboxValue.description);
    }

    public static bool IsCompatableTo(Type type)
    {
        return type == typeof(WUIFloatValue) || type == typeof(WUIRangeValue) || type == typeof(float) 
            || type == typeof(WUIIntergerValue) || type == typeof(int);
    }
}
