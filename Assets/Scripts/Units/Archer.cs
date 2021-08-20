using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Warrior
{
    public GameObject Arrow;

    void Start()
    {
        base.Start();
        //Initialize(50, 0, 100, 10);
        name = "Archer";
    }

    
    void Update()
    {
        base.Update();
    }
    
    protected override void Attack()
    {
        AttackCD = CD;
        anim.Play("ArcherAttack");
        StartCoroutine(WaitForAttackArcher());
        nav.speed = 0;
        Invoke("ReturnSpeed", 2f);
    }

     IEnumerator WaitForAttackArcher()
    {
        yield return new WaitForSeconds(AttackDelay);
        
        
        if (nav.speed == 0 && EnemyToKill != null)
        {
            float distance = Vector3.Distance(transform.position, EnemyToKill.transform.position);
            float speed = 25;
            float angle = Mathf.Asin(distance * 9.81f / (speed * speed)) / 2;
            //AttackDelay = 2 * speed * Mathf.Sin(Mathf.PI / 4) / 9.81f;
            GameObject newArrow = Instantiate(Arrow);
            newArrow.transform.position = transform.position;
            newArrow.GetComponent<Arrow>().dmg = Mathf.RoundToInt(Random.Range(DMG - 2, DMG + 2));
            newArrow.GetComponent<Arrow>().enemyteam = EnemyTeam;
            newArrow.GetComponent<Arrow>().archer = gameObject;
            Rigidbody rb = newArrow.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0, speed * Mathf.Sin(angle), 0) + (EnemyToKill.transform.position - transform.position).normalized * Mathf.Cos(angle) * speed;
        }
    }
}
