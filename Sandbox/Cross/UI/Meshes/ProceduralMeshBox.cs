using System.Collections.Generic;
using UnityEngine;

internal static class ProceduralMeshBox
{
    // tr f
    // 0.5f, 0.5f, 0.5f
    // tl f
    // -0.5f, 0.5f, 0.5f
    // br f
    // 0.5f, -0.5f, 0.5f),
    // bl f 
    // -0.5f, -0.5f, 0.5f
    // tr b
    // 0.5f, 0.5f, -0.5f
    // tl b
    // -0.5f, 0.5f, -0.5f
    // br b
    // 0.5f, -0.5f, -0.5f
    // bl b
    // -0.5f, -0.5f, -0.5f

    internal static Mesh GetMesh(Vector3 trf, Vector3 tlf, Vector3 brf, Vector3 blf, Vector3 trb, Vector3 tlb, Vector3 brb, Vector3 blb)
    {
        Vector3[] positions = new Vector3[]
            {
                trf,
                tlf,
                brf,
                blf,
                trb,
                tlb,
                brb,
                blb
            };

        List<Vector3> vertices = new List<Vector3>();
        vertices.AddRange(new Vector3[] { positions[1], positions[0], positions[2], positions[3] });
        vertices.AddRange(new Vector3[] { positions[2], positions[0], positions[4], positions[6] });
        vertices.AddRange(new Vector3[] { positions[7], positions[6], positions[4], positions[5] });
        vertices.AddRange(new Vector3[] { positions[3], positions[7], positions[5], positions[1] });
        vertices.AddRange(new Vector3[] { positions[4], positions[0], positions[1], positions[5] });
        vertices.AddRange(new Vector3[] { positions[2], positions[6], positions[7], positions[3] });


        int[] triangles = new int[72];
        for (int i = 0; i < 6; i++)
        {
            int startTriangle = i * 6;
            int startVertice = i * 4;
            ProceduralHead.CreateQuad(ref triangles, ref startTriangle, startVertice++, startVertice++, startVertice++, startVertice++);
        }


        Mesh mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles
        };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}