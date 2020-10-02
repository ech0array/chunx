using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed internal class WUIColorValue : WUIValue
{

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal Image colorDisplayImage;
    }
    [SerializeField]
    private Components _components;

    internal Color color;


    internal override void Bind(SandboxValue sandboxValue)
    {
        base.valueReference = sandboxValue;
        color = (Color)sandboxValue.get();
        _components.colorDisplayImage.color = color;

        wUITipable.Populate(sandboxValue.id, sandboxValue.description);
    }

    internal void Set(Color color)
    {
        this.color = color;
        valueReference.set(color);
        _components.colorDisplayImage.color = color;
    }

    public void UE_Click()
    {
        wUI.Edit(this);
    }
}