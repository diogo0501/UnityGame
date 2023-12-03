using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ConeMeshGenerator : MonoBehaviour
{
    public float radius = 1f;
    public float height = 1f;
    public int segments = 30;

    [HideInInspector]
    public MeshFilter visionConeMeshFilter;

    void Start()
    {
        if (visionConeMeshFilter == null)
        {
            // If the MeshFilter is not assigned, use the MeshFilter on the same GameObject
            visionConeMeshFilter = GetComponent<MeshFilter>();
        }

        GenerateMesh();
    }

    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        visionConeMeshFilter.mesh = mesh;

        // Rest of the code remains the same
        // ...
    }
}