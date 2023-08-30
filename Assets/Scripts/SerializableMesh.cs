using System;
using UnityEngine;

[Serializable]
public class SerializableMesh
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector3[] normals;

    public SerializableMesh(Vector3[] vertices, int[] triangles, Vector3[] normals)
    {
        this.vertices = vertices;
        this.triangles = triangles;
        this.normals = normals;
    }

    public SerializableMesh(Mesh mesh): this(mesh.vertices, mesh.triangles, mesh.normals)
    {}

    public Mesh ToMesh()
    {
        Mesh mesh = new();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        return mesh;
    }
}
