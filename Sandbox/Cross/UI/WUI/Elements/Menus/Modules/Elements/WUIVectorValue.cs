using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

sealed internal class WUIVectorValue : WUIValue
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal TMP_InputField xInputField;
        [SerializeField]
        internal TMP_InputField yInputField;
        [SerializeField]
        internal TMP_InputField zInputField;
    }
    [SerializeField]
    private Components _components;

    internal Action<Vector3> onValueChanged;

    private void Awake()
    {
        BindEvents();
    }

    internal override void Bind(SandboxValue sandboxValue)
    {
        base.valueReference = sandboxValue;
        object vector = sandboxValue.get();

        if(vector is Vector3)
        {
            Vector3 vector3 = (Vector3)vector;
            _components.xInputField.text = vector3.x.ToString();
            _components.yInputField.text = vector3.y.ToString();
            _components.zInputField.text = vector3.z.ToString();
        }
        else if (vector is Vector2)
        {
            Vector2 vector2 = (Vector2)vector;
            _components.xInputField.text = vector2.x.ToString();
            _components.yInputField.text = vector2.y.ToString();
            _components.zInputField.gameObject.SetActive(false);
        }

        wUITipable.Populate(sandboxValue.id, sandboxValue.description);
    }

    private void BindEvents()
    {
        _components.xInputField.onValueChanged.AddListener((string text) => { SetX(float.Parse(text)); });
        _components.yInputField.onValueChanged.AddListener((string text) => { SetY(float.Parse(text)); });
        _components.zInputField.onValueChanged.AddListener((string text) => { SetZ(float.Parse(text)); });
    }

    private void SetX(float value)
    {
        object vector = base.valueReference.get();

        if (vector is Vector3)
        {
            Vector3 vector3 = (Vector3)vector;
            vector3.x = value;

            base.valueReference.set.Invoke(vector3);
        }
        else if (vector is Vector2)
        {
            Vector2 vector2 = (Vector2)vector;
            vector2.x = value;

            base.valueReference.set.Invoke(vector2);
        }
    }
    private void SetY(float value)
    {
        object vector = base.valueReference.get();

        if (vector is Vector3)
        {
            Vector3 vector3 = (Vector3)vector;
            vector3.y = value;
            base.valueReference.set.Invoke(vector3);
        }
        else if (vector is Vector2)
        {
            Vector2 vector2 = (Vector2)vector;
            vector2.y = value;
            base.valueReference.set.Invoke(vector2);
        }
    }
    private void SetZ(float value)
    {
        object vector = base.valueReference.get();

        if (vector is Vector3)
        {
            Vector3 vector3 = (Vector3)vector;
            vector3.z = value;
            base.valueReference.set.Invoke(vector3);
        }
    }

    public static bool IsCompatableTo(Type type)
    {
        return type == typeof(WUIVectorValue) || type == typeof(Vector3) || type == typeof(Vector2);
    }
}