using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class WUIMenuBuildEditObjects : WUIMenu
{
    #region Values
    [Serializable]
    private sealed class Components
    {
        [SerializeField]
        internal RectTransform mapContainer;
        [SerializeField]
        internal RectTransform categoryContainer;

        [SerializeField]
        internal RectTransform createNew;

        internal List<WUISandboxObjectData> wUISandboxObjectDatas = new List<WUISandboxObjectData>();

        internal List<WUIMemberBrowserButton> categoryButtons = new List<WUIMemberBrowserButton>();
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private sealed class Prefabs
    {
        [SerializeField]
        internal WUISandboxObjectData map;
        [SerializeField]
        internal WUIMemberBrowserButton categoryButton;
    }
    [SerializeField]
    private Prefabs _prefabs;

    [Serializable]
    private sealed class Data
    {
        [SerializeField]
        internal EditorType editorType;
        internal UniverseData universeData;
        internal string activeCategory = "root";
        internal List<string> categoryStrings = new List<string>();
    }
    [SerializeField]
    private Data _data;
    #endregion

    internal override void Focus()
    {
        base.Focus();
        Refresh();
    }

    public void UE_CreateNew()
    {
        if (_data.editorType == EditorType.Object)
        {
            SandboxObjectData sandboxObjectData = EditorHead.instance.NewSandboxObject(_data.universeData.GenerateUniqueId(), _data.universeData.GenerateUniqueId());
            sandboxObjectData.category = _data.activeCategory;
            _data.universeData.objects.Add(sandboxObjectData);
        }
        else
        {
            SandboxData sandboxData = EditorHead.instance.NewSandbox(_data.universeData.GenerateUniqueId());
            _data.universeData.maps.Add(sandboxData);
        }

        UserHead.SaveAllUsersData();
        Refresh();
    }


    internal void Inspect(UniverseData universeData)
    {
        _data.universeData = universeData;
        Stack();
        Populate(_data.editorType == EditorType.Map ? universeData.maps : universeData.objects);
    }

    private void Refresh()
    {
        UserHead.SaveAllUsersData();
        Populate(_data.editorType == EditorType.Map ? _data.universeData.maps : _data.universeData.objects);
    }

    private void Populate(List<EditableData> sandboxObjectDatas)
    {
        Clear();

        for (int i = 0; i < sandboxObjectDatas.Count; i++)
        {
            EditableData editableData = sandboxObjectDatas[i];
            if (_data.editorType == EditorType.Object)
            {
                if (editableData.category == null || editableData.category == string.Empty)
                    editableData.category = "root";

                if (!_data.categoryStrings.Contains(editableData.category))
                    _data.categoryStrings.Add(editableData.category);
            }

            GameObject gameObject = Instantiate(_prefabs.map.gameObject, _components.mapContainer, false);
            WUISandboxObjectData wUISandboxObjectData = gameObject.GetComponent<WUISandboxObjectData>();
            wUISandboxObjectData.Populate(_data.universeData, editableData, this);
            _components.wUISandboxObjectDatas.Add(wUISandboxObjectData);
        }
        _components.createNew.SetAsFirstSibling();

        if(_data.editorType == EditorType.Object)
        {
            foreach (string category in _data.categoryStrings)
            {
                GameObject gameObject = Instantiate(_prefabs.categoryButton.gameObject, _components.categoryContainer, false);
                WUIMemberBrowserButton wUIMemberBrowserButton = gameObject.GetComponent<WUIMemberBrowserButton>();
                if (category == "root")
                {
                    _components.categoryButtons.Insert(0, wUIMemberBrowserButton);
                    wUIMemberBrowserButton.transform.SetAsFirstSibling();
                }
                else
                {
                    _components.categoryButtons.Add(wUIMemberBrowserButton);
                    wUIMemberBrowserButton.transform.SetAsLastSibling();
                }

                wUIMemberBrowserButton.SetText(category);
                wUIMemberBrowserButton.onClick += () => { SetCategory(category); };
            }
        SetCategory(_data.activeCategory);
        }

    }
    private void Clear()
    {
        foreach (WUISandboxObjectData wUISandboxObjectData in _components.wUISandboxObjectDatas)
            Destroy(wUISandboxObjectData.gameObject);

        foreach (WUIMemberBrowserButton wUIMemberBrowserButton in _components.categoryButtons)
            Destroy(wUIMemberBrowserButton.gameObject);

        _components.wUISandboxObjectDatas.Clear();
        _components.categoryButtons.Clear();
        _data.categoryStrings.Clear();
    }

    private void SetCategory(string category)
    {
        _data.activeCategory = category;

        foreach (WUISandboxObjectData wUISandboxObjectData in _components.wUISandboxObjectDatas)
            wUISandboxObjectData.gameObject.SetActive(wUISandboxObjectData.editableData.category == category);
    }
}