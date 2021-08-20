using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMoveAnim : MonoBehaviour
{
    public Material green;
    public Material red;
    public bool attacking;
    void Start()
    {
        Destroy(gameObject, 0.45f);
        transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        
        if (attacking)
        {
            gameObject.GetComponent<MeshRenderer>().material = red;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = green;
        }
    }

    
    void Update()
    {
        transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
    }
}
