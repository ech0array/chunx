using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal abstract class WUIChartable : Selectable
{
    #region Values
    [SerializeField]
    protected RectTransform _preview;
    [SerializeField]
    protected RectTransform _active;
    #endregion


    #region Unity Framework Entry Functions
    protected override void Awake()
    {
        base.Awake();
        if (_preview != null)
            _preview.gameObject.SetActive(false);
    }
    #endregion

    #region Extending Functions
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (_preview != null)
            _preview.gameObject.SetActive(true);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (_preview != null)
            _preview.gameObject.SetActive(false);
    }
    #endregion

    #region Contract Functions
    internal abstract void Chart(Vector3 worldPosition);
    internal abstract void ChartPreview(Vector3 worldPosition);
    #endregion

    #region Helper Functions
    protected static Vector3 ProjectPointOnLine(Vector3 lineStart, Vector3 lineDirection, Vector3 projection)
    {
        Vector3 linePointToPoint = projection - lineStart;
        float t = Vector3.Dot(linePointToPoint, lineDirection);
        return lineStart + lineDirection * t;
    } 
    #endregion
}