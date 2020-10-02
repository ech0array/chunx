using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class SingleMonoBehaviour<T> : MonoBehaviour where T : Component
{
    internal static T instance;
    protected abstract bool isPersistant { get; }

    protected virtual void Awake()
    {
        if (isPersistant)
            DontDestroyOnLoad(this);

        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this as T;
    }
}
