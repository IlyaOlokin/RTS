using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RebakePoint : MonoBehaviour
{
    public GameObject camera;
    public GameObject obj;
    
    private void Update()
    {
        Vector3 dist = camera.transform.position - transform.position;

        float y = camera.transform.position.y * 0.025f;

        if (y < 0.8f)
        {
            y *= 1.25f;
        }
        obj.SetActive(dist.z < 75 * y && dist.z > -250 * y && Mathf.Abs(dist.x) < 200 * y);
        
        
    }


    
    
}
