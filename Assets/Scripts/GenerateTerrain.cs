using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{

    int heightScale = 5;
    float detailScale = 5f;

    private void Start()
    {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;

        Vector3[] vertices = mesh.vertices;


        for (int v = 0; v < vertices.Length; v++)
        {
            vertices[v].y = Mathf.PerlinNoise((vertices[v].x + this.transform.position.x) / detailScale, (vertices[v].z + this.transform.position.z) / detailScale) * heightScale;

            float temp = Mathf.Abs((transform.position.x + vertices[v].x));

            //if (temp < 4 && temp >= 10) vertices[v].y += temp / 5;

            if (temp < 10) vertices[v].y += temp / 2;
            else if (temp >= 10 && temp < 12) vertices[v].y += temp / 1.5f;
            else vertices[v].y += temp / 1.2f;

        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        this.gameObject.AddComponent<MeshCollider>();
    }
}
