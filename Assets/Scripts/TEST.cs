using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TEST: MonoBehaviour
{
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

        public SerializableMesh(Mesh mesh): this(mesh.vertices, mesh.triangles, mesh.normals) {}

        public Mesh ToMesh()
        {
            Mesh mesh = new();
            mesh.vertices = this.vertices;
            mesh.triangles = this.triangles;
            mesh.normals = this.normals;
            return mesh;
        }
    }

    [Serializable]
    public class SerializableMeshArray
    {
        public SerializableMesh[] meshes;

        public SerializableMeshArray(IEnumerable<Mesh> meshes) =>
            this.meshes = meshes.Select(mesh => new SerializableMesh(mesh)).ToArray();

        public Mesh[] ToMeshArray() =>
            meshes.Select(mesh => mesh.ToMesh()).ToArray();
    }
    
    public ARMeshManager aRMeshManager;
    public MeshFilter meshChunk;
    //public Mesh[] meshArray;
    //public TextAsset json;


    void Start()
    {

    }

    public void LoadMesh() {
        // string json = JsonUtility.ToJson(new SerializableMeshArray(meshArray));
        string ruta = Path.Combine(Application.persistentDataPath, "meshes", "mesh1.json");
        string json = File.ReadAllText("C:/Proyectos Unity/Camilo/mesh1.json"); //IMPORANTE: cambiar por ruta
        Debug.Log("asdadasda" + json);
        SerializableMeshArray serializedMeshes = JsonUtility.FromJson<SerializableMeshArray>(json);
        Mesh[] meshes = serializedMeshes.ToMeshArray();
        foreach (Mesh mesh in meshes)
        {
            MeshFilter chunk = Instantiate(meshChunk, transform);
            chunk.sharedMesh = mesh;
            chunk.transform.localScale = new Vector3(1000f, 1000f, 1000f);
        }
    }

    
    public void SaveMesh()
    {

        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "meshes"))){
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "meshes"));
        };
        Debug.Log($"mesh count: {aRMeshManager.meshes.Count}");
        List<string> hierarchy = new();
        foreach (var meshFilter in aRMeshManager.meshes)
        {
            hierarchy.Clear();
            for (Transform transform = meshFilter.transform; transform != null; transform = transform.parent)
                hierarchy.Add(transform.name);
            Debug.Log($"{string.Join(" -> ", Enumerable.Reverse(hierarchy))};{meshFilter.transform.position}");
        }
        
        string ruta = Path.Combine(Application.persistentDataPath, "meshes", "mesh1.json");
        var json = JsonUtility.ToJson(new SerializableMeshArray(aRMeshManager.meshes.Select(mesh => mesh.sharedMesh)));
        File.WriteAllText(ruta, json);
        Debug.Log($"Archivo guardado en {ruta}");
    }
}
