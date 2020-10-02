using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

sealed internal class WUISpacerValue : WUIValue
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform rectTransform;
    }
    [SerializeField]
    private Components _components;

    internal override void Bind(SandboxValue sandboxValue)
    {
    }

    internal void SetSize(float value)
    {
        Vector2 size = _components.rectTransform.sizeDelta;
        _components.rectTransform.sizeDelta = new Vector2(size.x, (float)value * 5f);
    }
}
