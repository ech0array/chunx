using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *Data population cycle
 *  Interpret node
 *  Fulfill Node data
 *  
 */

internal abstract class LoomNode
{
    internal int id;
    internal Vector2 position { get; set; }

    internal abstract LoomNodeData Serialize();
}
[Serializable]
internal class LoomNodeData
{
    internal int id;
    internal SerializableVector2 position;
}


internal abstract class LoomField
{
    internal abstract string id { get; set; }
    internal LoomNode node;
    internal abstract SandboxValue GetValue();

    internal abstract LoomFieldData Serialize();
}
[Serializable]
internal class LoomFieldData
{
    internal string valueId;
}


// From an event, read only, outward
internal class LoomParameter : LoomField
{
    protected SandboxValue sandboxValue;

    internal override string id { get => sandboxValue.id; set { } }

    public LoomParameter(SandboxValue sandboxValue)
    {
        this.sandboxValue = sandboxValue;
    }
    internal override SandboxValue GetValue()
    {
        return sandboxValue;
    }


    internal override LoomFieldData Serialize()
    {
        return new LoomParameterData()
        {
            valueId = sandboxValue.id
        };
    }
}
[Serializable]
internal class LoomParameterData : LoomFieldData
{
}


// From a data set, read and write
internal class LoomVariable : LoomParameter
{
    internal override string id { get => base.id; set { } }

    internal LoomField inField;

    public LoomVariable(SandboxValue sandboxValue) : base(sandboxValue)
    {
        this.sandboxValue = sandboxValue;
    }

    internal override SandboxValue GetValue()
    {
        if (inField == null)
            return base.GetValue();
        return inField.GetValue();
    }

    internal void SetData()
    {
        if (inField == null)
            return;
        sandboxValue.set(inField.GetValue().get());
    }

    internal override LoomFieldData Serialize()
    {
        return new LoomVariableData()
        {
            valueId = base.sandboxValue.id,
            inFieldNodeId = inField == null ? null : (int?)inField.node.id,
            inFieldValueId = inField == null ? null : inField.id
        };
    }
}
[Serializable]
internal class LoomVariableData : LoomFieldData
{
    // The connection to the in field
    internal int? inFieldNodeId;
    internal string inFieldValueId;

}


internal class LoomReference : LoomField
{
    private string identifier;
    internal override string id { get { return identifier; } set { identifier = value; } }

    private LoomField inField;
    internal override SandboxValue GetValue()
    {
        return inField.GetValue();
    }
    internal void Reference(LoomParameter loomParameter)
    {
        this.inField = loomParameter;
    }

    internal override LoomFieldData Serialize()
    {
        return new LoomReferenceData()
        {
            valueId = id,
            inFieldNodeId = inField == null ? null : (int?)inField.node.id,
            inFieldValueId = inField == null ? null : inField.id
        };
    }
}
[Serializable]
internal class LoomReferenceData : LoomVariableData
{

}


internal class LoomCustomVariable : LoomNode
{
    // Data notes:
    // store module id (local link? or null for local?)
    // store member id (custom name? uid?)
    // store value or reference

    internal LoomVariable loomVariable;

    internal override LoomNodeData Serialize()
    {
        throw new NotImplementedException();
    }
}
[Serializable]
internal class LoomCustomVariableData : LoomNodeData
{
    // needs a good bit of time to understand how to save various types
}



internal abstract class LoomExecution : LoomNode
{
    internal abstract void Invoke();
}
internal class LoomModuleData : LoomExecution
{
    internal Module module;
    internal LoomExecution next;
    internal LoomModuleData(List<SandboxValue> sandboxValues)
    {
        module = sandboxValues[0].module;
        foreach (SandboxValue sandboxValue in sandboxValues)
            variables.Add(new LoomVariable(sandboxValue));
    }

    internal List<LoomVariable> variables = new List<LoomVariable>();
    internal override void Invoke()
    {
        // set all values
        foreach (LoomVariable loomVariable in variables)
            loomVariable.SetData();
    }

    internal override LoomNodeData Serialize()
    {
        return new LoomModuleDataData()
        {
            id = base.id,
            position = new SerializableVector2(base.position),
            moduleId = module.data.id,
            nextExecutionNodeId = next == null ? null : (int?)next.id
        };
    }
}
[Serializable]
internal class LoomModuleDataData : LoomNodeData
{
    internal int moduleId;
    internal int? nextExecutionNodeId;
}


internal class LoomEvent : LoomNode
{
    internal Module module;
    internal SandboxEvent sandboxEvent;
    internal LoomExecution next;
    internal List<LoomParameter> parameters = new List<LoomParameter>();
    internal LoomEvent(SandboxEvent sandboxEvent)
    {
        module = sandboxEvent.module;
        this.sandboxEvent = sandboxEvent;
    }


    internal void BindEvent()
    {
        sandboxEvent.onEventCall += Invoke;
    }

    internal void Invoke(List<SandboxValue> parameters = null)
    {
        foreach (SandboxValue sandboxValue in parameters)
            this.parameters.Add(new LoomParameter(sandboxValue));

        next?.Invoke();
    }

    internal override LoomNodeData Serialize()
    {
        return new LoomEventData()
        {
            moduleId = module.data.id,
            sandboxEventId = sandboxEvent.id,
            id = base.id,
            nextExecutionNodeId = next == null ? null : (int?)next.id,
            position = new SerializableVector2(base.position)
        };
    }
}
    [Serializable]
    internal class LoomEventData : LoomNodeData
    {
        internal int moduleId;
        internal string sandboxEventId;

        internal int? nextExecutionNodeId;

        internal List<LoomParameterData> loomParameterDatas;

    }

internal class LoomModuleCall : LoomNode
{
    internal Module module;
    internal SandboxCall sandboxCall;

    internal List<LoomReference> references = new List<LoomReference>();

    internal void Invoke()
    {
        List<SandboxValue> sandboxValues = new List<SandboxValue>();
        foreach (LoomReference loomReference in references)
            sandboxValues.Add(loomReference.GetValue());

        sandboxCall.Invoke(sandboxValues);
    }

    internal override LoomNodeData Serialize()
    {
        List<LoomFieldData> loomReferenceDatas = new List<LoomFieldData>();
        foreach (LoomReference loomReference in references)
        {
            loomReferenceDatas.Add(loomReference.Serialize());
        }

        return new LoomModuleCallData()
        {
            moduleId = sandboxCall.module.data.id,
            sandboxCallId = sandboxCall.id,
            id = base.id,
            position = new SerializableVector2(base.position),
            loomReferenceDatas = loomReferenceDatas
        };
    }
}
[Serializable]
internal class LoomModuleCallData : LoomNodeData
{
    internal int moduleId;
    internal string sandboxCallId;
    internal List<LoomFieldData> loomReferenceDatas;
}

internal class LoomCustomCallIn
{

}
[Serializable]
internal class LoomCustomCallInData : LoomNodeData
{

}
internal class LoomCustomCallOut : LoomExecution
{
    internal override LoomNodeData Serialize()
    {
        throw new NotImplementedException();
    }

    internal override void Invoke()
    {
    }
}
[Serializable]
internal class LoomCustomCallOutData : LoomNodeData
{

}


internal class LoomFork : LoomExecution
{
    internal LoomReference leftHand;
    internal LoomReference rightHand;

    internal enum ConditionType
    {
        Equal,
        GreaterThan,
        LessThan
    }

    internal override void Invoke()
    {
        bool equal = true;

        if (equal)
            onEqual?.Invoke();
        else
            onUnequal?.Invoke();
    }

    internal override LoomNodeData Serialize()
    {
        return new LoomForkData()
        {
            id = base.id,
            position = new SerializableVector2(base.position),
            leftHand = leftHand == null ? null : leftHand.Serialize(),
            rightHand = rightHand == null ? null : rightHand.Serialize(),
            onEqualExecutionId = onEqual == null ? null : (int?)onEqual.id,
            onUnequalExecutionId = onUnequal == null ? null : (int?)onUnequal.id
        };
    }

    internal LoomExecution onEqual;
    internal LoomExecution onUnequal;
}
[Serializable]
internal class LoomForkData : LoomNodeData
{
    internal LoomFieldData leftHand;
    internal LoomFieldData rightHand;
    internal int condition;

    internal int? onEqualExecutionId;
    internal int? onUnequalExecutionId;
}

internal class LoomLoop : LoomExecution
{

    // if ref is list, do foreach, if numeric, do for
    internal LoomReference list;

    internal override void Invoke()
    {
        // set parameter. then invoke step
    }

    internal override LoomNodeData Serialize()
    {
        return new LoomLoopData()
        {
            id = base.id,
            position = new SerializableVector2(base.position),
            list = list == null ? null : list.Serialize(),
            onStepExecutionId = onStep == null ? null : (int?)onStep.id
        };
    }

    internal LoomExecution onStep;
    internal LoomParameter step;
}
[Serializable]
internal class LoomLoopData : LoomNodeData
{
    internal LoomFieldData list;
    internal int? onStepExecutionId;
}

internal class LoomGetModule : LoomExecution
{
    internal LoomReference toy;
    internal LoomReference tag;
    internal LoomReference type;


    internal LoomExecution onSuccess;
    internal LoomParameter module;

    internal LoomExecution onFail;

    internal override LoomNodeData Serialize()
    {
        return new LoomGetModuleData()
        {
            id = base.id,
            position = new SerializableVector2(base.position),
            toy = toy == null ? null : toy.Serialize(),
            tag = tag == null ? null : tag.Serialize(),
            type = toy == null ? null : type.Serialize(),
            onSuccessExecutionId = onSuccess == null ? null : (int?)onSuccess.id,
            onFailExecutionId = onFail == null ? null : (int?)onFail.id
        };
    }

    internal override void Invoke()
    {

    }
}
[Serializable]
internal class LoomGetModuleData : LoomNodeData
{
    internal LoomFieldData toy;
    internal LoomFieldData tag;
    internal LoomFieldData type;

    internal int? onSuccessExecutionId;
    internal int? onFailExecutionId;
}
internal class LoomGetToy : LoomExecution
{
    internal LoomReference tag;


    internal LoomExecution onSuccess;
    internal LoomParameter toy;

    internal LoomExecution onFail;

    internal override LoomNodeData Serialize()
    {
        return new LoomGetToyData()
        {
            id = base.id,
            position = new SerializableVector2(base.position),
            tag = tag == null ? null : tag.Serialize(),
            onSuccessExecutionId = onSuccess == null ? null : (int?)onSuccess.id,
            onFailExecutionId = onFail == null ? null : (int?)onFail.id
        };
    }

    internal override void Invoke()
    {

    }
}
[Serializable]
internal class LoomGetToyData : LoomNodeData
{
    internal LoomFieldData tag;

    internal int? onSuccessExecutionId;
    internal int? onFailExecutionId;
}



internal class LoomMathFunction : LoomNode
{

    internal LoomReference inField;


    internal enum FunctionType
    {
        sin,
        cos,
        abs,
        round
    }
    internal FunctionType functionType;

    internal LoomParameter result
    {
        get;
    }

    internal override LoomNodeData Serialize()
    {
        return new LoomMathFunctionData()
        {
            id = base.id,
            position = new SerializableVector2(base.position),
            inField = inField == null ? null : inField.Serialize(),
            type = (int)functionType
        };
    }
}
[Serializable]
internal class LoomMathFunctionData : LoomNodeData
{
    internal LoomFieldData inField;
    internal int type;
}

internal class LoomMathOperation : LoomNode
{
    internal LoomReference leftHand;
    internal LoomReference rightHand;

    internal enum OperationType
    {
        plus,
        minus,
        times,
        divide
    }
    internal OperationType operationType;

    internal LoomParameter result
    {
        get;
    }

    internal override LoomNodeData Serialize()
    {
        return new LoomMathOperationData()
        {
            id = base.id,
            position = new SerializableVector2(base.position),
            leftHand = leftHand == null ? null : leftHand.Serialize(),
            rightHand = rightHand == null ? null : rightHand.Serialize(),
            type = (int)operationType
        };
    }
}
[Serializable]
internal class LoomMathOperationData : LoomNodeData
{
    internal LoomFieldData leftHand;
    internal LoomFieldData rightHand;
    internal int type;
}

internal class LoomNumericUtility : LoomNode
{
    internal LoomReference a;
    internal LoomReference b;
    internal LoomReference c;


    internal enum UtilityType
    {
        clamp,
        min,
        max
    }
    internal UtilityType utilityType;

    internal LoomParameter result;

    internal override LoomNodeData Serialize()
    {
        return new LoomNumericUtilityData()
        {
            id = base.id,
            position = new SerializableVector2(base.position),
            a = a == null ? null : a.Serialize(),
            b = b == null ? null : b.Serialize(),
            c = c == null ? null : c.Serialize(),
            type = (int)utilityType
        };
    }
    
}
[Serializable]
internal class LoomNumericUtilityData : LoomNodeData
{
    internal LoomFieldData a;
    internal LoomFieldData b;
    internal LoomFieldData c;
    internal int type;
}

