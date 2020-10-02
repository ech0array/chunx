using System.Collections.Generic;
using UnityEngine;

internal static class ProceduralMeshConeTube
{

    internal static Mesh GetMesh(float footRadiusOuter, float headRadiusOuter, float footRadiusInner, float headRadiusInner,
        float innerHeadHeight, float outerHeadHeight, float innerFootHeight, float outerFootHeight,
        int segments)
    {
        segments = Mathf.Max(segments, 3);
        headRadiusOuter = Mathf.Max(0, headRadiusOuter);
        footRadiusOuter = Mathf.Max(0, footRadiusOuter);
        innerHeadHeight = Mathf.Abs(innerHeadHeight);

        List<Vector3> newVerts = new List<Vector3>(ProceduralHead.GetCircleVertices(segments));
        newVerts.AddRange(newVerts);

        // Shaft ends
        List<Vector3> vertices = new List<Vector3>(newVerts.ToArray());

        // Outer cap ends
        vertices.AddRange(newVerts.ToArray());

        // Inner cap ends
        vertices.AddRange(newVerts.ToArray());

        // Inner shaft ends
        vertices.AddRange(newVerts.ToArray());

        int[] triangles = new int[segments * 30];


        Vector3 innerHeadPosition = -Vector3.up * innerHeadHeight;
        Vector3 outerHeadPosition = -Vector3.up * outerHeadHeight;

        Vector3 innerFootPosition = Vector3.up * innerFootHeight;
        Vector3 outerFootPosition = Vector3.up * outerFootHeight;

        Mesh mesh = new Mesh();

        int headOffset = segments - 1;
        int footCapStart = segments * 2;
        int outerCapEnd = segments * 3;
        int innerCapStart = segments * 4;
        int innerCapEnd = segments * 5;
        int innerShaftStart = segments * 6;

        for (int i = 0; i < segments; i++)
        {
            bool finalQuad = i == segments - 1;
            int outerShaftHead = i;
            int outerShaftFoot = i + segments;

            int outerCapHead = i + footCapStart;
            int innerCapHead = outerCapHead + segments;

            int outerCapFoot = i + innerCapStart;
            int innerCapFoot = outerCapFoot + segments;

            int innerShaftHead = i + innerShaftStart;
            int innerShaftFoot = innerShaftHead + segments;

            // Shaft ends
            vertices[outerShaftHead] *= headRadiusOuter;
            vertices[outerShaftHead] += outerHeadPosition;
            vertices[outerShaftFoot] *= footRadiusOuter;
            vertices[outerShaftFoot] += outerFootPosition;


            vertices[innerCapHead] *= headRadiusOuter;
            vertices[innerCapHead] += outerHeadPosition;

            vertices[outerCapHead] *= headRadiusInner;
            vertices[outerCapHead] += innerHeadPosition;


            vertices[outerCapFoot] *= footRadiusOuter;
            vertices[outerCapFoot] += outerFootPosition;
            vertices[innerCapFoot] *= footRadiusInner;
            vertices[innerCapFoot] += innerFootPosition;



            // Inner shaft ends
            vertices[innerShaftHead] *= footRadiusInner;
            vertices[innerShaftHead] += innerFootPosition;
            vertices[innerShaftFoot] *= headRadiusInner;
            vertices[innerShaftFoot] += innerHeadPosition;


            int triangle = i * 30;

            int vert1 = outerShaftHead;
            int vert2 = outerShaftFoot;
            int vert3 = finalQuad ? segments : outerShaftFoot + 1;
            int vert4 = finalQuad ? 0 : outerShaftHead + 1;

            ProceduralHead.CreateQuad(ref triangles, ref triangle, vert1, vert2, vert3, vert4);


            vert1 = outerCapHead;
            vert2 = innerCapHead;
            vert3 = finalQuad ? footCapStart + segments: innerCapHead + 1;
            vert4 = finalQuad ? footCapStart : outerCapHead + 1;

            ProceduralHead.CreateQuad(ref triangles, ref triangle, vert1, vert2, vert3, vert4);


            vert1 = outerCapFoot;
            vert2 = innerCapFoot;
            vert3 = finalQuad ? innerCapStart + segments : innerCapFoot + 1;
            vert4 = finalQuad ? innerCapStart : outerCapFoot + 1;

            ProceduralHead.CreateQuad(ref triangles, ref triangle, vert1, vert2, vert3, vert4);


            vert1 = innerShaftHead;
            vert2 = innerShaftFoot;
            vert3 = finalQuad ? innerShaftStart + segments : innerShaftFoot + 1;
            vert4 = finalQuad ? innerShaftStart : innerShaftHead + 1;

            ProceduralHead.CreateQuad(ref triangles, ref triangle, vert1, vert2, vert3, vert4);
        }

        mesh.vertices = vertices.ToArray();
        mesh.RecalculateBounds();

        mesh.triangles = triangles;
        mesh.RecalculateTangents();
        mesh.RecalculateNormals();

        return mesh;
    }
}