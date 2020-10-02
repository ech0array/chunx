using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
internal sealed class WUIMenuConfirm : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI intentLabel; 
    }
    [SerializeField]
    private Components _components;

    private class Data
    {
        internal Action action;
    }
    private Data _data = new Data();

    internal void Confirm(string intent, Action action)
    {
        _components.intentLabel.text = intent;
        _data.action = action;
        Stack();
    }
    
    public void UE_Confirm()
    {
        Unstack();
        _data.action.Invoke();
    }

    public void UE_Cancel()
    {
        Unstack();
    }
}
