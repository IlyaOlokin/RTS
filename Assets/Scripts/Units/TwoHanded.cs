using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoHanded : Warrior
{
    void Start()
    {
        base.Start();
        //Initialize(200, 0, 150, 10);
        name = "Knight";
    }

    
    void Update()
    {
        base.Update();
    }
    
    protected override void Attack()
    {
        AttackCD = CD;
        anim.Play("TwoHandedAttack");
        StartCoroutine(WaitForAttack());
        nav.speed = 0;
        Invoke("ReturnSpeed", 1.3f);
    }
}
