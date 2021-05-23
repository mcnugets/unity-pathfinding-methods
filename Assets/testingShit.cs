using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingShit : MonoBehaviour
{
    shape sqr;

    
    public float size;
    
    void Start()
    {
        GameObject[,] arrayGO = new GameObject[21,21];
       
        sqr = GetComponent<shape>();

        sqr.createcircle(new Vector3(0f, 0f, 0f), 2f);
       
       
    }
}

/*
for (int x = 0; x <= 20; x++)
{
    for (int y = 0; y <= 20; y++)
    {
        float value = (20 * (size + size / 2)) / 2;
        Vector3 center = transform.position - (Vector3.forward + Vector3.right) * value;
        Vector3 set_position = center + new Vector3(x, 0f, y) * (size + size / 2);
        arrayGO[x, y] = sqr.createSquare(set_position, size);

    }
}*/