using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObj : MonoBehaviour
{
    public float Delay;
    void Start()
    {
        Destroy(gameObject, Delay);
    }

}
