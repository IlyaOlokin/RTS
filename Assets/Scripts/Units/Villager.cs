using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Villager : Unit
{
    private Vector3 StartPos;
    
    public Vector2 MoveRange;
    
    void Start()
    {
        base.Start();
        StartPos = transform.position;
        StartCoroutine(MoveDelay());
        
    }

    public void LoadData(Save.UnitsSaveData save)
    {
        transform.position = new Vector3(save.Position.x, save.Position.y, save.Position.z);
    }
    

    IEnumerator MoveDelay()
    {
        yield return new WaitForSeconds(Random.Range(2f,5f));
        var x = Random.Range(StartPos.x - MoveRange.x, StartPos.x + MoveRange.x);
        var z = Random.Range(StartPos.z - MoveRange.y, StartPos.z + MoveRange.y);
        AddPath(new Vector3(x, 1, z), false, true, false);
        StartCoroutine(MoveDelay());
    }
    
    public override void TakeDamage(float dmg, GameObject attacker)  // Получение урона
    {
        base.TakeDamage(dmg, attacker);
        StopAllCoroutines();
        StartCoroutine(MoveDelay());
        Vector3 fearDir = (transform.position - attacker.gameObject.transform.position).normalized;
        AddPath(transform.position + fearDir * 6, false, true, false);
    }
}