using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal sealed class WUIUniverseObjectData : WUIElement
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI nameLabel;
        [SerializeField]
        internal WUIMenuBuildSelectObject wUIMenuBuildSelectObject;
        internal WUIMenuBuildEditUniverses parent;
    }
    [SerializeField]
    private Components _components;

    private class Data
    {
        internal UniverseData universeData;
        internal Action clickAction;
    }
    private Data _data = new Data();

    internal void Populate(UniverseData universeData, WUIMenuBuildEditUniverses parent, Action clickAction)
    {
        _data.universeData = universeData;
        _data.clickAction = clickAction;
        _components.nameLabel.text = universeData.name;
        _components.parent = parent;
    }

    public void UE_Click()
    {
        GameHead.instance.universeData = _data.universeData;
        _components.wUIMenuBuildSelectObject.Inspect(
            _data.clickAction,

        _data.universeData.name,

        (string name) =>
        {
            _data.universeData.name = name;
            _components.nameLabel.text = name;
            UserHead.SaveAllUsersData();
        },
        () =>
        {
            wUI.user.userData.universeDatas.Remove(_data.universeData);
            if (wUI.user.userData.universeDatas.Count == 0)
                wUI.UnstackCurrent();

            UserHead.SaveAllUsersData();
            Destroy(base.gameObject);

            wUI.user.SaveData();
        },
        () =>
        {
            wUI.user.userData.universeDatas.Add(new UniverseData(_data.universeData));
            wUI.user.SaveData();
            _components.parent.Inspect(wUI.user.userData.universeDatas);
        }
        );
    }
}
