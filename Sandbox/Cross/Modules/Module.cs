using System;
using System.Collections.Generic;
using UnityEngine;

/*

Q: How Do I add a new module?

A: 
- Editable head should reference the type in GetModulePrefab
- Editable head should add it in DefinePrefabCollections
- The field data needs to reference a data field of the modules data type AND MUST have a default (new [ModuleType] Data()) -- data => _data ... _data = new [INSERT TYPE HERE <3]Data();
- The data type should include a default data set
- Populate data needs to call apply data
 */

[Serializable]
internal class SerializableVector2
{
    public SerializableVector2(Vector2 vector2)
    {
        x = vector2.x;
        y = vector2.y;
    }

    internal Vector2 vector2
    {
        get
        {
            return new Vector2(x, y);
        }
        set
        {
            x = value.x;
            y = value.y;
        }
    }

    internal float x = 0f;
    internal float y = 0f;
}
[Serializable]
internal class SerializableVector3
{
    public SerializableVector3(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }

    internal Vector3 vector3
    {
        get
        {
            return new Vector3(x, y, z);
        }
        set
        {
            x = value.x;
            y = value.y;
            z = value.z;
        }
    }

    internal float x = 0f;
    internal float y = 0f;
    internal float z = 0f;
}
[Serializable]
internal class SerializableColor
{
    public SerializableColor() { }
    public SerializableColor(Color color)
    {
        r = color.r;
        g = color.g;
        b = color.b;
        a = color.a;
    }
    internal Color color
    {
        get
        {
            return new Color(r, g, b, a);
        }
        set
        {
            r = value.r;
            g = value.g;
            b = value.b;
            a = value.a;
        }
    }

    internal float r = 1f;
    internal float g = 1f;
    internal float b = 1f;
    internal float a = 1f;
}

[Serializable]
internal class SerializableGradient
{
    internal SerializableColor left;
    internal SerializableColor right;

    internal SerializableGradient() { }
    internal SerializableGradient(Color left, Color right)
    {
        this.left = new SerializableColor(left);
        this.right = new SerializableColor(right);
    }
    internal SerializableGradient(SerializableGradient serializableGradient)
    {
        if(serializableGradient == null)
        {
            left = new SerializableColor(Color.white);
            right = new SerializableColor(Color.black);
            return;
        }
        left = new SerializableColor(serializableGradient.left.color);
        right = new SerializableColor(serializableGradient.right.color);
    }


    internal Color Evaluate(float ratio)
    {
        return Color.Lerp(left.color, right.color, ratio);
    }
}

[Serializable]
internal class SerializableCurve
{
    internal SerializableVector2 leftValue;
    internal SerializableVector2 leftTangent;

    internal SerializableVector2 rightTangent;
    internal SerializableVector2 rightValue;

    internal AnimationCurve curve
    {
        get
        {
            AnimationCurve animationCurve = new AnimationCurve();
            for (int i = 0; i < 60; i++)
            {
                float ratio = i / (float)60;
                animationCurve.AddKey(ratio, Evaluate(ratio).y);
            }
            return animationCurve;
        }
    }

    internal SerializableCurve() { }
    internal SerializableCurve(Vector2 leftValue, Vector2 leftTangent, Vector2 rightTangent, Vector2 rightValue)
    {
        this.leftValue = new SerializableVector2(leftValue);
        this.leftTangent = new SerializableVector2(leftTangent);
        this.rightTangent = new SerializableVector2(rightTangent);
        this.rightValue = new SerializableVector2(rightValue);
    }
    internal SerializableCurve(SerializableCurve serializableCurve)
    {
        if(serializableCurve == null)
        {
            leftValue = new SerializableVector2(new Vector2(0, 0));
            leftTangent = new SerializableVector2(new Vector2(0.25f, 0.25f));
            rightValue = new SerializableVector2(new Vector2(1f, 1f));
            rightTangent = new SerializableVector2(new Vector2(0.75f, 0.75f));
            return;
        }
        leftValue = serializableCurve.leftValue;
        leftTangent = new SerializableVector2(serializableCurve.leftTangent.vector2);

        rightTangent = new SerializableVector2(serializableCurve.rightTangent.vector2);
        rightValue = serializableCurve.rightValue;

    }


    internal Vector3 Evaluate(float ratio)
    {
        return CalculateCubicBezierPoint(ratio, leftValue.vector2, leftTangent.vector2, rightTangent.vector2, rightValue.vector2);
    }
   private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}

[Serializable]
internal class SandboxValueReference
{
    internal SandboxValueReference() { }
    internal SandboxValueReference(SandboxValueReference sandboxValueReference)
    {
        this.moduleID = sandboxValueReference.moduleID;
        this.valueID = sandboxValueReference.valueID;
    }


    internal int moduleID;
    internal string valueID;
}

[Serializable]
internal abstract class ModuleData
{
    [SerializeField]
    internal int id;
    [SerializeField]
    internal string name;

    internal abstract ModuleData defaultData { get; }

    internal Vector3 position
    {
        get
        {
            return new Vector3(serializablePosition.x, serializablePosition.y, serializablePosition.z);
        }
        set
        {
            serializablePosition = new SerializableVector3(value);
        }
    }
    internal SerializableVector3 serializablePosition = new SerializableVector3(Vector3.zero);

    internal Vector3 rotation
    {
        get
        {
            return new Vector3(serializableRotation.x, serializableRotation.y, serializableRotation.z);
        }
        set
        {
            serializableRotation = new SerializableVector3(value);
        }
    }
    internal SerializableVector3 serializableRotation = new SerializableVector3(Vector3.zero);

    internal Vector3 scale
    {
        get
        {
            return new Vector3(serializableScale.x, serializableScale.y, serializableScale.z);
        }
        set
        {
            serializableScale = new SerializableVector3(value);
        }
    }
    internal SerializableVector3 serializableScale = new SerializableVector3(Vector3.one);

    [SerializeField]
    internal int parentId = -1;
    [SerializeField]
    internal List<string> tags = new List<string>();

    internal abstract ModuleData Clone();
}

internal enum SandboxMemberType
{
    Event,
    Value,
    Action
}

internal abstract class SandboxMember
{
    internal string id;
    internal Module module;
    internal string description;
}
internal sealed class SandboxValue : SandboxMember
{
    internal Type wuiValueType;
    internal Action<object> set;
    internal Func<object> get;

    internal object[] meta;

    internal string header;
    internal float spaceAfter;

    internal bool hideInScripter;
    internal bool hideInInspector;

}
internal sealed class SandboxEvent : SandboxMember
{
    internal List<Type> parameterTypes;
    internal Action<List<SandboxValue>> onEventCall;
    internal void Invoke(List<SandboxValue> parameters)
    {
        onEventCall?.Invoke(parameters);
    }
    internal void Invoke()
    {
        onEventCall?.Invoke(null);
    }
}
internal sealed class SandboxCall : SandboxMember
{
    internal List<Type> parameterTypes;
    internal Action<List<SandboxValue>> action;
    internal void Invoke(List<SandboxValue> parameters)
    {
        action.Invoke(parameters);
    }
}

internal enum ModuleCategory
{
    Hidden,
    Scripting,
    Art,
    Mechanical,
    HUD,
    Environment
}
internal enum PalleteVisibility
{
    Both,
    Object,
    Map
}

[Serializable]
internal class InspectionData
{
    [SerializeField]
    internal ModuleCategory category;
    [SerializeField]
    internal PalleteVisibility palleteVisibility;
    [SerializeField]
    internal Color color;
    [SerializeField]
    internal Sprite icon;
    [SerializeField]
    internal bool forceHideAtRuntime;
    [SerializeField]
    internal bool forceNoCollisionAtRuntime;
}
internal abstract class Module : MonoBehaviour
{
    #region Values
    internal User user;
    internal Editable parent;
    internal bool passive;

    [SerializeField]
    internal InspectionData inspectionData;

    internal abstract ModuleData data { get; }

    internal List<Connection> connections = new List<Connection>();

    internal Dictionary<string, SandboxCall> sandboxCallsById = new Dictionary<string, SandboxCall>();
    internal Dictionary<string, SandboxEvent> sandboxEventsById = new Dictionary<string, SandboxEvent>();
    internal Dictionary<string, SandboxValue> sandboxValuesById = new Dictionary<string, SandboxValue>();
    #endregion

    #region Cross Functions
    internal abstract void PopulateData(ModuleData moduleData);
    protected virtual void ApplyData()
    {
        base.gameObject.name = data.name;
        base.transform.localPosition = data.position;
        base.transform.localEulerAngles = data.rotation;
        base.transform.localScale = data.scale;
    }
    internal virtual void OnLoad()
    {
        RegisterSandboxValues();
        RegisterSandboxCalls();
        RegisterSandboxEvents();

        if (GameHead.instance.gameState == GameState.Editor)
            OnEditorLoad();
        else  if (GameHead.instance.isPreviewOrRuntime)
            OnRuntimeLoad();
    }

    internal virtual void SetParent(Module module)
    {
        if (module == null)
        {
            data.parentId = -1;
            return;
        }
        if (data.parentId != -1)
        {
            Module parentModule = parent.GetModuleByIdRecursive(data.parentId);
            BreakConnectionTo(parentModule);
        }
        data.parentId = module.data.id;
    }

    private void Start()
    {
        if (GameHead.instance.isPreviewOrRuntime)
            StartRuntime();
        if (GameHead.instance.gameState == GameState.Editor)
            StartEditor();
    }
    private void Update()
    {
        if (GameHead.instance.isPreviewOrRuntime)
            UpdateRuntime();
        if (GameHead.instance.gameState == GameState.Editor)
            UpdateEditor();
    }
    private  void LateUpdate()
    {
        if (GameHead.instance.isPreviewOrRuntime)
            LateUpdateRuntime();
        if (GameHead.instance.gameState == GameState.Editor)
            LateUpdateEditor();
    }
    private  void FixedUpdate()
    {
        if (GameHead.instance.isPreviewOrRuntime)
            FixedUpdateRuntime();
        if (GameHead.instance.gameState == GameState.Editor)
            FixedUpdateEditor();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(!(this is SandboxObject))
        {
            if (parent != null)
                parent.OnCollisionEnter(collision);
        }
    }
    protected virtual void OnCollisionStay(Collision collision)
    {
        if (!(this is SandboxObject))
        {
            if (parent != null)
                parent.OnCollisionStay(collision);
        }
    }
    protected virtual void OnCollisionExit(Collision collision)
    {
        if (!(this is SandboxObject))
        {
            if (parent != null)
                parent.OnCollisionExit(collision);
        }
    }

    #region Connections
    internal virtual void AddConnection(Connection connection)
    {
        connections.Add(connection);
    }

    internal virtual void BreakConnection(Connection connection)
    {
        connections.Remove(connection);

        if (data.parentId != -1)
        {
            Module parentModule = parent.GetModuleByIdRecursive(data.parentId);
            connection.IsConnectedTo(parentModule);
            data.parentId = -1;
        }
    }
    internal virtual void BreakConnectionTo(Module module)
    {
        foreach (Connection connection in connections)
        {
            if (connection.IsConnectedTo(module))
            {
                connection.Break();
                return;
            }
        }
    }
    protected virtual void BreakAllConnections()
    {
        foreach (Connection connection in connections)
        {
            connection.Break();
            BreakAllConnections();
            return;
        }
    }

    protected virtual void RefreshConnections()
    {
        foreach (Connection connection in connections)
            connection.RefreshLine();
    }
    internal bool IsConnectedTo(Module module)
    {
        foreach (Connection connection in connections)
        {
            if (connection.IsConnectedTo(module))
                return true;
        }
        return false;
    }

    private void DisplayWeld()
    {
        if (data.parentId == -1)
            return;

        Module module = parent.GetModuleByIdRecursive(data.parentId);
        parent.Connect(this, module, ConnectionType.Weld);
    }
    #endregion
    #endregion


    protected virtual void RegisterSandboxValues()
    {
        SandboxValue tags = new SandboxValue
        {
            module = this,
            id = "Tags",
            description = "Tags that identify the module.",
            wuiValueType = typeof(WUITagList),
            get = () => data.tags,
            set = (object obj) =>
            {
                data.tags = (List<string>)obj;
                ApplyData();
            },
            header = "Base Properties",
            meta = new object[] { }
        };
        sandboxValuesById.Add(tags.id, tags);

        SandboxValue position = new SandboxValue
        {
            module = this,
            id = "Position",
            description = "The position of the module.",
            wuiValueType = typeof(WUIVectorValue),
            get = () => data.position,
            set = (object obj) =>
            {
                data.position = (Vector3)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(position.id, position);

        SandboxValue rotation = new SandboxValue
        {
            module = this,
            id = "Rotation",
            description = "The rotation of the module.",
            wuiValueType = typeof(WUIVectorValue),
            get = () => data.rotation,
            set = (object obj) =>
            {
                data.rotation = (Vector3)obj;
                ApplyData();
            },
            meta = new object[] { }
        };
        sandboxValuesById.Add(rotation.id, rotation);

        SandboxValue scale = new SandboxValue
        {
            module = this,
            id = "Scale",
            description = "The scale of the module.",
            wuiValueType = typeof(WUIVectorValue),
            get = () => data.scale,
            set = (object obj) =>
            {
                data.scale = (Vector3)obj;
                ApplyData();
            },
            spaceAfter = 5f,
            meta = new object[] { }
        };
        sandboxValuesById.Add(scale.id, scale);
    }
    protected virtual void RegisterSandboxCalls()
    {
        SandboxCall deleteSandboxAction = new SandboxCall
        {
            module = this,
            id = "delete",
            action = (x) => { Delete(); }
        };
        sandboxCallsById.Add(deleteSandboxAction.id, deleteSandboxAction);
    }
    protected virtual void RegisterSandboxEvents()
    {
    }

    #region Editor Functions
    protected virtual void StartEditor()
    {
    }
    protected virtual void UpdateEditor()
    {
    }
    protected virtual void LateUpdateEditor()
    {
    }
    protected virtual void FixedUpdateEditor()
    {
    }
    protected virtual void OnEditorLoad()
    {
        if (passive)
            return;
        DisplayWeld();
    }
    internal virtual void OnSave()
    {
    }

    internal void OnTransformed()
    {
        data.position = base.transform.position;
        data.rotation = base.transform.eulerAngles;
        data.scale = base.transform.localScale;
        RefreshConnections();
    }
    internal virtual void Delete()
    {
        parent.UnregisterModule(this);
        Destroy(base.gameObject);
        BreakAllConnections();
    }
    #endregion

    #region Runtime Functions
    protected virtual void OnRuntimeLoad()
    {
        DisplayWeld();
    }
    protected virtual void StartRuntime()
    {
    }
    protected virtual void UpdateRuntime()
    {
    }
    protected virtual void LateUpdateRuntime()
    {
    }
    protected virtual void FixedUpdateRuntime()
    {
    }

    internal abstract bool OnAttach(User user);
    internal virtual void OnDetach()
    {
        this.user = null;
    }

    internal void InvokeCall(string id)
    {
        sandboxCallsById[id].Invoke(null);
    }
    internal void InvokeEvent(string id)
    {
        sandboxEventsById[id].Invoke();
    }
    #endregion
}