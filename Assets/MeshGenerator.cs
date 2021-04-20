using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class MeshGenerator : MonoBehaviour
{
    public int xSize = 100;
    public int zSize = 100;
    public float Mult = 100;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;

    [SerializeField]
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Dictionary<char, float> leyend = new Dictionary<char, float>();
        leyend.Add('F',0);
        leyend.Add('W',7);
        leyend.Add('V',-7);
        leyend.Add('H',0);
        leyend.Add('D',1);

        CreateShape(MapLoader.ExpandMap(MapLoader.ReadMap(leyend), 2));

        //CreateShape(Mapas.m1);
        UpdateMesh();
        //GetComponent<NavMeshSurface>().BuildNavMesh();
    }
    void CreateShape(float[,] alturas)
    {
        vertices = new Vector3[(alturas.GetLength(0)) * (alturas.GetLength(1))];

        for (int i = 0, z = 0; z <= alturas.GetLength(1) - 1; z++)
            for (int x = 0; x <= alturas.GetLength(0) - 1; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                //float y = 0;
                vertices[i] = new Vector3(x, alturas[x, z] + y, z) * Mult/100;
                i++;
            }

        triangles = new int[(alturas.GetLength(1) - 1) * (alturas.GetLength(0) - 1) * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < alturas.GetLength(1) - 1; z++)
        {
            for (int x = 0; x < alturas.GetLength(0) - 1; x++)
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + alturas.GetLength(0) - 1 + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + alturas.GetLength(0) - 1 + 1;
                triangles[tris + 5] = vert + alturas.GetLength(0) - 1 + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }

        //recalculating UVs coordinates
        uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }
    }
    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
    }
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        for (int i = 0; i < vertices.Length; i++)
            Gizmos.DrawSphere(vertices[i], .1f);
    }
}
public static class Mapas
{
    static float o = 5;
    static float l = 7;
    static float x = 0;

    public static float[,] m1 = new float[21, 20]
      {
            { x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x } ,
            { x,l,l,l,l,l,l,l,l,l,l,l,l,l,l,l,l,l,l,x },
            { x,l,l,l,l,l,l,l,l,l,l,l,l,l,l,l,l,l,l,x },
            { x,l,l,o,o,o,o,o,o,o,o,o,o,o,o,o,o,l,l,x },
            { x,l,l,o,o,o,o,o,o,o,o,o,o,o,o,o,o,l,l,x },
            { x,l,l,o,o,o,o,o,o,o,o,o,o,o,o,o,o,l,l,x },
            { x,l,l,o,o,o,o,o,o,o,o,o,o,o,o,o,o,l,l,x },
            { x,l,l,o,o,o,o,o,o,o,o,o,o,o,o,o,o,l,l,x },
            { x,l,l,o,o,o,o,o,o,o,o,o,o,o,o,o,o,l,l,x },
            { x,l,l,o,o,o,o,o,o,o,o,o,o,o,o,o,o,l,l,x },
            { x,l,l,o,o,o,o,o,o,o,o,o,o,o,o,o,o,l,l,x },
            { x,l,l,o,o,o,o,o,o,o,o,o,o,o,o,o,o,l,l,x },
            { x,l,l,o,o,o,o,l,l,l,l,l,l,l,l,l,l,l,l,x },
            { x,l,l,o,o,o,o,l,l,l,l,l,l,l,l,l,l,l,l,x },
            { x,l,l,o,o,o,o,l,l,l,l,x,x,x,x,x,x,x,x,x },
            { x,l,l,o,o,o,o,l,l,l,l,x,x,x,x,x,x,x,x,x },
            { x,l,l,o,o,o,o,l,l,l,l,x,x,x,x,x,x,x,x,x },
            { x,l,l,o,o,o,o,l,l,l,l,x,x,x,x,x,x,x,x,x },
            { x,l,l,o,o,o,o,l,l,l,l,x,x,x,x,x,x,x,x,x },
            { x,l,l,o,o,o,o,l,l,l,l,x,x,x,x,x,x,x,x,x },
            { x,l,l,o,o,o,o,l,l,l,l,x,x,x,x,x,x,x,x,x },
      };



}