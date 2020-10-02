using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal sealed class EntryWUI : WUI
{
    [Serializable]
    private class Components : BaseComponents
    {
        [SerializeField]
        internal RectTransform staticBackground;
        [SerializeField]
        internal WUIMenuHome wUIMenuHome;
        [SerializeField]
        internal WUIMenuEntry wUIMenuEntry;
        [SerializeField]
        internal WUICategoryScreen wUICategoryScreen;
    }
    [SerializeField]
    private Components _components;
    protected override BaseComponents baseComponents => _components;


    [Serializable]
    private class Data : BaseData
    {

    }
    [SerializeField]
    private Data _data;
    protected override BaseData baseData => _data;
    internal void ShowHome()
    {
        _components.wUIMenuEntry.Hide();
        _components.wUIMenuHome.Stack();
    }

    protected override void Awake()
    {
        base.Awake();
        Canvas canvas = _components.staticBackground.gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = -100;
    }

    internal void Move(EditableData editableData)
    {
        _components.wUICategoryScreen.Inspect(editableData);
    }
}