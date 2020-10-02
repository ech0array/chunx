using System;
using UnityEngine;

sealed internal class EditorWUI : WUI
{
    [Serializable]
    private class Components : BaseComponents
    {
        [SerializeField]
        internal WUILoomInpsector wUILoomInpsector;
        [SerializeField]
        internal WUIEditableInspector wUIEditableInspectorMenu;


        [SerializeField]
        internal WUIMenuEditorStart wUIMenuEditorStart;
        [SerializeField]
        internal WUIMenuEditorCreateModule wUIMenuEditorCreateModule;


        [SerializeField]
        internal WUIModuleProspect wUIEditablePreview;

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


    internal WUILoomInpsector wUILoomInpsector => _components.wUILoomInpsector;
    internal WUIEditableInspector wUIEditableInspectorMenu => _components.wUIEditableInspectorMenu;


    internal WUIMenuEditorStart wUIMenuEditorStart => _components.wUIMenuEditorStart;
    internal WUIMenuEditorCreateModule wUIMenuEditorCreateModule => _components.wUIMenuEditorCreateModule;


    internal WUIModuleProspect wUIEditablePreview => _components.wUIEditablePreview;
}