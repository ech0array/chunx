using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

internal sealed class WUILoomModulesModuleElement : MonoBehaviour
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI nameLabel;

        internal WUILoomModules wUILoomModules;
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Data
    {
        internal Module module;
    }
    [SerializeField]
    private Data _data;

    internal void Populate(WUILoomModules wUILoomModules, Module module)
    {
        _data.module = module;
        _components.wUILoomModules = wUILoomModules;
        _components.nameLabel.text = module.data.name;
    }

    public void UE_Click()
    {
        _components.wUILoomModules.Select(_data.module);
    }
}
