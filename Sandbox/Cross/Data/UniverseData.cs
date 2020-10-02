using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
internal sealed class UniverseData
{
    public UniverseData() { }
    public UniverseData(UniverseData universeData)
    {
        name = universeData.name;
        uniqueIds = universeData.uniqueIds;

        maps = new List<EditableData>();
        foreach (EditableData editableData in universeData.maps)
            maps.Add(new SandboxData((SandboxData)editableData));

        objects = new List<EditableData>();
        foreach (EditableData editableData in universeData.objects)
            objects.Add(new SandboxObjectData((SandboxObjectData)editableData));

        savedColors = new List<SerializableColor>(universeData.savedColors);
    }

    internal string name;

    internal List<int> uniqueIds = new List<int>();

    internal List<EditableData> maps = new List<EditableData>();
    internal List<EditableData> objects = new List<EditableData>();

    internal List<SerializableColor> savedColors = new List<SerializableColor>();

    internal int GenerateUniqueId()
    {
        int id = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        bool contained = uniqueIds.Contains(id);
        if (contained)
            return GenerateUniqueId();

        uniqueIds.Add(id);
        return id;
    }
}