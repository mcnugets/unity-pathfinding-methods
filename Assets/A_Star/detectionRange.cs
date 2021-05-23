using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectionRange : MonoBehaviour
{
    LineRenderer Circle;
    public float radius;
    int size;

    // Start is called before the first frame update
    void Start()
    {
        size = 101;
        Circle = GetComponent<LineRenderer>();
        Circle.startWidth = 0.2f;
        Circle.endWidth = 0.2f;
        Circle.positionCount = size;


        detection_range();
    }

    // Update is called once per frame
    void Update()
    {

        detection_range();
    }
    protected void detection_range()
    {

        // Circle area = 2 * mathf.pi * radiues^2

        float theta = 0f;
        Vector3 xP = transform.parent.transform.position;
        for (int i = 0; i < size; i++)
        {
            theta += (2.0f * Mathf.PI * 0.01f);
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Circle.SetPosition(i, new Vector3(x + xP.x, 5f, y + xP.z));
        }





    }
}
