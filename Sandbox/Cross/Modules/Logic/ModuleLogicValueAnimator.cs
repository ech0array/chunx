using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModuleLogicValueAnimatorData : ModuleArtData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleLogicValueAnimatorData
            {
                name = "Value Animator",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,
                serializableCurve = new SerializableCurve(Vector2.zero, Vector2.one * 0.25f, Vector2.one * 0.75f, Vector2.one),
                sandboxValueReference = new SandboxValueReference()
            };
        }
    }
    public ModuleLogicValueAnimatorData() { }
    public ModuleLogicValueAnimatorData(ModuleLogicValueAnimatorData modulesValueAnimatorData)
    {
        id = modulesValueAnimatorData.id;
        parentId = modulesValueAnimatorData.parentId;

        name = modulesValueAnimatorData.name;
        position = modulesValueAnimatorData.position;
        rotation = modulesValueAnimatorData.rotation;
        scale = modulesValueAnimatorData.scale;
        tags = modulesValueAnimatorData.tags;
        if(modulesValueAnimatorData.connections != null)
            connections = new List<int>(modulesValueAnimatorData.connections);

        duration = modulesValueAnimatorData.duration;
        loop = modulesValueAnimatorData.loop;
        playAtStart = modulesValueAnimatorData.playAtStart;
        value = modulesValueAnimatorData.value;
        serializableCurve = new SerializableCurve(modulesValueAnimatorData.serializableCurve);
        sandboxValueReference = new SandboxValueReference(modulesValueAnimatorData.sandboxValueReference);
    }


    internal List<int> connections = new List<int>();

    internal float duration;
    internal bool playAtStart;
    internal bool loop;
    internal float value;
    internal SandboxValueReference sandboxValueReference;
    internal SerializableCurve serializableCurve;

    internal override ModuleData Clone()
    {
        return new ModuleLogicValueAnimatorData(this);
    }
}

internal sealed class ModuleLogicValueAnimator : ModuleArt
{
    internal ModuleLogicValueAnimatorData _data = new ModuleLogicValueAnimatorData();
    internal override ModuleData data
    {
        get
        {
            return _data;
        }
    }


    private SandboxValue _sandboxValue;
    private bool _playing;
    private float _progress;


    #region Unity Framework Entry Functions
    private void Update()
    {
        UpdateAnimation();
    }

    #endregion

    internal override void PopulateData(ModuleData objectData)
    {
        _data = new ModuleLogicValueAnimatorData((ModuleLogicValueAnimatorData)objectData);
        ApplyData();
    }

    protected override void ApplyData()
    {
        base.ApplyData();
    }

    protected override void OnEditorLoad()
    {
        DisplayConnections();
    }
    protected override void OnRuntimeLoad()
    {
        DisplayConnections();
    }

    internal override bool OnAttach(User user)
    {
        if (_data.playAtStart)
            Play();
        return false;
    }

    protected override void RegisterSandboxValues()
    {
        SandboxValue playAtStart = new SandboxValue
        {
            module = this,
            id = "Play At Start",
            description = "Determines if the animation plays at start or not.",
            wuiValueType = typeof(WUIBoolValue),
            get = () => _data.playAtStart,
            set = (object obj) =>
            {
                _data.playAtStart = (bool)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(playAtStart.id, playAtStart);

        SandboxValue loop = new SandboxValue
        {
            module = this,
            id = "Loop",
            description = "Determines if the animation loops or not.",
            wuiValueType = typeof(WUIBoolValue),
            get = () => _data.loop,
            set = (object obj) =>
            {
                _data.loop = (bool)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(loop.id, loop);

        SandboxValue duration = new SandboxValue
        {
            module = this,
            id = "Duration",
            description = "The duration of the animation.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.duration,
            set = (object obj) =>
            {
                _data.duration = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(duration.id, duration);

        SandboxValue sandboxValueReference = new SandboxValue
        {
            module = this,
            id = "Animated Value",
            description = "The value that is to be animated.",
            wuiValueType = typeof(WUISandboxValueReference),
            get = () => _data.sandboxValueReference,
            set = (object obj) =>
            {
                _data.sandboxValueReference = (SandboxValueReference)obj;
                ApplyData();
            },
            meta = new object[] { this, new List<Type>{ typeof(WUIRangeValue), typeof(WUIFloatValue), typeof(WUIIntergerValue)} }
        };
        sandboxValuesById.Add(sandboxValueReference.id, sandboxValueReference);

        SandboxValue value = new SandboxValue
        {
            module = this,
            id = "Scaled Value",
            description = "The value scaled and applied to the animated value.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.value,
            set = (object obj) =>
            {
                _data.value = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(value.id, value);

        SandboxValue scaleOverDuration = new SandboxValue
        {
            module = this,
            id = "Scale Over Animation",
            description = "The scale of the value over the duration of the animation.",
            wuiValueType = typeof(WUICurveValue),
            get = () => _data.serializableCurve,
            set = (object obj) =>
            {
                _data.serializableCurve = (SerializableCurve)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(scaleOverDuration.id, scaleOverDuration);
    }
    protected override void RegisterSandboxEvents()
    {
        base.RegisterSandboxEvents();


        SandboxEvent play = new SandboxEvent
        {
            module = this,
            id = "Play",
            onEventCall = (x) => { Play(); }
        };
        sandboxEventsById.Add(play.id, play);

        SandboxEvent pause = new SandboxEvent
        {
            module = this,
            id = "Pause",
            onEventCall = (x) => { Pause(); }
        };
        sandboxEventsById.Add(pause.id, pause);
    }
    protected override void RegisterSandboxCalls()
    {
        base.RegisterSandboxCalls();
    }

    internal override void AddConnection(Connection connection)
    {
        base.AddConnection(connection);
        if (connection.moduleA == this && !_data.connections.Contains(connection.moduleB.data.id))
            _data.connections.Add(connection.moduleB.data.id);
    }
    internal override void BreakConnection(Connection connection)
    {
        base.BreakConnection(connection);
        Module module = connection.moduleA == this ? connection.moduleB : connection.moduleA;
        _data.connections.Remove(module.data.id);
        if(_data.sandboxValueReference.moduleID == module.data.id)
        {
            _data.sandboxValueReference.moduleID = 0;
            _data.sandboxValueReference.valueID = null;
        }
    }
    internal override void BreakConnectionTo(Module module)
    {
        base.BreakConnectionTo(module);
        _data.connections.Remove(module.data.id);
        if (_data.sandboxValueReference.moduleID == module.data.id)
        {
            _data.sandboxValueReference.moduleID = 0;
            _data.sandboxValueReference.valueID = null;
        }
    }
    private void DisplayConnections()
    {
        foreach (int connectionId in _data.connections)
        {
            Module module = parent.GetModuleByIdRecursive(connectionId);
            parent.Connect(this, module, ConnectionType.Link);
        }
    }




    private void Play()
    {
        if (_sandboxValue == null)
        {
            _sandboxValue = GetSandboxValue();
            if (_sandboxValue == null)
                return;
        }
        _playing = true;
        _progress = 0;
    }
    private void Pause()
    {
        _playing = false;
    }

    private void UpdateAnimation()
    {
        if (!_playing)
            return;
        _progress += Time.deltaTime;
        float completionRatio = _progress / _data.duration;

        _sandboxValue.set(_data.serializableCurve.Evaluate(completionRatio).magnitude * _data.value);

        if (_progress >= _data.duration)
        {
            if (_data.loop)
                Play();
            else
                Pause();
        }
    }
    private SandboxValue GetSandboxValue()
    {
       Module module = parent.GetModuleByIdRecursive(_data.sandboxValueReference.moduleID);
        return module.sandboxValuesById[_data.sandboxValueReference.valueID];
    }
}