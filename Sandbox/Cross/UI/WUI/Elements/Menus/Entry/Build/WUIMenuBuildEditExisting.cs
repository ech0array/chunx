using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

internal sealed class WUIMenuBuildEditExisting : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal WUIMenuBuildEditObjects wUIMenuBuildEditMaps;
        [SerializeField]
        internal WUIMenuBuildEditObjects wUIMenuBuildEditObjects;


        [SerializeField]
        internal TextMeshProUGUI nameLabel;
    }
    [SerializeField]
    private Components _components;

    private class Data
    {
        internal UniverseData universeData;
    }
    private Data _data = new Data();


    internal void Inspect(UniverseData universeData)
    {
        _data.universeData = universeData;
        _components.nameLabel.text = universeData.name;
        Stack();
    }

    private void OnRename(string str)
    {
        _data.universeData.name = str;
        _components.nameLabel.text = _data.universeData.name;
    }

    public void UE_Rename()
    {
        wUI.Edit(_data.universeData.name, OnRename);
    }

    public void UE_Maps()
    {
        base.Hide();
        _components.wUIMenuBuildEditMaps.Inspect(_data.universeData);
    }

    public void UE_Objects()
    {
        base.Hide();
        _components.wUIMenuBuildEditObjects.Inspect(_data.universeData);
    }

}
