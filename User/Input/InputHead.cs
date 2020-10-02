using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed internal class InputHead : MonoBehaviour
{
    [SerializeField]
    private bool _forceShowTouchInput;
    
    [Serializable]
    private class Components
    {
        [SerializeField]
        internal GameObject touchInputCanvas;
    }
    [SerializeField]
    private Components _components;

    private void Awake()
    {
        Initialize();   
    }

    private void Initialize()
    {
        TryDisplayTouchInput();
    }

    private void Update()
    {
        TryDispatchInput();
    }

    private void TryDispatchInput()
    {
        RawInputData rawInputData = RawInputData.current;
        UserHead.instance.ResolveUserInput(rawInputData);
    }

    private void TryDisplayTouchInput()
    {
        bool isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        bool isDevAndEnabled = _forceShowTouchInput;
        _components.touchInputCanvas.SetActive(isMobile || isDevAndEnabled);
    }
}
