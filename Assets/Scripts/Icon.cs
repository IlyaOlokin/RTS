using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
    public GameObject Unit;
    public Slider hpBar;
    private Unit u;
    
    void Start()
    {
        u = Unit.GetComponent<Unit>();
        GetComponent<Image>().sprite = u.spriteForIcon;
    }

    private void Update()
    {
        hpBar.maxValue = u.MaxHP;
        hpBar.value = u.HP;
        if (Unit == null)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnMouseDown()
    {
        foreach (var u in Mose.SelectedUnits)
        {
            u.GetComponent<Unit>().selected = false;

        }
        Mose.SelectedUnits.Clear();
        Mose.SelectedUnits.Add(Unit);
        u.selected = true;
    }
}
