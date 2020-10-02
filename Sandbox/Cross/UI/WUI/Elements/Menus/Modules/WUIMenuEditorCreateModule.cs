using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal sealed class WUIMenuEditorCreateModule : WUIMenu
{

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform moduleContainer;


        [SerializeField]
        internal GameObject envoronment;
        [SerializeField]
        internal GameObject mechanical;

        internal List<Component> activeObjects = new List<Component>();
    }
    [SerializeField]
    private Components _components;


    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal WUIEditorCreateModuleItem wUIEditorModuleObject;
        [SerializeField]
        internal WUIEditorCreateSandboxObjectItem wUIEditorCreateSandboxObjectItem;
    }
    [SerializeField]
    private Prefabs _prefabs;

    protected override void Awake()
    {
        base.Awake();
    }

    internal override bool Stack()
    {
        ShowModeRelative();
        return base.Stack();
    }

    private void ShowModeRelative()
    {
        if (EditorHead.instance.isMap)
            UE_ShowCategoryEnvironment();
        else
            UE_ShowCategoryMechanical();

        _components.envoronment.SetActive(EditorHead.instance.isMap);
        _components.mechanical.SetActive(!EditorHead.instance.isMap);
    }

    private void Populate(List<Module> modules)
    {
        Clear();

        if (modules == null)
            return;

        foreach (Module module in modules)
        {
            GameObject gameObject = Instantiate(_prefabs.wUIEditorModuleObject.gameObject, _components.moduleContainer, false);
            WUIEditorCreateModuleItem wUIEditorModuleObject = gameObject.GetComponent<WUIEditorCreateModuleItem>();
            wUIEditorModuleObject.Populate(module);
            _components.activeObjects.Add(wUIEditorModuleObject);
        }
    }
    private void Populate(List<SandboxObjectData> sandboxObjectDatas)
    {
        Clear();

        if (sandboxObjectDatas == null)
            return;

        foreach (SandboxObjectData sandboxObjectData in sandboxObjectDatas)
        {
            GameObject gameObject = Instantiate(_prefabs.wUIEditorCreateSandboxObjectItem.gameObject, _components.moduleContainer, false);
            WUIEditorCreateSandboxObjectItem wUIEditorCreateSandboxObjectItem = gameObject.GetComponent<WUIEditorCreateSandboxObjectItem>();
            wUIEditorCreateSandboxObjectItem.Populate(sandboxObjectData);
            _components.activeObjects.Add(wUIEditorCreateSandboxObjectItem);
        }
    }

    private void Clear()
    {
        HEAD:
        if (_components.activeObjects.Count == 0)
            return;

        Destroy(_components.activeObjects[0].gameObject);
        _components.activeObjects.Remove(_components.activeObjects[0]);
        goto HEAD;
    }

    public void UE_ShowCategoryVisuals()
    {
        Populate(ModuleHead.instance.GetModulesOfCategory(ModuleCategory.Art));
    }
    public void UE_ShowCategoryHud()
    {
        Populate(ModuleHead.instance.GetModulesOfCategory(ModuleCategory.HUD));
    }
    public void UE_ShowCategoryMechanical()
    {
        Populate(ModuleHead.instance.GetModulesOfCategory(ModuleCategory.Mechanical));
    }
    public void UE_ShowCategoryScripting()
    {
        Populate(ModuleHead.instance.GetModulesOfCategory(ModuleCategory.Scripting));
    }
    public void UE_ShowCategoryObjects()
    {
        Populate(EditorHead.instance.GetNestableObjects());
    }
    public void UE_ShowCategoryEnvironment()
    {
        Populate(ModuleHead.instance.GetModulesOfCategory(ModuleCategory.Environment));
    }
}