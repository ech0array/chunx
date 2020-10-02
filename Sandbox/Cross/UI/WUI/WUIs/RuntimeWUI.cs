using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class RuntimeWUI : WUI
{
    [Serializable]
    private class Components : BaseComponents
    {
        [SerializeField]
        internal WUIMenuRuntimeStart wUIMenuRuntimeStart;
    }
    [SerializeField]
    private Components _components;


    protected override BaseComponents baseComponents => _components;
    protected override BaseData baseData => new BaseData();

    internal WUIMenuRuntimeStart wUIMenuRuntimeStart => _components.wUIMenuRuntimeStart;
}