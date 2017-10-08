using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMarchingSquareMesh : MonoBehaviour
{
    public Texture2D noiseMap;
    public MeshFilter meshFilter;

    private List<Vector3> verticies;
    private List<int> triangles;

    private void Start ()
    {
        verticies = new List<Vector3>();
        triangles = new List<int>();

        GenerateMesh();

        Mesh newMesh = new Mesh();
        newMesh.vertices = verticies.ToArray();
        newMesh.triangles = triangles.ToArray();
        meshFilter.mesh = newMesh;
	}

    private void GenerateMesh()
    {
        Color[] pixels = noiseMap.GetPixels();

        for (int y = 0; y < noiseMap.height - 1; y++)
        {
            for (int x = 0; x < noiseMap.width - 1; x++)
            {
                AddShape(x, y, pixels);
            }
        }
    }

    private void AddShape(int x, int y, Color[] pixels)
    {
        Color a = pixels[y * noiseMap.width + x];
        Color b = pixels[y * noiseMap.width + x + 1];
        Color c = pixels[(y + 1) * noiseMap.width + x];
        Color d = pixels[(y + 1) * noiseMap.width + x + 1];

        int shapeID = 0;
        if (a == Color.black)
        {
            shapeID |= 1;
        }
        if (b == Color.black)
        {
            shapeID |= 2;
        }
        if (c == Color.black)
        {
            shapeID |= 4;
        }
        if (d == Color.black)
        {
            shapeID |= 8;
        }

        switch (shapeID)
        {
            case 3:
                AddQuad(new Vector3(x, y, 0), new Vector3(x, y + 0.5f, 0), new Vector3(x + 1, y + 0.5f, 0), new Vector3(x + 1, y, 0));
                break;
            case 5:
                AddQuad(new Vector3(x, y, 0), new Vector3(x, y + 1, 0), new Vector3(x + 0.5f, y + 1, 0), new Vector3(x + 0.5f, y, 0));
                break;
            case 10:
                AddQuad(new Vector3(x + 0.5f, y, 0), new Vector3(x + 0.5f, y + 1, 0), new Vector3(x + 1, y + 1, 0), new Vector3(x + 1, y, 0));
                break;
            case 12:
                AddQuad(new Vector3(x, y + 0.5f, 0), new Vector3(x, y + 1, 0), new Vector3(x + 1, y + 1, 0), new Vector3(x + 1, y + 0.5f, 0));
                break;
            case 15:
                AddQuad(new Vector3(x,y,0), new Vector3(x,y + 1,0), new Vector3(x + 1, y + 1, 0), new Vector3(x + 1, y, 0));
                break;

            case 1:
                AddTriangle(new Vector3(x, y, 0), new Vector3(x, y + 0.5f, 0), new Vector3(x + 0.5f, y, 0));
                break;
            case 2:
                AddTriangle(new Vector3(x + 0.5f, y, 0), new Vector3(x + 1, y + 0.5f, 0), new Vector3(x + 1, y, 0));
                break;
            case 4:
                AddTriangle(new Vector3(x, y + 0.5f, 0), new Vector3(x, y + 1, 0), new Vector3(x + 0.5f, y + 1, 0));
                break;
            case 8:
                AddTriangle(new Vector3(x + 0.5f, y + 1, 0), new Vector3(x + 1, y + 1, 0), new Vector3(x + 1, y + 0.5f, 0));
                break;

            case 6:
                AddTriangle(new Vector3(x, y + 0.5f, 0), new Vector3(x, y + 1, 0), new Vector3(x + 0.5f, y + 1, 0));
                AddTriangle(new Vector3(x + 0.5f, y, 0), new Vector3(x + 1, y + 0.5f, 0), new Vector3(x + 1, y, 0));
                break;
            case 9:
                AddTriangle(new Vector3(x, y, 0), new Vector3(x, y + 0.5f, 0), new Vector3(x + 0.5f, y, 0));
                AddTriangle(new Vector3(x + 0.5f, y + 1, 0), new Vector3(x + 1, y + 1, 0), new Vector3(x + 1, y + 0.5f, 0));
                break;

            case 7:
                AddPentagon(new Vector3(x, y, 0), new Vector3(x, y + 1, 0), new Vector3(x + 0.5f, y + 1, 0), new Vector3(x + 1, y + 0.5f, 0), new Vector3(x + 1, y, 0));
                break;
            case 11:
                AddPentagon(new Vector3(x, y, 0), new Vector3(x, y + 0.5f, 0), new Vector3(x + 0.5f, y + 1, 0), new Vector3(x + 1, y + 1, 0), new Vector3(x + 1, y, 0));
                break;
            case 13:
                AddPentagon(new Vector3(x, y, 0), new Vector3(x, y + 1, 0), new Vector3(x + 1, y + 1, 0), new Vector3(x + 1, y + 0.5f, 0), new Vector3(x + 0.5f, y, 0));
                break;
            case 14:
                AddPentagon(new Vector3(x, y + 0.5f, 0), new Vector3(x, y + 1, 0), new Vector3(x + 1, y + 1, 0), new Vector3(x + 1, y, 0), new Vector3(x + 0.5f, y, 0));
                break;
        }
    }

    private void AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        int vertexCount = verticies.Count;

        verticies.Add(a);
        verticies.Add(b);
        verticies.Add(c);
        verticies.Add(d);

        triangles.Add(vertexCount);
        triangles.Add(vertexCount + 1);
        triangles.Add(vertexCount + 2);

        triangles.Add(vertexCount + 2);
        triangles.Add(vertexCount + 3);
        triangles.Add(vertexCount);
    }

    private void AddTriangle(Vector3 a, Vector3 b, Vector3 c)
    {
        int vertexCount = verticies.Count;

        verticies.Add(a);
        verticies.Add(b);
        verticies.Add(c);

        triangles.Add(vertexCount);
        triangles.Add(vertexCount + 1);
        triangles.Add(vertexCount + 2);
    }

    private void AddPentagon(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e)
    {
        int vertexCount = verticies.Count;

        verticies.Add(a);
        verticies.Add(b);
        verticies.Add(c);
        verticies.Add(d);
        verticies.Add(e);

        triangles.Add(vertexCount);
        triangles.Add(vertexCount + 1);
        triangles.Add(vertexCount + 2);

        triangles.Add(vertexCount);
        triangles.Add(vertexCount + 2);
        triangles.Add(vertexCount + 3);

        triangles.Add(vertexCount);
        triangles.Add(vertexCount + 3);
        triangles.Add(vertexCount + 4);
    }
}
