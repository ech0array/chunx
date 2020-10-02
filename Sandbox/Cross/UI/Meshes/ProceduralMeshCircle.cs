using System.Collections.Generic;
using UnityEngine;

internal static class ProceduralMeshCircle
{
    internal static Mesh GetMesh(float radius, int segments, int removedTriagles = 0)
    {
        segments = Mathf.Max(segments, 3);
        removedTriagles = Mathf.Max(removedTriagles, 0);

        if (removedTriagles >= segments)
            return new Mesh();

        List<Vector3> vertices = new List<Vector3>(ProceduralHead.GetCircleVertices(segments));
        vertices.Add(Vector3.zero);
        int[] triangles = new int[(vertices.Count - 1) * 3];

        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] *= radius;

            if (i < vertices.Count - (1 + removedTriagles))
            {
                int triangleStart = i * 3;
                triangles[triangleStart++] = i;
                triangles[triangleStart++] = i == vertices.Count - 2 ? 0 : i + 1;
                triangles[triangleStart++] = vertices.Count - 1;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}