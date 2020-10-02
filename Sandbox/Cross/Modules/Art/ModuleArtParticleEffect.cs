using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum SimulationSpace
{
    World,
    Local
}

internal enum ParticleSystemShape
{
    Sphere,
    HemiSphere,
    Donut,
    Line
}

[Serializable]
internal sealed class ModuleArtParticleEffectData : ModuleArtData
{
    internal override ModuleData defaultData
    {
        get
        {
            return new ModuleArtParticleEffectData
            {
                name = "Particle Effect",
                position = Vector3.zero,
                scale = Vector3.one,
                rotation = Vector3.zero,

                colorOverDuration = new SerializableGradient(Color.gray, Color.yellow),
                effectTextureIndex = 0,

                simulationSpace = SimulationSpace.World,
                shape = ParticleSystemShape.Sphere,

                loop = true,
                duration = 1f,
                sizeOverLifetime = new SerializableCurve(Vector2.zero, Vector2.one * 0.25f, Vector2.one * 0.75f, Vector2.one),
                sizeMultiplier = 1f,


                maxParticles = 20,
                gravity = 1f,
                speed = 2f,
                emissionRate = 50f,

                rotationMin = 0,
                rotationMax = 90,
                particleLifetimeMin = 0.3f,
                particleLifetimeMax = 0.5f,
                shapeScale = 1f
            };
        }
    }
    public ModuleArtParticleEffectData() { }
    public ModuleArtParticleEffectData(ModuleArtParticleEffectData cosmeticParticleEffectData)
    {
        id = cosmeticParticleEffectData.id;
        parentId = cosmeticParticleEffectData.parentId;

        name = cosmeticParticleEffectData.name;
        position = cosmeticParticleEffectData.position;
        rotation = cosmeticParticleEffectData.rotation;
        scale = cosmeticParticleEffectData.scale;
        tags = cosmeticParticleEffectData.tags;

        shapeScale = cosmeticParticleEffectData.shapeScale;
        sizeMultiplier = cosmeticParticleEffectData.sizeMultiplier;
        colorOverDuration = new SerializableGradient(cosmeticParticleEffectData.colorOverDuration);
        effectTextureIndex = cosmeticParticleEffectData.effectTextureIndex;
        simulationSpace = cosmeticParticleEffectData.simulationSpace;
        duration = cosmeticParticleEffectData.duration;
        particleLifetimeMin = cosmeticParticleEffectData.particleLifetimeMin;
        particleLifetimeMax = cosmeticParticleEffectData.particleLifetimeMax;
        loop = cosmeticParticleEffectData.loop;
        gravity = cosmeticParticleEffectData.gravity;
        speed = cosmeticParticleEffectData.speed;
        maxParticles = cosmeticParticleEffectData.maxParticles;
        emissionRate = cosmeticParticleEffectData.emissionRate;
        shape = cosmeticParticleEffectData.shape;

        rotationMin = cosmeticParticleEffectData.rotationMin;
        rotationMax = cosmeticParticleEffectData.rotationMax;

        if (cosmeticParticleEffectData.sizeOverLifetime == null)
            sizeOverLifetime = new SerializableCurve(Vector2.zero, Vector2.one * 0.25f, Vector2.one * 0.75f, Vector2.one);
        else
            sizeOverLifetime = cosmeticParticleEffectData.sizeOverLifetime;
    }


    internal SerializableGradient colorOverDuration;

    internal SerializableCurve sizeOverLifetime;
    internal float sizeMultiplier;
    internal int effectTextureIndex;
    internal SimulationSpace simulationSpace;
    internal ParticleSystemShape shape;

    internal float shapeScale;

    internal float duration;
    internal float particleLifetimeMax;
    internal float particleLifetimeMin;
    internal bool loop;
    internal float gravity;
    internal float speed;
    internal int maxParticles;
    internal float emissionRate;

    internal float rotationMin;
    internal float rotationMax;

    internal override ModuleData Clone()
    {
        return new ModuleArtParticleEffectData(this);
    }
}

internal sealed class ModuleArtParticleEffect : ModuleArt
{
    internal ModuleArtParticleEffectData _data = new ModuleArtParticleEffectData();
    internal override ModuleData data
    {
        get
        {
            return _data;
        }
    }

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal ParticleSystem particleSystem;
        [SerializeField]
        internal ParticleSystemRenderer particleSystemRenderer;
    }
    [SerializeField]
    private Components _components;


    internal override void PopulateData(ModuleData objectData)
    {
        _data = new ModuleArtParticleEffectData((ModuleArtParticleEffectData)objectData);
        ApplyData();
    }

    protected override void ApplyData()
    {
        base.ApplyData();

        if (GameHead.instance.gameState == GameState.Editor)
            _components.particleSystem.Stop();

        Texture2D texture2D = TextureHead.instance.GetEffect(_data.effectTextureIndex);
        Material material = MaterialHead.instance.GetEffectMaterial(texture2D);
        _components.particleSystemRenderer.sharedMaterial = material;


        ParticleSystem.MainModule mainModule = _components.particleSystem.main;
        mainModule.simulationSpace = _data.simulationSpace == SimulationSpace.Local ? ParticleSystemSimulationSpace.Local : ParticleSystemSimulationSpace.World;
        mainModule.startLifetime = new ParticleSystem.MinMaxCurve(_data.particleLifetimeMin, _data.particleLifetimeMax);
        mainModule.duration = _data.duration;
        mainModule.loop = _data.loop;
        mainModule.gravityModifierMultiplier = _data.gravity;
        mainModule.maxParticles = _data.maxParticles;
        mainModule.startSpeed = _data.speed;
        mainModule.startRotation = new ParticleSystem.MinMaxCurve(_data.rotationMin, _data.rotationMax);


        ParticleSystem.ShapeModule shapeModule = _components.particleSystem.shape;
        shapeModule.scale = Vector3.one * _data.shapeScale;
        shapeModule.donutRadius = _data.shapeScale;
        switch (_data.shape)
        {
            case ParticleSystemShape.Sphere:
                shapeModule.shapeType = ParticleSystemShapeType.Sphere;
                break;
            case ParticleSystemShape.HemiSphere:
                shapeModule.shapeType = ParticleSystemShapeType.Hemisphere;
                break;
            case ParticleSystemShape.Donut:
                shapeModule.shapeType = ParticleSystemShapeType.Donut;
                break;
            case ParticleSystemShape.Line:
                shapeModule.shapeType = ParticleSystemShapeType.SingleSidedEdge;
                break;
        }


        ParticleSystem.EmissionModule emissionModule = _components.particleSystem.emission;
        emissionModule.rateOverTimeMultiplier = _data.emissionRate;


        ParticleSystem.SizeOverLifetimeModule sizeOverLifetimeModule = _components.particleSystem.sizeOverLifetime;
        sizeOverLifetimeModule.enabled = true;
        sizeOverLifetimeModule.size = new ParticleSystem.MinMaxCurve(_data.sizeMultiplier, _data.sizeOverLifetime.curve);


        ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = _components.particleSystem.colorOverLifetime;
        colorOverLifetimeModule.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(_data.colorOverDuration.left.color, 0.0f), new GradientColorKey(_data.colorOverDuration.right.color, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(_data.colorOverDuration.left.color.a, 0.0f), new GradientAlphaKey(_data.colorOverDuration.right.color.a, 1.0f) });
        colorOverLifetimeModule.color = gradient;


        if(GameHead.instance.gameState == GameState.Editor)
            _components.particleSystem.Play();
    }

    protected override void RegisterSandboxCalls()
    {
        base.RegisterSandboxCalls();
    }
    protected override void RegisterSandboxValues()
    {
        base.RegisterSandboxValues();


        SandboxValue duration = new SandboxValue
        {
            module = this,
            id = "Duration",
            description = "The duration the effect will play if not looped.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.duration,
            set = (object obj) =>
            {
                _data.duration = (float)obj;
                ApplyData();
            },
            header = "Emission Properties",
            meta = new object[] { }
        };
        sandboxValuesById.Add(duration.id, duration);
        SandboxValue loop = new SandboxValue
        {
            module = this,
            id = "Loop",
            description = "Determines if the particle effect loops playback.",
            wuiValueType = typeof(WUIBoolValue),
            get = () => _data.loop,
            set = (object obj) =>
            {
                _data.loop = (bool)obj;
                ApplyData();
            },
            spaceAfter = 5f,
            meta = new object[] { }
        };
        sandboxValuesById.Add(loop.id, loop);
        SandboxValue simulationSpace = new SandboxValue
        {
            module = this,
            id = "Simulation Space",
            description = "The space in-which the emission is simulated.",
            wuiValueType = typeof(WUIEnumValue),
            get = () => _data.simulationSpace,
            set = (object obj) =>
            {
                _data.simulationSpace = (SimulationSpace)obj;
                ApplyData();
            },
            meta = new object[] { typeof(SimulationSpace) }
        };
        sandboxValuesById.Add(simulationSpace.id, simulationSpace);


        SandboxValue shape = new SandboxValue
        {
            module = this,
            id = "Shape",
            description = "The shape of the emission volume.",
            wuiValueType = typeof(WUIEnumValue),
            get = () => _data.shape,
            set = (object obj) =>
            {
                _data.shape = (ParticleSystemShape)obj;
                ApplyData();
            },
            meta = new object[] {typeof(ParticleSystemShape)}
        };
        sandboxValuesById.Add(shape.id, shape);
        SandboxValue shapeScale = new SandboxValue
        {
            module = this,
            id = "Shape Scale",
            description = "The scale of the emission shape.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.shapeScale,
            set = (object obj) =>
            {
                _data.shapeScale = (float)obj;
                ApplyData();
            },
            spaceAfter = 5f,
            meta = new object[] { }
        };
        sandboxValuesById.Add(shapeScale.id, shapeScale);
        SandboxValue maxParticles = new SandboxValue
        {
            module = this,
            id = "Max Particles",
            description = "The maximum amount of particles at any one time.",
            wuiValueType = typeof(WUIIntergerValue),
            get = () => _data.maxParticles,
            set = (object obj) =>
            {
                _data.maxParticles = (int)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(maxParticles.id, maxParticles);
        SandboxValue emissionRate = new SandboxValue
        {
            module = this,
            id = "Emission Rate",
            description = "The rate at which particles emit.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.emissionRate,
            set = (object obj) =>
            {
                _data.emissionRate = (float)obj;
                ApplyData();
            },
            spaceAfter = 10f,
            meta = new object[] { }
        };
        sandboxValuesById.Add(emissionRate.id, emissionRate);


        SandboxValue texture = new SandboxValue
        {
            module = this,
            id = "Image",
            description = "The image of each particle.",
            wuiValueType = typeof(WUITextureReferenceValue),
            get = () => _data.effectTextureIndex,
            set = (object obj) =>
            {
                _data.effectTextureIndex = (int)obj;
                ApplyData();
            },
            header = "Particle Properties",
            meta = new object[] { TextureSet.Effects }
        };
        sandboxValuesById.Add(texture.id, texture);
        SandboxValue color = new SandboxValue
        {
            module = this,
            id = "Color Over Lifetime",
            description = "The color of each particle over its liftime.",
            wuiValueType = typeof(WUIGradientValue),
            get = () => _data.colorOverDuration,
            set = (object obj) =>
            {
                _data.colorOverDuration = (SerializableGradient)obj;
                ApplyData();
            },
            spaceAfter = 5f,
            meta = new object[] { }
        };
        sandboxValuesById.Add(color.id, color);

        SandboxValue sizeMultiplier = new SandboxValue
        {
            module = this,
            id = "Size Multiplier",
            description = "The multiplication of size over life time.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.sizeMultiplier,
            set = (object obj) =>
            {
                _data.sizeMultiplier = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(sizeMultiplier.id, sizeMultiplier);
        SandboxValue sizeOverLifetime = new SandboxValue
        {
            module = this,
            id = "Size Over Lifetime",
            description = "The size of each particle over its liftime.",
            wuiValueType = typeof(WUICurveValue),
            get = () => _data.sizeOverLifetime,
            set = (object obj) =>
            {
                _data.sizeOverLifetime = (SerializableCurve)obj;
                ApplyData();
            },
            spaceAfter = 5f,
            meta = new object[] { }
        };
        sandboxValuesById.Add(sizeOverLifetime.id, sizeOverLifetime);



        SandboxValue speed = new SandboxValue
        {
            module = this,
            id = "Speed",
            description = "The speed applied to each particle.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.speed,
            set = (object obj) =>
            {
                _data.speed = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(speed.id, speed);
        SandboxValue gravity = new SandboxValue
        {
            module = this,
            id = "Gravity",
            description = "The gravity applied to each particle.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.gravity,
            set = (object obj) =>
            {
                _data.gravity = (float)obj;
                ApplyData();
            },
            spaceAfter = 5f,
            meta = new object[] { }
        };
        sandboxValuesById.Add(gravity.id, gravity);

        SandboxValue particleLifetimeMin = new SandboxValue
        {
            module = this,
            id = "Minimum Particle Lifetime",
            description = "The minimum lifetime of each particle.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.particleLifetimeMin,
            set = (object obj) =>
            {
                _data.particleLifetimeMin = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(particleLifetimeMin.id, particleLifetimeMin);
        SandboxValue particleLifetimeMax = new SandboxValue
        {
            module = this,
            id = "Maximum Particle Lifetime",
            description = "The maximum lifetime of each particle.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.particleLifetimeMax,
            set = (object obj) =>
            {
                _data.particleLifetimeMax = (float)obj;
                ApplyData();
            },
            spaceAfter = 5f,
            meta = new object[] { }
        };
        sandboxValuesById.Add(particleLifetimeMax.id, particleLifetimeMax);

        SandboxValue rotationMin = new SandboxValue
        {
            module = this,
            id = "Minimum Rotation",
            description = "The minimum rotation of each particle.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.rotationMin,
            set = (object obj) =>
            {
                _data.rotationMin = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(rotationMin.id, rotationMin);
        SandboxValue rotationMax = new SandboxValue
        {
            module = this,
            id = "Maximum Rotation",
            description = "The maximum rotation of each particle.",
            wuiValueType = typeof(WUIFloatValue),
            get = () => _data.rotationMax,
            set = (object obj) =>
            {
                _data.rotationMax = (float)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(rotationMax.id, rotationMax);
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

        SandboxEvent stop = new SandboxEvent
        {
            module = this,
            id = "Stop",
            onEventCall = (x) => { Stop(); }
        };
        sandboxEventsById.Add(stop.id, stop);
    }

    private void Play()
    {
        _components.particleSystem.Play();
    }
    private void Pause()
    {
        _components.particleSystem.Pause();
    }

    private void Stop()
    {
        _components.particleSystem.Stop();
    }

    internal override bool OnAttach(User user)
    {
        return false;
    }
}