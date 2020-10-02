using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class Interactor : MonoBehaviour
{
    protected abstract ControllableCamera controllableCamera { get; set; }

    internal abstract void Register(ControllableCamera controllableCamera);
}
