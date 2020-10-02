using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


internal sealed class WUILoomNodeView : MonoBehaviour
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform nodeContainer;
    }
    [SerializeField]
    private Components _components;


    [Serializable]
    private class Prefabs
    {

    }
    [SerializeField]
    private Prefabs _prefabs;

    internal void Populate(ModuleLogicLink moduleLogicLink)
    {
        // use a fucking pool kid
    }
    internal void Clear()
    {

    }

    internal void DropAll()
    {

    }
}
