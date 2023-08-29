using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    List<Vector3> allVerticesList = new List<Vector3>();
    

    MeshFilter meshFilter;


    Vector3[] verticesArray;
    int[] triangles;


    // Start is called before the first frame update
    int numberOfTiles;
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();    
    }

    void Update()
    {

    }

    public void AddFourVertices(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {

        allVerticesList.Add(a);
        allVerticesList.Add(b);
        allVerticesList.Add(c);
        allVerticesList.Add(d);

        numberOfTiles++;
    }

    public void CalculateMesh()
    {
        verticesArray = new Vector3[allVerticesList.Count];
        triangles = new int[(int) (allVerticesList.Count * 1.5)];
        int tilesIndex = -1;

        while (numberOfTiles -1 != tilesIndex)
        {
            tilesIndex++;

            verticesArray[tilesIndex * 4] = allVerticesList[tilesIndex * 4];
            verticesArray[tilesIndex * 4 + 1] = allVerticesList[tilesIndex * 4 + 1];
            verticesArray[tilesIndex * 4 + 2] = allVerticesList[tilesIndex * 4 + 2];
            verticesArray[tilesIndex * 4 + 3] = allVerticesList[tilesIndex * 4 + 3];

            if (tilesIndex != 0)
            {
                // triangle 1
                triangles[tilesIndex * 6] = tilesIndex * 4;
                triangles[tilesIndex * 6 + 1] = tilesIndex * 4 - 2;
                triangles[tilesIndex * 6 + 2] = tilesIndex * 4 - 3;

                // triangle 2
                triangles[tilesIndex * 6 + 3] = tilesIndex * 4 + 3;
                triangles[tilesIndex * 6 + 4] = tilesIndex * 4 - 2;
                triangles[tilesIndex * 6 + 5] = tilesIndex * 4;

            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verticesArray;
        mesh.triangles = triangles;
        meshFilter.mesh = mesh;




    }

    public void ResetVerticesList()
    {
        allVerticesList.Clear();
        numberOfTiles = 0;
    }

}
        
        /*
        Vector3[] vertices = new Vector3[3];
        Vector2[] uv = new Vector2[3];
        int[] triangles = new int[3];

        vertices[0] = new Vector3(0, 0);
        vertices[1] = new Vector3(0, 100);
        vertices[2] = new Vector3(100, 100);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;


        GetComponent<MeshFilter>().mesh = mesh;
    }
}

        */
