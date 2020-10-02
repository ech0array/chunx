using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;


internal enum LogType
{
    Log,
    Warning,
    Error,
    Reminder
}

internal sealed class DevelopmentHead : SingleMonoBehaviour<DevelopmentHead>
{
    protected override bool isPersistant => true;

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TextMeshProUGUI frameRateRunningLabel;

        [SerializeField]
        internal TextMeshProUGUI trackLabel;
        [SerializeField]
        internal TextMeshProUGUI logLabel;
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Data
    {
        [SerializeField]
        internal bool showLog;
        [SerializeField]
        internal bool showTrack;

        internal float accumulation = 0;
        internal int frames = 0;
        internal float timeleft;
        internal int frameRate;

        internal ulong debugCounter;
        internal Dictionary<string, object> keyObjectMap = new Dictionary<string, object>();
        internal Dictionary<string, Color> keyColorMap = new Dictionary<string, Color>();
        internal Dictionary<string, float> logDurationMap = new Dictionary<string, float>();
    }
    [SerializeField]
    private Data _data = new Data();

    private void Update()
    {
        UpdateFrameRate();
        UpdateTrack();
        UpdateLog();
    }

    private void UpdateFrameRate()
    {
        _data.timeleft -= Time.deltaTime;
        _data.accumulation += Time.timeScale / Time.deltaTime;
        _data.frames++;

        if (_data.timeleft <= 0.0)
        {
            float fps = _data.accumulation / _data.frames;

            if (0 > (int)fps)
                fps = 0;

            _data.frameRate = (int)fps;


            _data.timeleft = 0.1f;
            _data.accumulation = 0;
            _data.frames = 0;
        }

        _data.debugCounter += 0x00723345654345A3;
        string debugCounter = _data.debugCounter.ToString("X16");

        _components.frameRateRunningLabel.text = $"{_data.frameRate} : {debugCounter}";
    }

    internal static void Track(string key, object value, Color? color = null)
    {
        if (instance == null)
            return;
        if (!instance._data.showTrack)
            return;
        if (color == null)
            color = Color.white;

        if (instance._data.keyObjectMap.ContainsKey(key))
        {
            instance._data.keyObjectMap[key] = value;
            instance._data.keyColorMap[key] = (Color)color;
        }
        else
        {
            instance._data.keyObjectMap.Add(key, value);
            instance._data.keyColorMap.Add(key, (Color)color);
        }
    }
    private void UpdateTrack()
    {
        string accumulativeString = string.Empty;
        int counter = 0;
        foreach (KeyValuePair<string, object> keyValueMapEntry in _data.keyObjectMap)
        {
            string newLine = counter == 0 ? string.Empty : "\n";
            object value = keyValueMapEntry.Value;
            string valueString = value == null ? "NULL" : value.ToString();
            
            accumulativeString += $"<color=#{ColorUtility.ToHtmlStringRGBA(_data.keyColorMap[keyValueMapEntry.Key])}>{newLine}{keyValueMapEntry.Key} : {value} </color>".ToLower();
            counter++;
        }
        _components.trackLabel.text = accumulativeString;
    }

    internal static void Log(string value, LogType logType, float duration = 3f)
    {
        if (!instance._data.showLog)
            return;
        Color color = ColorByLogType(logType);
        string colorHex = $"#{ColorUtility.ToHtmlStringRGB(color)}";
        value = $"<color={colorHex}>{value}";

        if (instance._data.logDurationMap.ContainsKey(value))
            instance._data.logDurationMap[value] = duration;
        else
            instance._data.logDurationMap.Add(value, duration);
    }
    private static Color ColorByLogType(LogType logType)
    {
        switch (logType)
        {
            case LogType.Log:
                return Color.white;
                break;
            case LogType.Warning:
                return Color.yellow;
                break;
            case LogType.Error:
                return Color.red;
                break;
            case LogType.Reminder:
                return Color.cyan;
                break;
            default:
                break;
        }
        return Color.white;
    }
    private void UpdateLog()
    {
        string accumulativeString = string.Empty;
        string[] keys = _data.logDurationMap.Keys.ToArray();
        for (int i = 0; i < keys.Length; i++)
        {
            string key = keys[i];
            string newLine = i == 0 ? string.Empty : "\n";
            accumulativeString += $"{newLine}{key}";

            _data.logDurationMap[key] -= Time.deltaTime;
            if (_data.logDurationMap[key] <= 0f)
                _data.logDurationMap.Remove(key);
        }
        _components.logLabel.text = accumulativeString;
    }
}