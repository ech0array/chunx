using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class ModuleLogicColorAnimatorData : ModuleArtData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleLogicColorAnimatorData
            {
                name = "Color Animator",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,
                loop = false,
                duration = 1f,
                serializableGradient = new SerializableGradient(Color.white, Color.black),
                sandboxValueReference = new SandboxValueReference()
            };
        }
    }
    public ModuleLogicColorAnimatorData() { }
    public ModuleLogicColorAnimatorData(ModuleLogicColorAnimatorData moduleColorAnimatorData)
    {
        id = moduleColorAnimatorData.id;
        parentId = moduleColorAnimatorData.parentId;

        name = moduleColorAnimatorData.name;
        position = moduleColorAnimatorData.position;
        rotation = moduleColorAnimatorData.rotation;
        scale = moduleColorAnimatorData.scale;
        tags = moduleColorAnimatorData.tags;
        if(moduleColorAnimatorData.connections != null)
            connections = new List<int>(moduleColorAnimatorData.connections);

        duration = moduleColorAnimatorData.duration;
        loop = moduleColorAnimatorData.loop;
        serializableGradient = new SerializableGradient(moduleColorAnimatorData.serializableGradient);
        sandboxValueReference = new SandboxValueReference(moduleColorAnimatorData.sandboxValueReference);
    }


    internal List<int> connections = new List<int>();

    internal float duration;
    internal bool loop;
    internal SandboxValueReference sandboxValueReference;
    internal SerializableGradient serializableGradient;

    internal override ModuleData Clone()
    {
        return new ModuleLogicColorAnimatorData(this);
    }
}

internal sealed class ModuleLogicColorAnimator : ModuleArt
{
    internal ModuleLogicColorAnimatorData _data = new ModuleLogicColorAnimatorData();
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

    private void OnDestroy()
    {
        GameHead.instance.SetSunState(true);
    } 
    #endregion

    internal override void PopulateData(ModuleData objectData)
    {
        _data = new ModuleLogicColorAnimatorData((ModuleLogicColorAnimatorData)objectData);
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
        return false;
    }

    protected override void RegisterSandboxValues()
    {
        //SandboxValue test = new SandboxValue
        //{
        //    module = this,
        //    id = "TEST PLAY",
        //    description = "---",
        //    wuiValueType = typeof(WUIButtonValue),
        //    get = () => null,
        //    set = (object obj) =>
        //    {
        //    },
        //    meta = new object[] { new Action(Play)}
        //};
        //sandboxValues.Add(test);

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
            meta = new object[] { this, new List<Type>{ typeof(WUIColorValue)} }
        };
        sandboxValuesById.Add(sandboxValueReference.id, sandboxValueReference);

        SandboxValue scaleOverDuration = new SandboxValue
        {
            module = this,
            id = "Color Over Animation",
            description = "The color of the value over the duration of the animation.",
            wuiValueType = typeof(WUIGradientValue),
            get = () => _data.serializableGradient,
            set = (object obj) =>
            {
                _data.serializableGradient = (SerializableGradient)obj;
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
        if (_data.sandboxValueReference.moduleID == module.data.id)
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

        _sandboxValue.set(_data.serializableGradient.Evaluate(completionRatio));
        if(_progress >= _data.duration)
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