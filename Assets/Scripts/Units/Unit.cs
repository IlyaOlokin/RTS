using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

[Serializable]
public class Unit : MonoBehaviour
{
    public String EnemyTeam;

    public Sprite spriteForIcon;

    public bool selected;

    public NavMeshAgent nav;
    public Animator anim;

    public GameObject EnemyToKill;
    public GameObject SelectedIndicator;

    public Slider HpBar;
    public Slider HpBarRegenable;
    private float RegenDelay;
    private float RegenPercent = 0.02f;

    public GameObject textForDeathEffect;
    
    public GameObject VisionPoint;

    private Vector3 AttackDestPoint;

    public EnemySpot NativeSpot;

    public Queue<Vector3> path = new Queue<Vector3>();


    //Effects
    public bool moving;
    public bool attacking;

    // XP LVL UP
    public int xpCost;
    public int xpNeed;
    public int xp;
    
    public int UnitLVL;

    public int moneyCost;
    
    //Stats
    public float MaxHP;
    public float HP;
    public float BlockChance;
    public int vision;
    public float speed;
    

    public void Start()
    {
        //ControlledTeam = "Team1";

        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        HpBar.maxValue = MaxHP;
        HpBar.gameObject.SetActive(false);
        HP = MaxHP;
        HpBar.value = HP;
        HpBarRegenable.maxValue = HpBar.maxValue;
        HpBarRegenable.value = HpBar.value;
        

        if (gameObject.CompareTag("Team2") && gameObject.GetComponent<Warrior>())
        {
            GameManager.EnemyUnits.Add(gameObject);
        }

        if (gameObject.CompareTag("Team1"))
        {
            GameManager.Units.Add(gameObject);
        }
        AttackDestPoint = transform.position;
    }

    protected void Update()
    {
        
        // Всякие манипуляции с хп
        if (HP > MaxHP) HP = MaxHP;
        
        // Полоска хп 
        
        HpBar.gameObject.SetActive(selected || HP < MaxHP);
        HpBarRegenable.gameObject.SetActive(selected || HP < MaxHP);
        HpBar.value = HP;
        SelectedIndicator.SetActive(selected);
        
        
        //Движение с атакой
        if (Vector3.Distance(AttackDestPoint, transform.position) > 2 && EnemyToKill == null)
        {
            AddPath(AttackDestPoint, true, false, false);
        }
        else if (Vector3.Distance(AttackDestPoint, transform.position) <= 2)
        {
            AttackDestPoint = transform.position;
        }
        
        // Задержка перед регеном
        if (RegenDelay > 0)
        {
            RegenDelay -= Time.deltaTime;
        }
        else
        {
            RegenDelay = 0;
        }

        if (RegenDelay == 0 && HP < HpBarRegenable.value)
        {
            HP += RegenPercent * MaxHP * Time.deltaTime;
        }
        
        // Отмена действий на S
        if (Input.GetKeyDown(KeyCode.S) && selected && nav.enabled)
        {
            nav.SetDestination(transform.position);
            AttackDestPoint = transform.position;
                
            moving = false;
            
        }
        
        // Разрешение атаковать и очередь движения

        
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(nav.destination.x, 0, nav.destination.z)) < 0.4f)
        {
            moving = false;
            
            
            if (path.Count > 0)
            {
                AddPath(path.Dequeue(), attacking, false, false);
            }
            
            
        }
    }

    

    public void DestroyUnit()  // Смерть
    {
        Mose.SelectedUnits.Remove(gameObject);
        if (gameObject.CompareTag("Team1"))
        {
            GameManager.Units.Remove(gameObject);
        }
        else if (gameObject.CompareTag("Team2"))
        {
            GameManager.EnemyUnits.Remove(gameObject);
            StartDeathEffect(moneyCost);
        }

        if (NativeSpot != null)
        {
            NativeSpot.UnitsInSpot.Remove(gameObject);
        }
        

        
        RestartSessionAndStore.Money += moneyCost;
        
        
        Destroy(gameObject);
    }

    void StartDeathEffect(int money)
    {
        GameObject newText = Instantiate(textForDeathEffect);
        newText.transform.position = transform.position;
        newText.GetComponent<DeathEffect>().textMoney.GetComponent<Text>().text = "+" + money;
    }

    public void AddPath(Vector3 point, bool attack, bool moveUnlockForScripts, bool addToPath) // Получеие точки назначиния
    {
        //VisionPoint.transform.position = transform.position;
        if (nav.enabled && (gameObject.CompareTag(Mose.ControlledTeam) || moveUnlockForScripts))
        {
            nav.speed = speed;
            nav.stoppingDistance = 0;

            if (Input.GetKey(KeyCode.LeftShift) && addToPath)
            {
                path.Enqueue(point);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse1) || moveUnlockForScripts)
            {
                path.Clear();
                nav.SetDestination(point);
                path.Enqueue(point);
            }
            
            if (path.Count > 0)
            {
                nav.SetDestination(path.Peek());
            }

            AttackDestPoint = attack ? point : transform.position;
            
            moving = !attack;
            attacking = attack;
        }

        
    }
    
    public virtual void TakeDamage(float dmg, GameObject attacker)  // Получение урона
    {
        if (Random.value > BlockChance)
        {
            HP -= dmg;
            HpBarRegenable.value -= dmg * 0.6f;
            RegenDelay = 10;
        }
        else
        {
            anim.Play("Block" + (EnemyTeam[4] == '2' ? "1" : "2"));
        }

        if (Vector3.Distance(VisionPoint.transform.position, attacker.transform.position) > vision)
        {
            AttackDestPoint = attacker.transform.position;
            VisionPoint.transform.position = transform.position;
        }
        
        // Cмерть
        if (HP <= 0)
        {
            attacker?.GetComponent<Unit>().AddXP(xpCost);
            DestroyUnit();
        }
    }

    public void AddXP(int xp)
    {
        this.xp += xp;
        Upgrade();
    }

    protected virtual void Upgrade()
    {
        
    }
    
    private void OnMouseDown()  // Выделение юнита
    {
        
        if (!gameObject.CompareTag(Mose.ControlledTeam))
        {
            foreach (var unit in Mose.SelectedUnits)
            {
                unit.GetComponent<Unit>().selected = false;
            }
            Mose.SelectedUnits.Clear();
            Mose.SelectedUnits.Add(gameObject);
            selected = true;
            return;
        }

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            foreach (var unit in Mose.SelectedUnits)
            {
                unit.GetComponent<Unit>().selected = false;
            }
            Mose.SelectedUnits.Clear();
            Mose.SelectedUnits.Add(gameObject);
            SelectedIndicator.gameObject.SetActive(false);
            SelectedIndicator.gameObject.SetActive(true);
            selected = true;
        }
        else if (selected)
        {
            selected = false;
            Mose.SelectedUnits.Remove(gameObject);
        }
        else
        {
            Mose.SelectedUnits.Add(gameObject);
            selected = true;
        }
    }
}
