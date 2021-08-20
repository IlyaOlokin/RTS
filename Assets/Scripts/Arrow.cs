using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    public  int dmg;
    public String enemyteam;
    public GameObject archer;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 100);
    }

    
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(enemyteam))
        {
            other.gameObject.GetComponent<Unit>().TakeDamage(dmg, archer);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Terrain") || other.gameObject.CompareTag("Building"))
        {
            rb.isKinematic = true;
            Destroy(gameObject, 15f);
        }
    }
}
