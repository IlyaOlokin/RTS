using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Axeman : Warrior
{
     void Start()
     {
         base.Start();
         //Initialize(150, 0, 100, 10);
         name = "Axeman";
     }
 
     
     void Update()
     {
         base.Update();
     }
     
     protected override void Attack()
     {
         AttackCD = CD;
         anim.Play("AxeAttack" + Random.Range(1,4));
         StartCoroutine(WaitForAttack());
         nav.speed = 0;
         Invoke("ReturnSpeed", 1.3f);
     }
}
