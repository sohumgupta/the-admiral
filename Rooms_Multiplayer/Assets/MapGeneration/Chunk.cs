using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Chunk that renders a chunk of terrain.
/// </summary>
public class Chunk
{
    Mesh mesh;

    /// <summary>
	/// Method that renders the given triangle vertices and vertex indicies.
	/// </summary>
	/// <param name="verticesToRender">
	/// array of Vector3 holding triangle vertices.
	/// </param>
	/// <param name="indicesToRender">
	/// array holding indicies of vertices to render.
	/// </param>
    public void Render(Vector3[] verticesToRender, int[] indicesToRender) {
        GameObject obj = new GameObject("chunk");
        obj.transform.SetParent(GameObject.Find("Terrain").transform);
        obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        mesh = obj.GetComponent<MeshFilter>().mesh;
        MeshCollider meshCollider = obj.AddComponent<MeshCollider>();
        mesh.Clear();

        mesh.vertices = verticesToRender;
        mesh.triangles = indicesToRender;
        mesh.Optimize();
        mesh.RecalculateNormals();

        // attaching material to mesh
        Material mat = Resources.Load("MeshMaterial") as Material;
        Renderer renderer = obj.GetComponent<MeshRenderer>();
        renderer.material = mat;
        meshCollider.sharedMesh = mesh;
    }
}
