using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal sealed class WUIMenuBuildEditUniverses : WUIMenu
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal WUIMenuBuildEditExisting wUIMenuBuildEditExisting;

        [SerializeField]
        internal RectTransform universeContainer;

        internal List<WUIUniverseObjectData> wUIUniverseObjectDatas = new List<WUIUniverseObjectData>();
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Prefabs
    {
        [SerializeField]
        internal WUIUniverseObjectData wUIUniverseObjectData;
    }
    [SerializeField]
    private Prefabs _prefabs;

    internal void Inspect(List<UniverseData> universeDatas)
    {
        Populate(universeDatas);
        Stack();
    }

    private void Populate(List<UniverseData> universeDatas)
    {
        Clear();

        foreach (UniverseData universeData in universeDatas)
        {
            GameObject gameObject = Instantiate(_prefabs.wUIUniverseObjectData.gameObject, _components.universeContainer, false);
            WUIUniverseObjectData wUIUniverseObjectData = gameObject.GetComponent<WUIUniverseObjectData>();
            wUIUniverseObjectData.Populate(universeData, this, () => 
            {
                _components.wUIMenuBuildEditExisting.Inspect(universeData);
                Hide();
            });
            _components.wUIUniverseObjectDatas.Add(wUIUniverseObjectData);
        }
    }

    private void Clear()
    {
        HEAD:
        if (_components.wUIUniverseObjectDatas.Count == 0)
            return;
        if(_components.wUIUniverseObjectDatas[0] != null)
            Destroy(_components.wUIUniverseObjectDatas[0].gameObject);
        _components.wUIUniverseObjectDatas.Remove(_components.wUIUniverseObjectDatas[0]);
        goto HEAD;
    }


}
