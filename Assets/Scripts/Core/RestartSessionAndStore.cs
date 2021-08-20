using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RestartSessionAndStore : MonoBehaviour
{
    public static int Money;
    public int MoneyCheat;
    public Text MoneyTxt;
    
    public int TotalUnits;

    public Vector3 defaultCameraPos;
    
    // зона с рандомными точками куда и откуда пойдут юниты
    public Transform firstPoinEnd;
    public Transform secondPointEnd;
    public Transform firstPointStart;
    public Transform secondPointStart;

    // кол-во юнитов каждого уровня
    public List<int> SavedSwords;
    public List<int> SavedSpears;
    public List<int> SavedAxes;
    public List<int> SavedArchers;
    
    // цены юнитов
    public int swordCost;
    public int spearCost;
    public int archerCost;
    public int axeCost;
    
    // префабы юнитов
    public GameObject Swordsman;
    public GameObject Spearman;
    public GameObject Axeman;
    public GameObject Archer;
    
    // начальный уровень
    public int BaseSwordmanLVL = 1;
    public int BaseSpearmanLVL = 1;
    public int BaseAxemanLVL = 1;
    public int BaseArcherLVL = 1;
    
    // бонусные статы
    public int SwordBonusHP;
    public int SwordBonusDMG;
    public float SwordBonusAS;
    public float SwordBonusMS;
    
    public int SpearBonusHP;
    public int SpearBonusDMG;
    public float SpearBonusAS;
    public float SpearBonusMS;
    
    public int AxeBonusHP;
    public int AxeBonusDMG;
    public float AxeBonusAS;
    public float AxeBonusMS;
    
    public int ArcherBonusHP;
    public int ArcherBonusDMG;
    public float ArcherBonusAS;
    public float ArcherBonusMS;
    
    
    // интерфейс магазина юнитов и улучшений
    public GameObject _camera;
    public GameObject StoreInterface;
    public GameObject IslandInterface;
    
    // переход между интерфейсами
    public GameObject RetreatButton;
    
    
    
    private void Start()
    {
        Mose.ControlledTeam = "Team1";
        
        SpawnUnits(Swordsman, SavedSwords, SwordBonusHP, SwordBonusDMG, SwordBonusAS, SwordBonusMS);
        SpawnUnits(Axeman, SavedAxes, AxeBonusHP, AxeBonusDMG, AxeBonusAS, AxeBonusMS);
        SpawnUnits(Spearman, SavedSpears, SpearBonusHP, SpearBonusDMG, SpearBonusAS, SpearBonusMS);
        SpawnUnits(Archer, SavedArchers, ArcherBonusHP, ArcherBonusDMG, ArcherBonusAS, ArcherBonusMS);
        
        SetDefaultCameraPos();

        if (MoneyCheat != 0)
        {
            Money = MoneyCheat;
        }
        
    }

    private void Update()
    {
        RetreatButton.SetActive(TotalUnits > 0);
        MoneyTxt.text = "GOLD: " + Money;
        if (Input.GetKeyDown(KeyCode.M))
        {
            Money += 100;
        }
    }

    void SetDefaultCameraPos()
    {
        Camera.main.transform.position = defaultCameraPos;
    }

    public void GetToStore()
    {
        _camera.GetComponent<CameraMove>().camLocked = true;
        _camera.GetComponent<Camera>().orthographic = true;
        StoreInterface.SetActive(true);
        IslandInterface.SetActive(false);
        TotalUnits = 0;
        foreach (var unit in GameObject.FindGameObjectsWithTag("Team1"))
        {
            unit.GetComponent<Unit>()?.DestroyUnit();
        }
    }

    public void GetToIland()
    {
        _camera.GetComponent<CameraMove>().camLocked = false;
        _camera.GetComponent<Camera>().orthographic = false;
        StoreInterface.SetActive(false);
        IslandInterface.SetActive(true);
        Start();
    }
    

    public void BuySwordsman()
    {
        if (Money >= swordCost)
        {
            SavedSwords[BaseSwordmanLVL]++;
            Money -= swordCost;
        }
    }
    public void BuySpearman()
    {
        if (Money >= spearCost)
        {
            SavedSpears[BaseSpearmanLVL]++;
            Money -= spearCost;
        }
    }
    public void BuyArcher()
    {
        if (Money >= archerCost)
        {
            SavedArchers[BaseArcherLVL]++;
            Money -= archerCost;
        }
        
    }
    public void BuyAxeman()
    {
        if (Money >= axeCost)
        {
            SavedAxes[BaseAxemanLVL]++;
            Money -= axeCost;
        }
    }
    

    void SpawnUnits(GameObject unitPrefab, List<int> savedUnits, int BonusHP, int BonusDMG, float BonusAS, float BonusMS)
    {
        for (int i = 0; i < savedUnits.Count; i++)
        {
            if (savedUnits[i] == 0)
            {
                continue;
            }

            int temp = savedUnits[i];
            savedUnits[i] = 0;
            for (int j = 0; j < temp; j++)
            {
                GameObject newUnit = Instantiate(unitPrefab);
                newUnit.transform.position = new Vector3(Random.Range(firstPointStart.position.x, secondPointStart.position.x), 1,Random.Range(secondPointStart.position.z, firstPointStart.position.z));
                var war = newUnit.GetComponent<Warrior>();
                war.MaxHP += BonusHP;
                war.DMG += BonusDMG;
                war.CD -= BonusAS;
                war.speed += BonusMS;
                war.GetLVL(i);
                newUnit.GetComponent<Unit>().AddPath(new Vector3(Random.Range(firstPoinEnd.position.x, secondPointEnd.position.x),1,Random.Range(secondPointEnd.position.z, firstPoinEnd.position.z)), false, true, false);
                
            }
            
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Team1"))
        {
            if (other.GetComponent<Warrior>())
            {
                TotalUnits++;
            }
        
            if (other.GetComponent<SwordShield>())
            {
                SavedSwords[other.GetComponent<Warrior>().UnitLVL]++;   
            }
            else if (other.GetComponent<Spearman>())
            {
                SavedSpears[other.GetComponent<Warrior>().UnitLVL]++;   
            }
            else if (other.GetComponent<Axeman>())
            {
                SavedAxes[other.GetComponent<Warrior>().UnitLVL]++;   
            }
            else if (other.GetComponent<Archer>())
            {
                SavedArchers[other.GetComponent<Warrior>().UnitLVL]++;   
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Team1"))
        {
            if (other.GetComponent<Warrior>())
            {
                TotalUnits--;
            }
        
            if (other.GetComponent<SwordShield>())
            {
                SavedSwords[other.GetComponent<Warrior>().UnitLVL]--;   
            }
            else if (other.GetComponent<Spearman>())
            {
                SavedSpears[other.GetComponent<Warrior>().UnitLVL]--;   
            }
            else if (other.GetComponent<Axeman>())
            {
                SavedAxes[other.GetComponent<Warrior>().UnitLVL]--;   
            }
            else if (other.GetComponent<Archer>())
            {
                SavedArchers[other.GetComponent<Warrior>().UnitLVL]--;   
            }
        }
        
        
    }
}

