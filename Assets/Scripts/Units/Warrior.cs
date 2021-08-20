using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Warrior : Unit
{
    public float DMG;
    
    
    public float AttackCD;
    public float CD;
    public float AttackDelay;
    public float AttackRange;
    

    public Image LvlUpArrow;


    void Start()
    {
        base.Start();
        UnitLVL = 1;
    }

    protected void Update()
    {
        base.Update();
        
        // КД атаки  
        AttackCD -= Time.deltaTime * 1f;
        if (AttackCD < 0) AttackCD = 0;

        // Поиск и атака врага  
        if (!moving)
        {
            TryAttackEnenmy(FindClosestEnemy(vision));
        }
    }

    public void LoadData(Save.UnitsSaveData save)
    {
        transform.position = new Vector3(save.Position.x, save.Position.y, save.Position.z);
        xp = 0;
        GetLVL(save.LVL);
        AddXP(save.XP);
    }
    
    protected override void Upgrade()  // Повышение уровня   
    {
        while (xp >= xpNeed)
        {
            if (UnitLVL == 10)
            {
                break;
            }
            LvlUpArrow.gameObject.SetActive(true);
            LvlUpArrow.gameObject.GetComponent<LvlUpArrow>().StartAnim();
            LVLUP();
        }
    }

    public void LVLUP()
    {
        float temp = HP / MaxHP;
        MaxHP += Mathf.RoundToInt(MaxHP * 0.1f);
        HP = Mathf.RoundToInt(MaxHP * temp);
            
        BlockChance *= 1.1f;
        DMG += Mathf.RoundToInt(DMG * 0.2f);
        HpBar.maxValue = MaxHP;
        HpBarRegenable.maxValue = MaxHP;
        if (xp > 0)
        {
            xp -= xpNeed;
        }
        xpNeed += 50;
        UnitLVL++;
    }

    public void GetLVL(int lvl)
    {
        for (int i = 0; i < lvl - 1; i++)
        {
            LVLUP();
        }
    }
    
    void TryAttackEnenmy(GameObject enemy)  // Атака  
    {
        EnemyToKill = enemy;
        
        if (enemy == null) return;
        transform.LookAt(EnemyToKill.transform);
        
        VisionPoint.transform.position = transform.position;
        vision = 10;
        if (NativeSpot != null)
        {
            NativeSpot.aggred = true;
        }
        
        
        Vector3 x = enemy.transform.position - transform.position;
        
        nav.SetDestination(enemy.transform.position + (transform.position - enemy.transform.position).normalized * (AttackRange - 0.01f));
        if (x.magnitude <= AttackRange)
        {
            nav.SetDestination(transform.position);
        }

        if (AttackCD == 0 && x.magnitude <= AttackRange)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        
    }
    
    protected IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(AttackDelay);
        
        int dmg = Mathf.RoundToInt(Random.Range(DMG - 2, DMG + 2));
        if (nav.speed == 0)
        {
            EnemyToKill?.GetComponent<Unit>()?.TakeDamage(dmg, gameObject);
        }
    }

    protected void ReturnSpeed()
    {
        nav.speed = speed;
    }
    
    private GameObject FindClosestEnemy(float maxDist = float.MaxValue)  // Поиск ближайшего юнита 
    {
        GameObject enemy = null;
        foreach (GameObject e in GameObject.FindGameObjectsWithTag(EnemyTeam))
        {
            float currDist = Vector3.Distance(e.transform.position, VisionPoint.transform.position);

            if (currDist < maxDist)
            {
                maxDist = currDist;
                enemy = e;
            }
        }

        return enemy;
    }
}
