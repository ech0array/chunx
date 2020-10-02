using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed internal class WUIEnumValue : WUIValue
{
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

    internal Action<int> onValueChanged;
    private string[] _names;
    internal string[] names => _names;
    private Dictionary<int, string> _nameValueMap = new Dictionary<int, string>();
    private int _value;

    protected override void Awake()
    {
        base.Awake();
        BindEvents();
    }

    private void BindEvents()
    {
        _components.button.onClick.AddListener(() =>
        {
            wUI.Edit(this);
        });
    }


    internal override void Bind(SandboxValue sandboxValue)
    {
        base.valueReference = sandboxValue;

        _names = Enum.GetNames((Type)sandboxValue.meta[0]);
        int[] values = (int[])Enum.GetValues((Type)sandboxValue.meta[0]);
        for (int i = 0; i < _names.Length; i++)
        {
            _nameValueMap.Add(values[i], _names[i]);
        }

        _value = (int)sandboxValue.get.Invoke();
        Set(_value);

        wUITipable.Populate(sandboxValue.id, sandboxValue.description);
    }


    internal void Set(int value)
    {
        if (_value != value)
            valueReference.set.Invoke(value);
        _value = value;
        _components.label.text = GetCorrectedName(_nameValueMap[value]);
    }

    internal static string GetCorrectedName(string name)
    {
        return Regex.Replace(name, "(\\B[A-Z])", " $1");
    }
}
