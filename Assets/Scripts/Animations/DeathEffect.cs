using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathEffect : MonoBehaviour
{
    public Text textMoney;
    private void Start()
    {
        Destroy(gameObject, 1.3f);
        
    }

    private void FixedUpdate()
    {
        textMoney.gameObject.transform.position += new Vector3(0, 0.03f, 0);
    }
}
