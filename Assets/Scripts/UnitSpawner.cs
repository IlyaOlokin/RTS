using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public Transform SpawnerTeam1;
    public Transform SpawnerTeam2;
    public GameObject Sword1;
    public GameObject Sword2;
    public GameObject Spear1;
    public GameObject Spear2;
    public GameObject Axe;
    public GameObject TwoHanded;
    public GameObject Archer1;
    public GameObject Archer2;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject newUnit = Instantiate(Sword1);
            newUnit.transform.position = SpawnerTeam1.position;
            GameManager.Units.Add(newUnit);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject newUnit = Instantiate(Spear1);
            newUnit.transform.position = SpawnerTeam1.position;
            GameManager.Units.Add(newUnit);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject newUnit = Instantiate(Axe);
            newUnit.transform.position = SpawnerTeam1.position;
            GameManager.Units.Add(newUnit);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameObject newUnit = Instantiate(Archer1);
            newUnit.transform.position = SpawnerTeam1.position;
            GameManager.Units.Add(newUnit);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameObject newUnit = Instantiate(Sword2);
            newUnit.transform.position = SpawnerTeam2.position;
            GameManager.EnemyUnits.Add(newUnit);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            GameObject newUnit = Instantiate(Spear2);
            newUnit.transform.position = SpawnerTeam2.position;
            GameManager.EnemyUnits.Add(newUnit);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            GameObject newUnit = Instantiate(TwoHanded);
            newUnit.transform.position = SpawnerTeam2.position;
            GameManager.EnemyUnits.Add(newUnit);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            GameObject newUnit = Instantiate(Archer2);
            newUnit.transform.position = SpawnerTeam2.position;
            GameManager.EnemyUnits.Add(newUnit);
        }
        
    }
}
