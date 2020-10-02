using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal sealed class WUIGradient : Selectable
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal Graphic leftColor;
        [SerializeField]
        internal Graphic rightColor;
    }
    [SerializeField]
    private Components _components;

    internal void Inspect(Color a, Color b)
    {

    }
}
