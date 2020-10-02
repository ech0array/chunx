using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class WUILoomNodeCustomCallIn : MonoBehaviour
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform parameterContainer;

        [SerializeField]
        internal WUILoomExecutionConnector outConnector;
    }
    [SerializeField]
    private Components _components;
    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal WUILoomParameter wUILoomParameter;
    }
    [SerializeField]
    private Prefabs _prefabs;

}
