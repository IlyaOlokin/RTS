using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;


public class SaveLoadManager : MonoBehaviour
{
    string filePath;

    public RestartSessionAndStore rsas;
    

    private void Start()
    {
        filePath = Application.persistentDataPath + "/save.txt";
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Open);
        Save save = new Save();
        
        save.SaveUnits(GameManager.AllUnits);
        
        bf.Serialize(fs, save);
        
        fs.Close();
    } 

    public void LoadGame()
    {
        if (!File.Exists(filePath))
            return;
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Open);

        Save save = (Save) bf.Deserialize(fs);
        fs.Close();

        int i = 0;
        foreach (var unitsdata in save.unitsData)
        {
            var unit = GameManager.AllUnits[i];

            int hp = Mathf.RoundToInt(unit.GetComponent<Unit>().HP  / unit.GetComponent<Unit>().MaxHP);
            unit.GetComponent<Unit>().xp = 0;
            unit.GetComponent<Unit>().UnitLVL = 1;
            
            if (unit.GetComponent<SwordShield>())
            {
                unit.GetComponent<SwordShield>().HP = hp * (100 + rsas.SwordBonusHP);
                unit.GetComponent<SwordShield>().MaxHP = 100 + rsas.SwordBonusHP;
                unit.GetComponent<SwordShield>().DMG = 10 + rsas.SwordBonusDMG;
                unit.GetComponent<SwordShield>().BlockChance = 0.2f;
            }
            else if (unit.GetComponent<Spearman>())
            {
                unit.GetComponent<Spearman>().HP = hp * (80 + rsas.SpearBonusHP);
                unit.GetComponent<Spearman>().MaxHP = 80 + rsas.SpearBonusHP;
                unit.GetComponent<Spearman>().DMG = 20 + rsas.SpearBonusDMG;
                unit.GetComponent<Spearman>().CD = 2 - rsas.SpearBonusAS;
            }
            else if (unit.GetComponent<Archer>())
            {
                unit.GetComponent<Archer>().HP = hp * (50 + rsas.ArcherBonusHP);
                unit.GetComponent<Archer>().MaxHP = 50 + rsas.ArcherBonusHP;
                unit.GetComponent<Archer>().DMG = 50 + rsas.ArcherBonusDMG;
                unit.GetComponent<Archer>().speed = 5 + rsas.ArcherBonusMS;
            }
            else if (unit.GetComponent<Axeman>())
            {
                unit.GetComponent<Axeman>().HP = hp * (150 + rsas.AxeBonusHP);
                unit.GetComponent<Axeman>().MaxHP = 15 - +rsas.AxeBonusHP;
                unit.GetComponent<Axeman>().DMG = 25 + rsas.AxeBonusDMG;
            }
            else if (unit.GetComponent<TwoHanded>())
            {
                unit.GetComponent<TwoHanded>().HP = hp * 200;
                unit.GetComponent<TwoHanded>().MaxHP = 200;
                unit.GetComponent<TwoHanded>().DMG = 20;
            }

            if (unit.GetComponent<Warrior>())
            {
                unit.GetComponent<Warrior>().LoadData(unitsdata);
            }
            else if (unit.GetComponent<Villager>())
            {
                unit.GetComponent<Villager>().LoadData(unitsdata);
            }
            
            i++;
        }
            
            
        
    }
}

[System.Serializable]
public class Save
{
    [System.Serializable]
    public struct Vec3
    {
        public float x, y, z;

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    
    [System.Serializable]
    public struct UnitsSaveData
    {
        public Vec3 Position;
        public int LVL, XP;

        public UnitsSaveData(Vec3 pos, int lvl, int xp)
        {
            Position = pos;
            LVL = lvl;
            XP = xp;
        }
    }
    
    public List<UnitsSaveData> unitsData = new List<UnitsSaveData>();

    public void SaveUnits(List<GameObject> Units)
    {
        FileStream fs = new FileStream("ads", FileMode.Open);
        foreach (var unit in Units)
        {
            Vec3 pos = new Vec3(unit.transform.position.x, unit.transform.position.y, unit.transform.position.z);
            int lvl = unit.GetComponent<Unit>().UnitLVL;
            int xp = unit.GetComponent<Unit>().xp;
            var x = new XmlSerializer(unit.GetType());
            
            x.Serialize(fs, unit);
            
            unitsData.Add(new UnitsSaveData(pos, lvl, xp));
        }
    }

    
}
