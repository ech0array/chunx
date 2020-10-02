using System;
using System.Collections.Generic;
using UnityEngine;

internal sealed class UIHead : SingleMonoBehaviour<UIHead>
{
    #region Values
    protected override bool isPersistant => true;

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform hudContainer;
        [SerializeField]
        internal Transform canvasContainer;
        [SerializeField]
        internal Dictionary<User, HUD> userHuds = new Dictionary<User, HUD>();
        [SerializeField]
        internal Dictionary<User, WUI> userEditorMenus = new Dictionary<User, WUI>();
        [SerializeField]
        internal Dictionary<User, WUI> userRuntimeMenus = new Dictionary<User, WUI>();

        [SerializeField]
        internal EntryWUI entryWUI;
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal HUD editorHud;
        [SerializeField]
        internal EditorWUI editorWUI;
        [SerializeField]
        internal RuntimeWUI runtimeWUI;
    }
    [SerializeField]
    private Prefabs _prefabs;

    internal EntryWUI entryWUI => _components.entryWUI; 
    #endregion

    protected override void Awake()
    {
        base.Awake();
        _components.entryWUI.gameObject.SetActive(true);
    }

    internal void RegisterUser(User user)
    {
        CreateHud(user);
        CreateEditorUI(user);
        CreateRuntimeUI(user);
        
        _components.entryWUI.Register(user);
        _components.entryWUI.ShowHome();
        AppropriateRects();
    }
    internal void UnregisterUser(User user)
    {
        Destroy(_components.userHuds[user].gameObject);
        Destroy(_components.userEditorMenus[user].gameObject);
        Destroy(_components.userRuntimeMenus[user].gameObject);

        _components.userHuds.Remove(user);
        _components.userEditorMenus.Remove(user);
        _components.userRuntimeMenus.Remove(user);
        

        AppropriateRects();
    }

    internal void SetEntryUIState(bool value)
    {
        _components.entryWUI.gameObject.SetActive(value);
    }

    private void CreateHud(User user)
    {
        GameObject gameObject = Instantiate(_prefabs.editorHud.gameObject, _components.hudContainer);
        HUD hUD = gameObject.GetComponent<HUD>();
        _components.userHuds.Add(user, hUD);
        user.hUD = hUD;
    }
    private void CreateEditorUI(User user)
    {
        GameObject gameObject = Instantiate(_prefabs.editorWUI.gameObject, _components.canvasContainer);
        EditorWUI editorWUI = gameObject.GetComponent<EditorWUI>();
        _components.userEditorMenus.Add(user, editorWUI);
        editorWUI.Register(user);
        user.editorWUI = editorWUI;
    }
    private void CreateRuntimeUI(User user)
    {
        GameObject gameObject = Instantiate(_prefabs.runtimeWUI.gameObject, _components.canvasContainer);
        RuntimeWUI runtimeWUI = gameObject.GetComponent<RuntimeWUI>();
        _components.userRuntimeMenus.Add(user, runtimeWUI);
        runtimeWUI.Register(user);
        user.runtimeWUI = runtimeWUI;
    }

    private void AppropriateRects()
    {
        foreach (KeyValuePair<User, HUD> item in _components.userHuds)
            item.Value.AppropriateRect(item.Key.controllableCamera.camera);
    }
}