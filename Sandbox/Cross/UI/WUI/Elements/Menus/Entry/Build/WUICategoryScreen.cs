using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

internal sealed class WUICategoryScreen : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform categoryContainer;
        internal List<WUIMemberBrowserButton> buttons = new List<WUIMemberBrowserButton>();
        internal List<string> categories = new List<string>();
        [SerializeField]
        internal RectTransform createNew;
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

    private class Data
    {
        internal EditableData editableData;
    }
    private Data _data = new Data();

    internal void Inspect(EditableData editableData)
    {
        Stack();

        _data.editableData = editableData;

        Clear();

        foreach (EditableData sandboxObjectData in GameHead.instance.universeData.objects)
        {
            if (_components.categories.Contains(sandboxObjectData.category))
                continue;
            CreateButton(sandboxObjectData.category);
        }
        _components.createNew.SetAsFirstSibling();
    }

    private void Clear()
    {
        foreach (WUIMemberBrowserButton wUIMemberBrowserButton in _components.buttons)
            Destroy(wUIMemberBrowserButton.gameObject);
        _components.buttons.Clear();
        _components.categories.Clear();
    }

    private void CreateButton(string category)
    {
        if (category == string.Empty || category == null)
            return;

        _components.categories.Add(category);
        GameObject gameObject = Instantiate(_prefabs.wUIMemberBrowserButton.gameObject, _components.categoryContainer, false);
        WUIMemberBrowserButton wUIMemberBrowserButton = gameObject.GetComponent<WUIMemberBrowserButton>();
        _components.buttons.Add(wUIMemberBrowserButton);
        wUIMemberBrowserButton.onClick += ()=> { OnChooseCategory(category); };
        wUIMemberBrowserButton.SetText(category);
        if (category == "root")
            wUIMemberBrowserButton.transform.SetAsFirstSibling();
        else
            wUIMemberBrowserButton.transform.SetAsLastSibling();
    }

    public void UE_CreateNewCategory()
    {
        wUI.Edit(string.Empty, OnChooseCategory);
    }

    private void OnChooseCategory(string category)
    {
        _data.editableData.category = category;
        UserHead.SaveAllUsersData();
        Unstack();
    }
}
