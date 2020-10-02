using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal sealed class WUILoomModules : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform moduleElementContainer;

        internal List<WUILoomModulesModuleElement> wUILoomModulesModuleElements = new List<WUILoomModulesModuleElement>();

        [SerializeField]
        internal WUILoomModuleMembers wUILoomModuleMembers;
    }
    [SerializeField]
    private Components _components;


    [Serializable]
    private class Data
    {

    }
    [SerializeField]
    private Data _data;


    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal WUILoomModulesModuleElement wUILoomModulesModuleElement;
    }
    [SerializeField]
    private Prefabs _prefabs;


    internal void Inspect(List<Module> modules)
    {
        Clear();

        foreach (Module module in modules)
            CreateModuleElement(module);

        Stack();
    }

    private void CreateModuleElement(Module module)
    {
        GameObject gameObject = Instantiate(_prefabs.wUILoomModulesModuleElement.gameObject, _components.moduleElementContainer, false);
        WUILoomModulesModuleElement wUILoomModulesModuleElement = gameObject.GetComponent<WUILoomModulesModuleElement>();
        _components.wUILoomModulesModuleElements.Add(wUILoomModulesModuleElement);
        wUILoomModulesModuleElement.Populate(this, module);
    }

    private void Clear()
    {
        foreach (WUILoomModulesModuleElement wUILoomModulesModuleElement in _components.wUILoomModulesModuleElements)
            Destroy(wUILoomModulesModuleElement.gameObject);

        _components.wUILoomModulesModuleElements.Clear();
    }

    internal void Select(Module module)
    {
        _components.wUILoomModuleMembers.Populate(this, module);
    }
}
