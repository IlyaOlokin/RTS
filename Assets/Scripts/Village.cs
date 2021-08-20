using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : EnemySpot
{
    
    public Transform CollectionPoint;
    public Transform DirPoint;
    public AlarmerUnit alarmUnit;
    void Start()
    {
        base.Start();
    }

    private void Update()
    {
        base.Update();
    }


    void GetAlarm()
    {
        var form = ArmyManager.GetSquareFormation(CollectionPoint.position, DirPoint.position - CollectionPoint.position, UnitsInSpot.Count);
        gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(IsThereStillEnemies());
        StartCoroutine(AlarmReset());
        
        for (int i = 0; i < UnitsInSpot.Count; i++)
        {
            //if (UnitsInSpot[i] != null)
            
            UnitsInSpot[i].GetComponent<Unit>().AddPath(form[i], true, true, false);
            
        }

        
    }

    IEnumerator IsThereStillEnemies()
    {
        yield return new WaitForSeconds(35f);
        bool IsEnemiesHere = false;
        foreach (var unit in GameObject.FindGameObjectsWithTag("Team1"))
        {
            if (Vector3.Distance(unit.transform.position, transform.position) < spotRange)
            {
                StartCoroutine(IsThereStillEnemies());
                IsEnemiesHere = true;        
                break;
            }
        }

        if (!IsEnemiesHere)
        {
            ReturnToStartPositions(false);
        }
    }
    
    IEnumerator AlarmReset()
    {
        yield return new WaitForSeconds(34f);
        gameObject.GetComponent<Collider>().enabled = true;
        alarmUnit.alarmed = false;
        alarmUnit.gameObject.GetComponent<Unit>().vision = 20;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<AlarmerUnit>())
        {
            GetAlarm();
            other.GetComponent<Unit>().attacking = true;
            other.GetComponent<Unit>().vision = 10;
            alarmUnit = other.gameObject.GetComponent<AlarmerUnit>();
            alarmUnit.alarmed = true;
        }
        
    }
}
