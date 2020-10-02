using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchInputAxis : MonoBehaviour
{
    #region Values
    [SerializeField]
    private TouchAxis _type;

    [Serializable]
    private class Data
    {
        [SerializeField]
        internal float clearOriginOnUpWait = 1f;
        [SerializeField]
        internal float originRepositionScale = 150f;

        [SerializeField]
        internal Vector2 referenceResolution = new Vector2(1920, 1080);

        [SerializeField]
        internal float minDistanceOfOrigin;

        internal float clearOriginTime;

        internal bool hasOrigin;
        internal Vector2 originPositon;


        [SerializeField]
        internal AnimationCurve scaleOverInputExpanse;
    }
    [SerializeField]
    private Data _data;

    [SerializeField]
    private RectTransform[] _igonredRects;

    [SerializeField]
    private RectTransform _origin;
    [SerializeField]
    private RectTransform _active;

    private RectTransform _rectTransform;
    #endregion
    
    #region Unity Framework Entry Functions
    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        UpdateAxis();
    }
    #endregion

    #region Unity Event Functions
    public void UE_ResetUpTime()
    {
        _data.clearOriginTime = Time.time + _data.clearOriginOnUpWait;
    }
    #endregion

    #region Functions
    private void Initialize()
    {
        _rectTransform = (RectTransform)base.gameObject.transform;
    }

    private void UpdateAxis()
    {
        _data.hasOrigin = Time.time < _data.clearOriginTime;

        if (Input.touchCount == 0)
        {
            _active.anchoredPosition = _origin.anchoredPosition;

            TouchInputNode.SetAxisState(_type, Vector2.zero);
            return;
        }
        else if (!_data.hasOrigin)
        {
            Touch touch = GetFirstTouch();
            _data.originPositon = touch.position;
            _data.hasOrigin = _data.originPositon != Vector2.zero;
            if (!_data.hasOrigin)
            {
                TouchInputNode.SetAxisState(_type, Vector2.zero);
                return;
            }
        }


        Touch closestTouchToOrigin = GetClosesTouchToOrigin();
        Vector2 activePosition = closestTouchToOrigin.position;

        bool denyInput = IgnoreRectsContain(activePosition);
        if (denyInput)
        {
            TouchInputNode.SetAxisState(_type, Vector2.zero);
            return;
        }

        if (activePosition == Vector2.zero)
        {
            _active.anchoredPosition = _origin.anchoredPosition;
            TouchInputNode.SetAxisState(_type, Vector2.zero);
            return;
        }

        Vector2 output = _data.originPositon - activePosition;
        output = output.normalized;


        float scale = GetScale();
        float minDistanceFromOrigin = _data.minDistanceOfOrigin * scale;
        float originActiveDistance = Vector2.Distance(_data.originPositon, activePosition);
        if (originActiveDistance > minDistanceFromOrigin)
        {
            Vector2 targetOriginPosition = activePosition + (output * minDistanceFromOrigin);
            _data.originPositon = Vector2.MoveTowards(_data.originPositon, targetOriginPosition, Time.time * _data.originRepositionScale);
        }


        float distanceX = _data.originPositon.x - activePosition.x;
        distanceX = Mathf.Abs(distanceX);
        bool right = _data.originPositon.x < activePosition.x;
        output.x = _data.scaleOverInputExpanse.Evaluate(distanceX / minDistanceFromOrigin) * (right ? -1 : 1);

        float distanceY = _data.originPositon.y - activePosition.y;
        distanceY = Mathf.Abs(distanceY);
        bool up = _data.originPositon.y < activePosition.y;
        output.y = _data.scaleOverInputExpanse.Evaluate(distanceY / minDistanceFromOrigin) * (up ? -1 : 1);


        TouchInputNode.SetAxisState(_type, -output);

        DisplayInput(_data.originPositon, activePosition);

        _data.clearOriginTime = Time.time + _data.clearOriginOnUpWait;
    }

    private void DisplayInput(Vector2 origin, Vector2 active)
    {
        Vector2 originLocalized = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, origin, null, out originLocalized);
        Vector2 activeLocalized = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, active, null, out activeLocalized);

        _origin.anchoredPosition = originLocalized;
        _active.anchoredPosition = activeLocalized;

    }
    #endregion

    #region Helper Functions
    private float GetScale()
    {
        float baseScale = _data.referenceResolution.x * _data.referenceResolution.x;
        float currentScale = Screen.width * Screen.height;
        return currentScale / baseScale;
    }
    private Touch GetFirstTouch()
    {
        foreach (Touch touch in Input.touches)
        {
            bool contained = RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, touch.position);
            if (contained)
                return touch;
        }
        return new Touch();
    }
    private Touch GetClosesTouchToOrigin()
    {
        Touch closestTouch = new Touch();
        float shortestDistance = Mathf.Infinity;
        foreach (Touch touch in Input.touches)
        {
            bool contained = RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, touch.position);
            if (contained)
            {
                float distance = Vector2.Distance(touch.position, _data.originPositon);

                if (distance == 0)
                    return touch;

                if (distance < shortestDistance)
                    closestTouch = touch;

            }
        }
        return closestTouch;
    }

    private bool IgnoreRectsContain(Vector2 screenPosition)
    {
        foreach (RectTransform rectTransform in _igonredRects)
        {
            if (!rectTransform.gameObject.activeSelf)
                continue;
            bool contained = RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPosition);
            if (contained)
                return true;
        }
        return false;
    } 
    #endregion
}