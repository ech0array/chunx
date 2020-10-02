using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

sealed internal class WUICurveValue : WUIValue
{

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal LineRenderer lineRenderer;
        [SerializeField]
        internal RectTransform curveContainer;
        [SerializeField]
        internal RectTransform curveAnchor;
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Data
    {
        [SerializeField]
        internal Vector2 referenceSize;
    }
    [SerializeField]
    private Data _data;

    internal SerializableCurve curve;

    private void Update()
    {
        _components.lineRenderer.widthMultiplier = 20f;
    }

    internal override void Bind(SandboxValue sandboxValue)
    {
        base.valueReference = sandboxValue;
        curve = (SerializableCurve)sandboxValue.get();
        wUITipable.Populate(sandboxValue.id, sandboxValue.description);
        DisplayCurve();
    }

    internal void Set()
    {
        DisplayCurve();
        base.valueReference.set(curve);
    }

    private void DisplayCurve()
    {
        float verticalValue = 0;
        float horizontalValue = 0;

        for (int i = 0; i < 32; i++)
        {
            float time = ((float)i) / (float)31;
            Vector3 value = curve.Evaluate(time);
            if (verticalValue < value.y)
                verticalValue = value.y;
            if (horizontalValue < value.x)
                horizontalValue = value.x;
        }


        float verticalScale = (_components.curveContainer.sizeDelta.y + _components.curveAnchor.sizeDelta.y) / verticalValue;
        float horizontalScale = (_components.curveContainer.sizeDelta.x + _components.curveAnchor.sizeDelta.x) / horizontalValue;

        List<Vector3> vector3s = new List<Vector3>();
        for (int i = 0; i < 32; i++)
        {
            float time = ((float)i) / (float)31;
            vector3s.Add(Vector3.Scale(curve.Evaluate(time), new Vector3(horizontalScale, verticalScale)));
        }
        _components.lineRenderer.SetPositions(vector3s.ToArray());
    }

    public void UE_Click()
    {
        wUI.Edit(this);
    }
}