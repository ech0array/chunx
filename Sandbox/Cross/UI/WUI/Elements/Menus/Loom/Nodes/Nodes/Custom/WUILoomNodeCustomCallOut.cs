using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class WUILoomNodeCustomCallOut : MonoBehaviour
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform referenceContainer;

        [SerializeField]
        internal WUILoomExecutionConnector inConnector;
    }
    [SerializeField]
    private Components _components;
    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal WUILoomReference wUILoomReference;
    }
    [SerializeField]
    private Prefabs _prefabs;


    internal void Populate()
    {
        // base.Formalize(loomNode);

        // loop values, create sub refs 
    }
}
