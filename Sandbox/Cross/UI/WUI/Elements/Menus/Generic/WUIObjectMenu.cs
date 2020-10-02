using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

sealed internal class WUIObjectMenu : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform objectContainer;
        internal WUIObjectReferenceValue wUIObjectReferenceValue;
        internal List<WUIObjectMenuButton> buttons = new List<WUIObjectMenuButton>();
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal WUIObjectMenuButton objectButton;
    }
    [SerializeField]
    private Prefabs _prefabs;

    internal void Open(WUIObjectReferenceValue wUIObjectReferenceValue)
    {
        _components.wUIObjectReferenceValue = wUIObjectReferenceValue;
        Populate();
        Stack();
    }

    private void Set(int id)
    {
        Unstack();
        _components.wUIObjectReferenceValue.Set(id);
    }

    private void Populate()
    {
        Clear();
        foreach (SandboxObjectData  sandboxObjectData in GameHead.instance.universeData.objects)
            CreateObjectButton(sandboxObjectData);
    }
    private void Clear()
    {
        foreach (WUIObjectMenuButton wUIObjectMenuButton in _components.buttons)
            Destroy(wUIObjectMenuButton.gameObject);
        _components.buttons.Clear();
    }

    private void CreateObjectButton(SandboxObjectData sandboxObjectData)
    {
        GameObject gameObject = Instantiate(_prefabs.objectButton.gameObject, _components.objectContainer, false);
        WUIObjectMenuButton wUIObjectMenuButton = gameObject.GetComponent<WUIObjectMenuButton>();
        _components.buttons.Add(wUIObjectMenuButton);
        wUIObjectMenuButton.Populate(sandboxObjectData, Set);
    }
}
