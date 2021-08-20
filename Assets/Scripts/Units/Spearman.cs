using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spearman : Warrior
{

    
    void Start()
    {
        base.Start();
        //Initialize(80, 0, 100, 10);
        name = "Spearman";
    }

    
    void Update()
    {
        base.Update();
        anim.SetBool("ReadyToAttack", EnemyToKill != null);
    }
    
    protected override void Attack()
    {
        AttackCD = CD;
        anim.Play("SpearAttack");
        StartCoroutine(WaitForAttack());
        nav.speed = 0;
        Invoke("ReturnSpeed", 1f);
    }
}

