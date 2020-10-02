using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class ProceduralHead
{
    private static Dictionary<int, Vector3[]> _circleVerticesBySegments = new Dictionary<int, Vector3[]>();

    internal static Vector3[] GetCircleVertices(int segments)
    {
        bool contained = _circleVerticesBySegments.ContainsKey(segments);
        if (contained)
            return _circleVerticesBySegments[segments];

        List<Vector3> vertices = new List<Vector3>();
        float theta = (Mathf.PI * 2f) / segments;
        for (int i = 0; i < segments; i++)
        {
            float increment = theta * i;
            vertices.Add(new Vector3(Mathf.Sin(increment), 0, Mathf.Cos(increment)));
        }
        Vector3[] verticeArray = vertices.ToArray();
        _circleVerticesBySegments.Add(segments, verticeArray);
        return verticeArray;
    }


    internal static void CreateQuad(ref int[] triangles, ref int startTri, int vert1, int vert2, int vert3, int vert4)
    {
        triangles[startTri++] = vert1;
        triangles[startTri++] = vert4;
        triangles[startTri++] = vert3;

        triangles[startTri++] = vert3;
        triangles[startTri++] = vert2;
        triangles[startTri++] = vert1;
    }
}