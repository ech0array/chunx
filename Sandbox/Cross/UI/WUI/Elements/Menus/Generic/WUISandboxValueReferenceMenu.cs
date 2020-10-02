using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

internal sealed class WUISandboxValueReferenceMenu : WUIMenu
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

    internal void Edit(Module module, Action<SandboxValue> callback, List<Type> types)
    {
        Clear();
        SandboxValue[] sandboxValues = module.sandboxValuesById.Values.ToArray();
        foreach (SandboxValue sandboxValue in sandboxValues)
        {
            if(types.Contains(sandboxValue.wuiValueType))
            AddValue(module, sandboxValue, callback);
        }
        Stack();
    }

    private void Clear()
    {
        foreach (GameObject gameObject in _components.buttons)
            Destroy(gameObject);
        _components.buttons.Clear();
    }

    private void AddValue(Module module, SandboxValue sandboxValue, Action<SandboxValue> callback)
    {
        GameObject gameObject = Instantiate(_prefabs.wUIMemberBrowserButton.gameObject, _components.buttonContainer, false);
        WUIMemberBrowserButton wUIMemberBrowserButton = gameObject.GetComponent<WUIMemberBrowserButton>();
        _components.buttons.Add(gameObject);
        wUIMemberBrowserButton.onClick += ()=>{ Unstack(); callback.Invoke(sandboxValue); };
        wUIMemberBrowserButton.SetText(sandboxValue.id);
    }
}
