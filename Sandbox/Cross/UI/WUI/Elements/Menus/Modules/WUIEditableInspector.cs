using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed internal class WUIEditableInspector : WUIMenu
{
    #region Values

    [SerializeField]
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI nameText;

        [SerializeField]
        internal RectTransform valueContainer;
        [SerializeField]
        internal WUIEditableTools wUIEditableTools;

        [SerializeField]
        internal GameObject connectButton;
        [SerializeField]
        internal GameObject disconnectButton;

        internal List<WUIValue> values = new List<WUIValue>();

    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal List<WUIValue> values = new List<WUIValue>();
        [SerializeField]
        internal WUIValue spacer;
        [SerializeField]
        internal WUIValue header;
    }
    [SerializeField]
    private Prefabs _prefabs;

    internal Module module;
    private EditorWUI _editorWUI => (EditorWUI)wUI;
    #endregion

    #region Functions
    internal override void Unstack()
    {
        base.Unstack();
        ClearValues();

    }

    internal void Inspect(Module module, Vector3 cursorPosition)
    {
        _components.nameText.text = module.data.name;

        this.module = module;
        Stack();
        wUI.Position(cursorPosition);

        DisplayValues(module);

        bool showConnection = module is ModuleLogicValueAnimator || module is ModuleLogicColorAnimator;
        _components.connectButton.SetActive(showConnection);
        _components.disconnectButton.SetActive(showConnection);
    }


    #region Value Functions
    internal void DisplayValues(Module module)
    {
        SandboxValue[] sandboxValues = module.sandboxValuesById.Values.ToArray();
        foreach (SandboxValue sandboxValue in sandboxValues)
        {
            if (sandboxValue.hideInInspector)
                continue;
            WUIValue wUIValue = CreateValue<WUIValue>(sandboxValue);
            wUIValue.Bind(sandboxValue);
        }
    }

    internal void ClearValues()
    {
        foreach (WUIValue wUIValue in _components.values)
            Destroy(wUIValue.gameObject);
        _components.values.Clear();
    }

    internal T CreateValue<T>(SandboxValue sandboxValue) where T : WUIValue
    {
        if(sandboxValue.header != null && sandboxValue.header != string.Empty)
            _components.values.Add(CreateHeader(sandboxValue.header));


        WUIValue prefab = GetValuePrefab(sandboxValue.wuiValueType);
        GameObject gameObject = Instantiate(prefab.gameObject, _components.valueContainer, false);
        T instance = gameObject.GetComponent<T>();
        instance.wUIMenu = this;

        _components.values.Add(instance);
        if (sandboxValue.spaceAfter > 0)
            _components.values.Add(CreateSpacer(sandboxValue.spaceAfter));
        return instance;
    }

    private WUISpacerValue CreateSpacer(float size)
    {
        GameObject gameObject = Instantiate(_prefabs.spacer.gameObject, _components.valueContainer);
        WUISpacerValue wUISpacerValue = gameObject.GetComponent<WUISpacerValue>();
        wUISpacerValue.SetSize(size);
        return wUISpacerValue;
    }

    private WUIHeaderValue CreateHeader(string text)
    {
        GameObject gameObject = Instantiate(_prefabs.header.gameObject, _components.valueContainer);
        WUIHeaderValue wUIHeaderValue = gameObject.GetComponent<WUIHeaderValue>();
        wUIHeaderValue.SetText(text);
        return wUIHeaderValue;
    }
    private WUIValue GetValuePrefab(Type type)
    {
        foreach (WUIValue wUIValue in _prefabs.values)
        {
            if (wUIValue.GetType() == type)
                return wUIValue;
        }
        return null;
    }
    #endregion

    public void UE_ShowTools()
    {
        _components.wUIEditableTools.Inspect(module);
    }
    public void UE_Connect()
    {
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.Link, module);
        Unstack();
    }
    public void UE_Disconnect()
    {
        wUI.user.controllableCamera.EnterMode(ControllableCamera.Mode.Unlink, module);
        Unstack();
    }
    #endregion
}