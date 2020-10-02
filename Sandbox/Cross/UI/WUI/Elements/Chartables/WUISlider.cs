using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class WUISlider : WUIChartable
{
    #region Values
    [Range(0, 1)]
    [SerializeField]
    private float _value;
    #endregion

    #region Actions
    [SerializeField]
    private UnityEventFloat _onValueChanged;
    [SerializeField]
    private UnityEventFloat _onValuePreview;

    internal UnityAction<float> onValueChanged = new UnityAction<float>((float value) => { });
    internal UnityAction<float> onValuePreview = new UnityAction<float>((float value) => { });
    #endregion

    #region Extending Functions (WUIChartable)s
    protected override void Awake()
    {
        base.Awake();
        Chart(_value);
    }

    internal override void ChartPreview(Vector3 worldPosition)
    {
        RectTransform rectTransform = (RectTransform)transform;
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        Vector3 middleLine = Vector3.Lerp(corners[0], corners[1], 0.5f);
        Vector3 position = ProjectPointOnLine(middleLine, base.transform.right, worldPosition);
        float lengthPosition = Vector3.Distance(middleLine, position);

        float widthLength = Vector3.Distance(corners[1], corners[2]);
        float ratio = lengthPosition / widthLength;
        if (_preview != null)
            _preview.position = position;
        onValuePreview.Invoke(ratio);
        _onValuePreview.Invoke(ratio);
    }
    internal override void Chart(Vector3 worldPosition)
    {
        RectTransform rectTransform = (RectTransform)transform;

        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);


        Vector3 middleLine = Vector3.Lerp(corners[0], corners[1], 0.5f);
        Vector3 position = ProjectPointOnLine(middleLine, base.transform.right, worldPosition);

        float distanceOfWidth = Vector3.Distance(middleLine, position);

        float widthLength = Vector3.Distance(corners[1], corners[2]);
        float ratio = distanceOfWidth / widthLength;
        if (_active != null)
            _active.position = position;

        onValueChanged.Invoke(ratio);
        _onValueChanged.Invoke(ratio);
        _value = ratio;
    }
    #endregion

    #region Functions
    internal void Chart(float ratio)
    {
        ratio = Mathf.Clamp(ratio, 0f, 1f);

        RectTransform rectTransform = (RectTransform)transform;

        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        Vector3 centerRight = Vector3.Lerp(corners[2], corners[3], 0.5f);
        Vector3 centerLeft = Vector3.Lerp(corners[0], corners[1], 0.5f);
        Vector3 position = Vector3.Lerp(centerLeft, centerRight, ratio);

        if (_active != null)
            _active.position = position;
    } 
    #endregion
}
