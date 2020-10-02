using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal sealed class ProceduralMeshTester : SingleMonoBehaviour<ProceduralMeshTester>
{
    protected override bool isPersistant => true;

    [Serializable]
    private class Components
    {
        [SerializeField]
        internal MeshFilter meshFilter;
    }
    [SerializeField]
    private Components _components;

    [SerializeField]
    private Vector3 trf;
    [SerializeField]
    private Vector3 tlf;
    [SerializeField]
    private Vector3 brf;
    [SerializeField]
    private Vector3 blf;
    [SerializeField]
    private Vector3 trb;
    [SerializeField]
    private Vector3 tlb;
    [SerializeField]
    private Vector3 brb;
    [SerializeField]
    private Vector3 blb;



    private void OnDrawGizmos()
    {
        _components.meshFilter.mesh = ProceduralMeshBox.GetMesh(trf, tlf, brf, blf, trb, tlb, brb, blb);
    }
}
