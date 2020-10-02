using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal sealed class WUILoomVariable : MonoBehaviour
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI nameLabel;

        [SerializeField]
        internal WUILoomFieldConnector inConnector;
        [SerializeField]
        internal WUILoomFieldConnector outConnector;
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Data
    {
        internal LoomVariable loomVariable;
    }
    [SerializeField]
    private Data _data;

    internal void Populate(LoomVariable loomVariable)
    {
        _data.loomVariable = loomVariable;
        _components.nameLabel.text = loomVariable.GetValue().id;
    }
}
