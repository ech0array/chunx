using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal sealed class WUITipable : MonoBehaviour
{
    internal string name { get; private set; }
    internal string description { get; private set; }
    internal Color color { get; private set; }

    [Serializable]
    private class Data
    {
        [SerializeField]
        internal string name;
        [SerializeField]
        internal string description;
    }
    [SerializeField]
    private Data _data;

    private void Awake()
    {
        name = _data.name;
        description = _data.description;
    }

    private void Start()
    {
        Selectable selectable = base.gameObject.GetComponent<Selectable>();
        color = selectable != null ? selectable.colors.normalColor : Color.white;
    }

    internal void Populate(string name, string description)
    {
        this.name = name;
        this.description = description;
    }
}
