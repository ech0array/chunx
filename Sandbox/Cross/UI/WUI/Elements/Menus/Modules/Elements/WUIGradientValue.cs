using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed internal class WUIGradientValue : WUIValue
{

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal Image gradientLeft;
        [SerializeField]
        internal Image gradeintRight;
        [SerializeField]
        internal Image colorLeft;
        [SerializeField]
        internal Image colorRight;
    }
    [SerializeField]
    private Components _components;

    internal SerializableGradient gradient;
    internal Color color => _data.isActiveRight ? gradient.right.color : gradient.left.color;

    private class Data
    {
        internal bool isActiveRight;
    }
    private Data _data = new Data();


    internal override void Bind(SandboxValue sandboxValue)
    {
        base.valueReference = sandboxValue;
        gradient = (SerializableGradient)sandboxValue.get();
        _components.gradientLeft.color = gradient.left.color;
        _components.colorLeft.color = gradient.left.color;
        _components.gradeintRight.color = gradient.right.color;
        _components.colorRight.color = gradient.right.color;

        wUITipable.Populate(sandboxValue.id, sandboxValue.description);
    }

    internal void Set(Color color)
    {
        if (!_data.isActiveRight)
        {
            gradient.left.color = color;
            valueReference.set(gradient);
            _components.gradientLeft.color = gradient.left.color;
            _components.colorLeft.color = gradient.left.color;
        }
        else
        {
            gradient.right.color = color;
            valueReference.set(gradient);
            _components.gradeintRight.color = gradient.right.color;
            _components.colorRight.color = gradient.right.color;
        }
    }

    public void UE_ClickLeft()
    {
        _data.isActiveRight = false;
        wUI.Edit(this);
    }
    public void UE_ClickRight()
    {
        _data.isActiveRight = true;
        wUI.Edit(this);
    }
}