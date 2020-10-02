using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class WUIMenuBuildHome : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal WUIMenuBuildEditExisting wUIMenuBuildEditExisting;

        [SerializeField]
        internal WUIMenuBuildEditUniverses wUIMenuBuildEditUniverses;

        [SerializeField]
        internal GameObject editExistingButton;
    }
    [SerializeField]
    private Components _components;

    private void OnEnable()
    {
        _components.editExistingButton.SetActive(wUI.user.userData.universeDatas?.Count > 0);
    }

    public void UE_NewGame()
    {
        UniverseData universeData = new UniverseData
        {
            name = "test",
            maps = new List<EditableData>(),
            objects = new List<EditableData>(),
        };
        wUI.user.userData.universeDatas.Add(universeData);
        _components.wUIMenuBuildEditExisting.Inspect(universeData);
        UserHead.SaveAllUsersData();
    }

    public void UE_EditExisting()
    {
        _components.wUIMenuBuildEditUniverses.Inspect(wUI.user.userData.universeDatas);
    }
}
