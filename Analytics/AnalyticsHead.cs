using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum AnalyticMetric : uint
{
    Placeholder
}

internal sealed class AnalyticsHead : SingleMonoBehaviour<AnalyticsHead>
{
    protected override bool isPersistant => true;
    private Dictionary<AnalyticMetric, int> _idCountMap;

    internal void TrackMetric(AnalyticMetric id, int count = 1)
    {
        if (_idCountMap.ContainsKey(id))
            _idCountMap[id] += count;
        else
            _idCountMap.Add(id, count);
    }
}