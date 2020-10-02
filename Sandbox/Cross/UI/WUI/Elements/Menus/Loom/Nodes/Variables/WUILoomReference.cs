using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal sealed class WUILoomReference : MonoBehaviour
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI nameLabel;

        [SerializeField]
        internal WUILoomFieldConnector inConnector;
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Data
    {
        internal LoomReference loomReference;
    }
    [SerializeField]
    private Data _data;

    internal void Populate(LoomReference loomReference)
    {
        _data.loomReference = loomReference;
        _components.nameLabel.text = loomReference.GetValue().id;
    }
}