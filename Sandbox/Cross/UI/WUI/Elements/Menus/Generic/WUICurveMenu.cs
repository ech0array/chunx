using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

sealed internal class WUICurveMenu : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal WUICurve wUICurve;
    }
    [SerializeField]
    private Components _components;

    private class Data
    {
        internal WUICurveValue wUICurveValue;
    }
    private Data _data = new Data();

    internal override void Unstack()
    {
        bool changed = _components.wUICurve.TryUnselect();
        if (changed)
            return;
        _data.wUICurveValue.Set();
        wUI.onClick -= Delselect;
        base.Unstack();
    }


    internal void Edit(WUICurveValue wUICurveValue)
    {
        Stack();
        wUI.onClick += Delselect;
        _data.wUICurveValue = wUICurveValue;
        _components.wUICurve.Populate(wUICurveValue);
    }

    private void Delselect()
    {
        _components.wUICurve.TryUnselect();

    }
}
