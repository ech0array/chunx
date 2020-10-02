using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class WUILoomNode : MonoBehaviour
{
    private LoomNode _loomNode;

    protected void Formalize(LoomNode loomNode)
    {
        _loomNode = loomNode;
        MoveTo(loomNode.position);
    }

    internal virtual void Grab()
    {

    }
    internal virtual void Drop()
    {
    }


    internal virtual void MoveTo(Vector3 value)
    {
        _loomNode.position = value;
    }
}