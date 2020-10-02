using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

internal class WUICurve : WUIChartable
{
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal RectTransform container;

        [SerializeField]
        internal LineRenderer curveLine;

        [SerializeField]
        internal LineRenderer leftTangentLine;
        [SerializeField]
        internal LineRenderer rightTangentLine;

        [SerializeField]
        internal RectTransform leftValue;
        [SerializeField]
        internal RectTransform rightValue;
        [SerializeField]
        internal RectTransform leftTangent;
        [SerializeField]
        internal RectTransform rightTangent;
    }
    [SerializeField]
    private Components _components;

    [Serializable]
    private class Data
    {
        internal WUICurveValue wUICurveValue;

        [SerializeField]
        internal float lineDistanceFromHandle;

        internal bool leftValueClicked;
        internal bool rightValueClicked;

        internal bool leftTangentClicked;
        internal bool rightTangentClicked;

        internal Vector3 position;

        internal bool disallowSelectionThisFrame;
        internal bool grabbedThisFrame;
    }
    [SerializeField]
    private Data _data = new Data();

    protected override void Awake()
    {
        base.Awake();
    }


    private void LateUpdate()
    {
        if (!Application.isPlaying)
            return;
        UpdateCurve();
    }


    #region Extending Functions (WUIChartable)
    internal void Populate(WUICurveValue wUICurveValue)
    {
        _data.wUICurveValue = wUICurveValue;
        _components.leftTangent.anchoredPosition = Vector3.Scale(_data.wUICurveValue.curve.leftTangent.vector2,  _components.container.sizeDelta);
        _components.rightTangent.anchoredPosition = Vector3.Scale(_data.wUICurveValue.curve.rightTangent.vector2,  _components.container.sizeDelta);

        _components.leftValue.anchoredPosition = Vector3.Scale(_data.wUICurveValue.curve.leftValue.vector2,  _components.container.sizeDelta);
        _components.rightValue.anchoredPosition = Vector3.Scale(_data.wUICurveValue.curve.rightValue.vector2,  _components.container.sizeDelta);
    }

    private void UpdateCurve()
    {
        if (_data.rightTangentClicked)
        {
            _components.rightTangent.position = _data.position;
            _components.rightTangent.localPosition = new Vector3(_components.rightTangent.localPosition.x, _components.rightTangent.localPosition.y, 0);
        }
        if (_data.leftTangentClicked)
        {
            _components.leftTangent.position = _data.position;
            _components.leftTangent.localPosition = new Vector3(_components.leftTangent.localPosition.x, _components.leftTangent.localPosition.y, 0);
        }
        if (_data.rightValueClicked)
        {
            _components.rightValue.position = _data.position;
            _components.rightValue.localPosition = new Vector3(_components.container.sizeDelta.x, _components.rightValue.localPosition.y, 0);
        }
        if (_data.leftValueClicked)
        {
            _components.leftValue.position = _data.position;
            _components.leftValue.localPosition = new Vector3(-(_components.container.sizeDelta.x /2f), _components.leftValue.localPosition.y, 0);
        }

        _components.leftTangent.anchoredPosition = IsolatePosiiton(_components.leftTangent.anchoredPosition);
        _components.rightTangent.anchoredPosition = IsolatePosiiton(_components.rightTangent.anchoredPosition);
        _components.leftValue.anchoredPosition = IsolatePosiiton(_components.leftValue.anchoredPosition);
        _components.rightValue.anchoredPosition = IsolatePosiiton(_components.rightValue.anchoredPosition);


        _data.wUICurveValue.curve.leftTangent = new SerializableVector2(_components.leftTangent.anchoredPosition / _components.container.sizeDelta);
        _data.wUICurveValue.curve.rightTangent = new SerializableVector2(_components.rightTangent.anchoredPosition / _components.container.sizeDelta);

        _data.wUICurveValue.curve.leftValue = new SerializableVector2(_components.leftValue.anchoredPosition / _components.container.sizeDelta);
        _data.wUICurveValue.curve.rightValue = new SerializableVector2(_components.rightValue.anchoredPosition / _components.container.sizeDelta);

        List<Vector3> vector3s = new List<Vector3>();
        for (int i = 0; i < 32; i++)
        {
            float time = ((float)i) / (float)31;
            Vector3 value = Vector3.Scale(_data.wUICurveValue.curve.Evaluate(time), _components.container.sizeDelta); ;
            vector3s.Add(value);
        }

        _components.curveLine.SetPositions(vector3s.ToArray());
        Vector3 leftDirection = (_components.leftTangent.anchoredPosition3D - _components.leftValue.anchoredPosition3D).normalized;
        _components.leftTangentLine.SetPosition(0, _components.leftTangent.anchoredPosition3D - (leftDirection * _data.lineDistanceFromHandle));
        _components.leftTangentLine.SetPosition(1, _components.leftValue.anchoredPosition3D);

        Vector3 rightDistance = (_components.rightTangent.anchoredPosition3D - _components.rightValue.anchoredPosition3D).normalized;
        _components.rightTangentLine.SetPosition(0, _components.rightTangent.anchoredPosition3D - (rightDistance * _data.lineDistanceFromHandle));
        _components.rightTangentLine.SetPosition(1, _components.rightValue.anchoredPosition3D);

        if (_data.disallowSelectionThisFrame)
            SetRaycastable(true);
        _data.disallowSelectionThisFrame = false;
        _data.grabbedThisFrame = false;
    }

    internal override void ChartPreview(Vector3 worldPosition)
    {
        _data.position = worldPosition;
    }
    internal override void Chart(Vector3 worldPosition)
    {
    }
    #endregion


    private Vector3 IsolatePosiiton(Vector3 value)
    {
        value.x = Mathf.Clamp(value.x, 0, _components.container.sizeDelta.x);
        value.y = Mathf.Clamp(value.y, 0, _components.container.sizeDelta.y);
        return value;
    }


    private Vector2 GetRatio(Vector3 localPosition)
    {
        RectTransform rectTransform = (RectTransform)transform;

        Vector3[] corners = new Vector3[4];
        rectTransform.GetLocalCorners(corners);
        return new Vector2((1 - (localPosition.x / corners[0].x)) / 2f, 1 - (1 - (localPosition.y / corners[1].y)) / 2f);
    }

    internal bool TryUnselect()
    {
        if (_data.grabbedThisFrame)
            return false;
        bool changed = _data.leftTangentClicked || _data.rightTangentClicked || _data.leftValueClicked || _data.rightValueClicked;

        if (changed)
            _data.disallowSelectionThisFrame = true;

        _data.rightTangentClicked = false;
        _data.leftTangentClicked = false;
        _data.rightValueClicked = false;
        _data.leftValueClicked = false;
        return changed;
    }

    public void UE_ClickLeftValue()
    {
        if (_data.disallowSelectionThisFrame)
            return;
        SetRaycastable(false);
        _data.grabbedThisFrame = true;
        _data.rightTangentClicked = false;
        _data.leftTangentClicked = false;
        _data.rightValueClicked = false;

        _data.leftValueClicked = true;
    }

    public void UE_ClickRightValue()
    {
        if (_data.disallowSelectionThisFrame)
            return;
        SetRaycastable(false);

        _data.grabbedThisFrame = true;
        _data.rightTangentClicked = false;
        _data.leftTangentClicked = false;
        _data.leftValueClicked = false;

        _data.rightValueClicked = true;
    }

    public void UE_ClickLeftTangent()
    {
        if (_data.disallowSelectionThisFrame)
            return;
        SetRaycastable(false);

        _data.grabbedThisFrame = true;
        _data.rightTangentClicked = false;
        _data.rightValueClicked = false;
        _data.leftValueClicked = false;

        _data.leftTangentClicked = true;
    }

    public void UE_ClickRightTangent()
    {
        if (_data.disallowSelectionThisFrame)
            return;
        SetRaycastable(false);

        _data.grabbedThisFrame = true;
        _data.leftTangentClicked = false;
        _data.rightValueClicked = false;
        _data.leftValueClicked = false;

        _data.rightTangentClicked = true;
    }


    private void SetRaycastable(bool value)
    {
        _components.leftTangent.GetComponent<Graphic>().raycastTarget = value;
        _components.leftValue.GetComponent<Graphic>().raycastTarget = value;
        _components.rightTangent.GetComponent<Graphic>().raycastTarget = value;
        _components.rightValue.GetComponent<Graphic>().raycastTarget = value;
    }
}