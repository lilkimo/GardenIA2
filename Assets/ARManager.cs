using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.IO;
using System.Text;


public class ARManager : MonoBehaviour
{


    public ARMeshManager aRMeshManager;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hola");
    }

    // Update is called once per frame


    void Update()
    {
        
    }


    public void SaveMesh() {

        var meshFilters = aRMeshManager.meshes;
        Debug.Log("LARGO DE meshFilters: " + meshFilters.Count);

        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "meshes"))){
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "meshes"));
        };
        FileStream  fs = new FileStream(Path.Combine(Application.persistentDataPath, "meshes", "mesh1.bin"), FileMode.Create);
        BinaryWriter bw = new BinaryWriter(fs);
        var offset = 0;
        var n_vert = 0;
        var n_trian = 0;
        var n_norm = 0;


        foreach (var meshFilter in meshFilters) {
            var sharedMesh = meshFilter.sharedMesh;
            n_vert += sharedMesh.vertices.Length;
            n_trian += sharedMesh.triangles.Length;
            n_norm += sharedMesh.normals.Length;
        }

        bw.Write(n_vert);
        bw.Write(n_trian);
        bw.Write(n_norm);

        foreach(var mesh in meshFilters) {
            //var meshFilter = elem.Value;

            //var transform = elem.transform; me tira error?
            var sharedMesh = mesh.sharedMesh;


            //Debug.Log($"Transform: {transform.Translate}");
            Debug.Log($"SharedMeshVertices: {sharedMesh.vertices}");

            
            var vertices = sharedMesh.vertices;
            var triangles = sharedMesh.triangles;
            

            Debug.Log($"Vertices: {vertices[0]} {vertices[1]} {vertices[2]}");

            var n_vertices = vertices.Length;

            foreach (var vert in vertices) {
                bw.Write(vert[0]);
                bw.Write(vert[1]);
                bw.Write(vert[2]);
            };
        };

        foreach (var mesh in meshFilters) {

            var sharedMesh = mesh.sharedMesh;
            Debug.Log($"SharedMeshNormals: {sharedMesh.normals}");
            var normals = sharedMesh.normals;
            Debug.Log($"Normal: {normals[0]} {normals[1]} {normals[2]}");
            foreach (var norm in normals) {
                bw.Write(norm[0]);
                bw.Write(norm[1]);
                bw.Write(norm[2]);
            };

        };

        foreach (var mesh in meshFilters) {

            var sharedMesh = mesh.sharedMesh;
            Debug.Log($"SharedMeshTriangles: {sharedMesh.triangles}");
            var vertices = sharedMesh.vertices;
            var triangles = sharedMesh.triangles;
            
            Debug.Log($"Triangulos: {triangles[0]} {triangles[1]} {triangles[2]}");

            var n_vertices = vertices.Length;


            foreach (var triang in triangles) {
                bw.Write(triang+offset);
            };
            offset += n_vertices;
        };

        bw.Close();
        fs.Close();
    }
}
