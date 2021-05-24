using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shape : MonoBehaviour
{

   
   
   

    public GameObject createSquare(Vector3 pos, float size)
    {
        Mesh square = new Mesh();
        GameObject newGO = new GameObject("square");
        
        Vector3[] vertices =
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,1),
            new Vector3(1,0,0)

        };

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 center = (Vector3.forward + Vector3.right) / 2;
            vertices[i] = (vertices[i] - center) * size;
        }

        int[] triangles =
        {
            0,1,2,0,2,3
        };

        square.vertices = vertices;
        square.triangles = triangles;

       
        newGO.AddComponent<MeshFilter>().mesh = square;
        newGO.AddComponent<MeshRenderer>();
        newGO.transform.position = pos;

        return newGO;
    }
    public GameObject  createcircle(Vector3 position, float radius) 
    {
        GameObject newCircle = new GameObject("Circle");
        Mesh circleMesh = new Mesh();

        float theta = 0f;

        ;
        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(position);
        for (int i = 0; i < 10; i++)
        {
            theta += (2.0f * Mathf.PI * 0.1f);
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            vertices.Add(Vector3.forward * (y + position.z) + Vector3.right * (x+ position.x) );
        }
        vertices.Reverse();
        int[] triangles = {0,1,2,0,2,3,0,3,4,0,4,5,0,5,6,0,6,7,0,7,8,0,8,9,0,9,10};
          
        circleMesh.vertices = vertices.ToArray();
        circleMesh.triangles = triangles;
        newCircle.AddComponent<MeshFilter>().mesh = circleMesh;
        newCircle.AddComponent<MeshRenderer>();

        return newCircle;

    }


}
