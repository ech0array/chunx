using System.Collections.Generic;
using UnityEngine;

internal static class ProceduralMeshRing
{
    internal static Mesh GetMesh(float outerRadius, float innerRadius, int segments, int removedQuads = 0)
    {
        segments = Mathf.Max(segments, 3);
        removedQuads = Mathf.Max(removedQuads, 0);

        outerRadius = Mathf.Max(0, outerRadius);
        innerRadius = Mathf.Max(0, innerRadius);
        if (innerRadius > outerRadius)
        {
            float newInnerRadius = outerRadius;
            outerRadius = innerRadius;
            innerRadius = newInnerRadius;
        }

        if (removedQuads >= segments)
            return new Mesh();


        List<Vector3> vertices = new List<Vector3>(ProceduralHead.GetCircleVertices(segments));
        vertices.AddRange(vertices.ToArray());
        int[] triangles = new int[segments * 6];

        int outerOffset = segments - 1;

        for (int i = 0; i < segments; i++)
        {
            vertices[i] *= innerRadius;
            vertices[i + segments] *= outerRadius;

            if (i < segments - removedQuads)
            {
                int triangleStart = i * 6;
                int outerStart = i + outerOffset;

                triangles[triangleStart++] = i == outerOffset ? 0 : i;
                triangles[triangleStart++] = i == outerOffset ? i : outerStart + 1;
                triangles[triangleStart++] = i == outerOffset ? vertices.Count - 1 : outerStart + 2;

                triangles[triangleStart++] = i == outerOffset ? segments : i;
                triangles[triangleStart++] = i == outerOffset ? 0 : outerStart + 2;
                triangles[triangleStart++] = i == outerOffset ? vertices.Count - 1 : i + 1;
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