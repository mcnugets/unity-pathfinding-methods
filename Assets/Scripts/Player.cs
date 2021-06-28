using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    private float x;
    private float y;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {

        controls();



    }
    private void controls() 
    {
        float hr, vr;
        hr = Input.GetAxis("Horizontal") * 8f * Time.deltaTime;
        vr = Input.GetAxis("Vertical") * 8f * Time.deltaTime;
        Vector3 target = gameObject.transform.position + (Vector3.right * hr) + (Vector3.forward * vr);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, Time.deltaTime * 2f);
    }
}
