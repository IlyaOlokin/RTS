using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmerUnit : MonoBehaviour
{

    private Unit me;
    public Transform AlarmPoint;
    public bool alarmed = false;
    void Start()
    {
        me = gameObject.GetComponent<Unit>();
        me.vision = 20;
    }

    
    void Update()
    {
        if (me.EnemyToKill != null && !alarmed)
        {
            me.AddPath(AlarmPoint.position, false, true, false);
        }
    }
    
}
