using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class WUIMenu : WUIElement
{
    internal bool isInFocus { get; private set; }

    [SerializeField]
    private Color _backgroundColor;

    [SerializeField]
    internal bool blockStack;
    [SerializeField]
    internal bool unstackAllOnStack;
    [SerializeField]
    internal bool hideOnUnfocus;

    [SerializeField]
    private bool _restorePreviousPositionOnFocus;

    private TransformData _transformData;

    private LineRenderer[] _lineRenderers;
    private Canvas _canvas;

    protected override void Awake()
    {
        base.Awake();
    }

    internal virtual bool Stack()
    {
        if (wUI == null)
            Register();
        if (unstackAllOnStack)
            wUI.UnstackAll();
        return wUI.Stack(this);
    }

    internal virtual void Unstack()
    {
        Unfocus();
        base.gameObject.SetActive(false);
        wUI.Unstack();
    }

    internal virtual void Focus()
    {
        DestroyImmediate(_canvas);
        isInFocus = true;
        base.gameObject.SetActive(true);
        wUI.Focus(this, _backgroundColor);

        if (_restorePreviousPositionOnFocus && _transformData != null)
            _transformData.ApplyTo(wUI.user.controllableCamera.transform);
        SetLineRendererSorting();
    }

    internal virtual void Unfocus()
    {
        isInFocus = false;
        if (hideOnUnfocus)
            Hide();

        _transformData = new TransformData(wUI.user.controllableCamera.transform);

        _canvas = base.gameObject.AddComponent<Canvas>();
        if (_canvas != null)
        {
            _canvas.enabled = true;
            _canvas.overrideSorting = true;
            _canvas.sortingOrder = 1000;
            SetLineRendererSorting();
        }
    }

    internal virtual void Hide()
    {
        base.gameObject.SetActive(false);
    }

    private void SetLineRendererSorting()
    {
        Canvas canvas = _canvas;
        if(canvas == null)
            canvas = base.gameObject.GetComponentInParent<Canvas>();
        LineRenderer[] lineRenderers = base.gameObject.GetComponentsInChildren<LineRenderer>();
        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            if (lineRenderer != null)
                lineRenderer.sortingOrder = canvas.sortingOrder + 1;
        }
    }
}
