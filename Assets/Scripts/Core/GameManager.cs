using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static List<GameObject> EnemyUnits = new List<GameObject>();
    public  List<GameObject> EnemyUnits1 = new List<GameObject>();
    public static List<GameObject> Units = new List<GameObject>();
    public  List<GameObject> Units1 = new List<GameObject>();
    
    public static List<GameObject> AllUnits = new List<GameObject>();
    

    void Start()
    {
        foreach (var unit in GameObject.FindGameObjectsWithTag("Team2"))
        {
            AllUnits.Add(unit);
        }

        foreach (var unit in GameObject.FindGameObjectsWithTag("Team1"))
        {
            AllUnits.Add(unit);
        }
        EnemyUnits1 = AllUnits;
        Units1 = Units;
    }



}
