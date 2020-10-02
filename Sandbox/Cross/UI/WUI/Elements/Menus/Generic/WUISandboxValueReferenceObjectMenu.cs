using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal sealed class WUISandboxValueReferenceObjectMenu : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform buttonContainer;
        internal List<GameObject> buttons = new List<GameObject>();
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal WUIMemberBrowserButton wUIMemberBrowserButton;
    }
    [SerializeField]
    private Prefabs _prefabs;

    private WUISandboxValueReference _wUISandboxValueReference;

    internal void Edit(WUISandboxValueReference wUISandboxValueReference)
    {
        _wUISandboxValueReference = wUISandboxValueReference;
        Clear();
        foreach (Connection connection in wUISandboxValueReference.rootModule.connections)
            AddModule(connection.GetOtherTo(wUISandboxValueReference.rootModule));
        Stack();
    }

    private void Clear()
    {
        foreach (GameObject gameObject in _components.buttons)
            Destroy(gameObject);
        _components.buttons.Clear();
    }

    private void AddModule(Module module)
    {
        GameObject gameObject = Instantiate(_prefabs.wUIMemberBrowserButton.gameObject, _components.buttonContainer, false);
        WUIMemberBrowserButton wUIMemberBrowserButton = gameObject.GetComponent<WUIMemberBrowserButton>();
        _components.buttons.Add(gameObject);
        wUIMemberBrowserButton.onClick += ()=>{ wUI.Edit(module, OnSelectValue, _wUISandboxValueReference.types); };
        wUIMemberBrowserButton.SetText(module.data.name);
    }

    private void OnSelectValue(SandboxValue sandboxValue)
    {
        _wUISandboxValueReference.Set(sandboxValue);
        Unstack();
    }
}
