using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

internal sealed class WUILoomNodeModuleData : WUILoomNode
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform variableContainer;

        [SerializeField]
        internal WUILoomExecutionConnector inConnector;

        [SerializeField]
        internal TextMeshProUGUI nameLabel;
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal WUILoomVariable wUILoomVariable;
    }
    [SerializeField]
    private Prefabs _prefabs;

    internal void Populate(LoomModuleData loomModuleData)
    {
        base.Formalize(loomModuleData);

        _components.nameLabel.text = loomModuleData.module.data.name;

        foreach (LoomVariable loomVariable in loomModuleData.variables)
        {
            Create(loomVariable);
        }

        WUI.RebuildLayoutRecursive((RectTransform)base.transform);
        // loop values, create sub refs 

        // consider call chains, if the value in the chain gets itself somehow....
    }

    private void Create(LoomVariable loomVariable)
    {
        GameObject gameObject = Instantiate(_prefabs.wUILoomVariable.gameObject, _components.variableContainer);
        WUILoomVariable wUILoomVariable = gameObject.GetComponent<WUILoomVariable>();
        wUILoomVariable.Populate(loomVariable);
    }
}
