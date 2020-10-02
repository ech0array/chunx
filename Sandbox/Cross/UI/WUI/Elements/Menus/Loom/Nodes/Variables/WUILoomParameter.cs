using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

internal sealed class WUILoomParameter : MonoBehaviour
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI nameLabel;

        [SerializeField]
        internal WUILoomFieldConnector outConnector;
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Data
    {
        internal LoomParameter loomParameter;
    }
    [SerializeField]
    private Data _data;

    internal void Populate(LoomParameter loomParameter)
    {
        _data.loomParameter = loomParameter;
        _components.nameLabel.text = loomParameter.GetValue().id;
    }
}
