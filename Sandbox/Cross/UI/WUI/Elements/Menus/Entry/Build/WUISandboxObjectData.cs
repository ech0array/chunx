using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

internal sealed class WUISandboxObjectData : WUIElement
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI nameLabel;

        [SerializeField]
        internal WUIMenuBuildSelectObject wUIMenuBuildSelectObject;

        internal WUIMenuBuildEditObjects parent;
    }
    [SerializeField]
    private Components _components;

    private class Data
    {
        internal UniverseData universeData;
        internal EditableData editableData;
    }
    private Data _data = new Data();

    internal EditableData editableData => _data.editableData; 

    internal void Populate(UniverseData universeData, EditableData editableData, WUIMenuBuildEditObjects parent)
    {
        _data.universeData = universeData;
        _data.editableData = editableData;
        _components.nameLabel.text = editableData.name;
        _components.parent = parent;
    }

    public void UE_Click()
    {
        // mb find better way to enumerate :/
        Action moveToAction = null;
        if(_data.editableData is SandboxObjectData)
        {
            moveToAction = () =>
            {
                ((EntryWUI)wUI).Move(_data.editableData);
            };
        }

        _components.wUIMenuBuildSelectObject.Inspect(() =>
        {
            EditorHead.instance.Edit(_data.universeData, _data.editableData);
        },

        _data.editableData.name,

        (string name) =>
        {
            _data.editableData.name = name;
            _components.nameLabel.text = name;
            UserHead.SaveAllUsersData();
        },
        () =>
        {
            if (_data.editableData is SandboxObjectData)
                _data.universeData.objects.Remove(_data.editableData);
            else
                _data.universeData.maps.Remove(_data.editableData);
            UserHead.SaveAllUsersData();
            Destroy(base.gameObject);

            UserHead.SaveAllUsersData();
        },
        () =>
        {
            if (_data.editableData is SandboxObjectData)
                _data.universeData.objects.Add(new SandboxObjectData((SandboxObjectData)_data.editableData));
            else
                _data.universeData.maps.Add(new SandboxData((SandboxData)_data.editableData));

            UserHead.SaveAllUsersData();
            _components.parent.Inspect(_data.universeData);
        },
        moveToAction
        );
    }
}
