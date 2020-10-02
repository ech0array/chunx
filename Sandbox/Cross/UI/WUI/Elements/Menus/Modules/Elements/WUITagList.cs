using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

sealed internal class WUITagList : WUIValue
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform container;

        internal List<WUITagListItem> wUITagListItems = new List<WUITagListItem>();
    }
    [SerializeField]
    private Components _components;


    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal WUITagListItem wUITagListItem;
    }
    [SerializeField]
    private Prefabs _prefabs;


    internal override void Bind(SandboxValue sandboxValue)
    {
        base.valueReference = sandboxValue;
        wUITipable.Populate(sandboxValue.id, sandboxValue.description);

        List<string> tags = ((List<string>)sandboxValue.get());
        if (tags != null)
        {
            foreach (string tag in tags)
                CreateTag(tag);
        }
    }

    private void CreateTag(string value)
    {
        GameObject gameObject = Instantiate(_prefabs.wUITagListItem.gameObject, _components.container, false);
        WUITagListItem wUITagListItem = gameObject.GetComponent<WUITagListItem>();
        _components.wUITagListItems.Add(wUITagListItem);
        wUITagListItem.Populate(value, RemoveTag, Refresh);
        wUI.RebuildVisibleLayouts();
    }

    private void Refresh()
    {
        List<string> tags = new List<string>();
        foreach (WUITagListItem wUITagListItem in _components.wUITagListItems)
            tags.Add(wUITagListItem.tag);

        base.valueReference.set(tags);
        wUI.RebuildVisibleLayouts();
    }

    private void RemoveTag(WUITagListItem wUITagListItem)
    {
        _components.wUITagListItems.Remove(wUITagListItem);
        // remove from container - due to destruction delay
        wUITagListItem.gameObject.transform.SetParent(null);
        Destroy(wUITagListItem.gameObject);
        Refresh();
    }

    internal static bool IsCompatableTo(Type type)
    {
        return type == typeof(WUITagList);
    }
    public void UE_Add()
    {
        CreateTag("default");
        Refresh();
    }
}
