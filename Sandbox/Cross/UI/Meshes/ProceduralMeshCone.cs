using System.Collections.Generic;
using UnityEngine;

internal static class ProceduralMeshCone
{
    internal static Mesh GetMesh(float headRadius, float footRadius, float extent, int segments)
    {
        segments = Mathf.Max(segments, 3);
        footRadius = Mathf.Max(0, footRadius);
        headRadius = Mathf.Max(0, headRadius);
        extent = Mathf.Abs(extent);

        List <Vector3> newVerts = new List<Vector3>(ProceduralHead.GetCircleVertices(segments));
        List<Vector3> vertices = new List<Vector3>(newVerts.ToArray());
        vertices.AddRange(newVerts.ToArray());
        vertices.AddRange(newVerts.ToArray());
        vertices.AddRange(newVerts.ToArray());
        int[] triangles = new int[segments * 12];


        Vector3 headPosition = Vector3.up * extent;
        Vector3 footPosition = -Vector3.up * extent;

        Mesh mesh = new Mesh();

        int capsStart = segments * 2;
        int headOffset = segments - 1;

        for (int i = 0; i < segments; i++)
        {
            vertices[i] *= headRadius;
            vertices[i] += headPosition;
            vertices[i + segments] *= footRadius;
            vertices[i + segments] += footPosition;

            vertices[i + capsStart] *= headRadius;
            vertices[i + capsStart] += headPosition;
            vertices[i + capsStart + segments] *= footRadius;
            vertices[i + capsStart + segments] += footPosition;


            int shaftTriangle = i * 6;
            int footVert = i;
            int headStart = footVert + headOffset;

            triangles[shaftTriangle++] = footVert == headOffset ? 0 : footVert;
            triangles[shaftTriangle++] = footVert == headOffset ? footVert : headStart + 1;
            triangles[shaftTriangle++] = footVert == headOffset ? capsStart - 1 : headStart + 2;

            triangles[shaftTriangle++] = footVert == headOffset ? segments : footVert;
            triangles[shaftTriangle++] = footVert == headOffset ? 0 : headStart + 2;
            triangles[shaftTriangle++] = footVert == headOffset ? capsStart - 1 : footVert + 1;


            int capTriangle = (i * 6) + (segments * 6);
            int capFootVert = i + capsStart;
            int capHeadVert = capFootVert + headOffset;

            triangles[capTriangle++] = capHeadVert;
            triangles[capTriangle++] = capsStart + segments;
            triangles[capTriangle++] = capFootVert != vertices.Count - 1 ?  capHeadVert + 1 : capHeadVert;

            if (i < segments - 1)
            {
                triangles[capTriangle++] = capsStart;
                triangles[capTriangle++] = capFootVert;
                triangles[capTriangle++] = capFootVert != capHeadVert - 1 ? capFootVert + 1 : capsStart;
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.RecalculateBounds();

        mesh.triangles = triangles;
        mesh.RecalculateTangents();
        mesh.RecalculateNormals();

        return mesh;
    }
}