using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class WUIGraph : WUIChartable
{
    #region Values
    [Range(0, 1)]
    [SerializeField]
    private float _xValue;
    [Range(0, 1)]
    [SerializeField]
    private float _yValue;
    #endregion

    #region Actions
    internal UnityAction<Vector2> onValueChanged = new UnityAction<Vector2>((Vector2 value) => { });
    internal UnityAction<Vector2> onValuePreview = new UnityAction<Vector2>((Vector2 value) => { });
    #endregion

    #region Extending Functions (WUIChartable)
    protected override void Awake()
    {
        base.Awake();
        Chart(_xValue, _yValue);
    }

    internal override void ChartPreview(Vector3 worldPosition)
    {
        base._preview.position = worldPosition;
        _preview.localPosition = new Vector3(_preview.localPosition.x, _preview.localPosition.y, 0);
        onValuePreview.Invoke(GetRatio(_preview.localPosition));
    }
    internal override void Chart(Vector3 worldPosition)
    {
        if (_active != null)
        {
            _active.position = worldPosition;
            _active.localPosition = new Vector3(_active.localPosition.x, _active.localPosition.y, 0);
        }

        onValueChanged.Invoke(GetRatio(_active.localPosition));
    }
    #endregion

    #region Functions
    internal void Chart(float xRatio, float yRatio)
    {
        xRatio = Mathf.Clamp(xRatio, 0f, 1f);
        yRatio = Mathf.Clamp(yRatio, 0f, 1f);

        RectTransform rectTransform = (RectTransform)transform;

        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        float heightDistance = Vector3.Distance(corners[0], corners[1]);
        Vector3 position = Vector3.Lerp(corners[0], corners[3], xRatio);
        position = position + (base.transform.up * heightDistance * yRatio);

        if (_active != null)
            base._active.position = position;
    } 
    #endregion

    #region Helper Functions
    private Vector2 GetRatio(Vector3 localPosition)
    {
        RectTransform rectTransform = (RectTransform)transform;

        Vector3[] corners = new Vector3[4];
        rectTransform.GetLocalCorners(corners);
        return new Vector2((1 - (localPosition.x / corners[0].x)) / 2f, 1 - (1 - (localPosition.y / corners[1].y)) / 2f);
    }
    #endregion
}
