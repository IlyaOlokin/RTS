using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Unit>())
        {
            other.GetComponent<Unit>().HP = other.GetComponent<Unit>().MaxHP;
        }
    }
}
