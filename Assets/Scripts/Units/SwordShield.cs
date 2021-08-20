using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShield : Warrior
{
    
    void Start()
    {
        base.Start();
        //Initialize(100, .25f, 100, 10);
        name = "Swordsman";
    }

    
    void Update()
    {
        base.Update();
    }
    
    protected override void Attack()
    {
        AttackCD = CD;
        anim.Play("Attack" + Random.Range(1, 4));
        StartCoroutine(WaitForAttack());
        nav.speed = 0;
        Invoke("ReturnSpeed", 1f);
    }
}
