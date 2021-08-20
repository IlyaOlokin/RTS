using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interface : MonoBehaviour
{
    public GameObject canvas;
    public GameObject InfoPlate;
    public Text UnitName;
    public Slider HpBarRegenable; 
    public Slider HpBar;
    public Slider XpBar;
    public Text Health;
    public Text XP;
    public Text Damage;
    public Text BlockChance;
    public Text LVL;

    public Color hpBarGreenColor;
    public Color hpBarGreenColorRegen;
    public Color hpBarRedColor;
    public Color hpBarRedColorRegen;

    public Text Lose;
    public Text Win;
    
    
    public GameObject GameMenu;

    public GameObject FastDelete;
    private GameObject FastDeleteSave;
    public GameObject UnitList;
    public Transform FirstPos;
    public Transform StepX;
    public Transform StepY;
    public GameObject UnitIcon;
    public List<GameObject> IconsList;
    public int UnitsPage = 1;

    public GameObject Forms;
    public Button LineForm;
    public Button SquareForm;
    public Button JapanForm;
    public Button TriangleForm;

    public Button Play;
    public Button Restart;
    public static bool isStarted;

    public void WinGame()
    {
        Win.gameObject.SetActive(true);
        Restart.gameObject.SetActive(true);
        isStarted = false;
    }

    public void LoseGame()
    {
        Lose.gameObject.SetActive(true);
        Restart.gameObject.SetActive(true);
        isStarted = false;
    }
    
    
    private void Start()
    {
        SetFormSquare();
        Time.timeScale = 0;
        Restart.gameObject.SetActive(false);
        isStarted = false;
        Forms.SetActive(false);
        InfoPlate.SetActive(false);
        Win.gameObject.SetActive(false);
        Lose.gameObject.SetActive(false);
        GameMenu.gameObject.SetActive(false);
        
        FastDeleteSave = new GameObject();
        
        
        StartGame();
    }

    public void StartGame()
    {
        isStarted = true;
        Time.timeScale = 1;
        Play.gameObject.SetActive(false);
        Restart.gameObject.SetActive(true);
        Forms.SetActive(false);
        InfoPlate.SetActive(false);
        Win.gameObject.SetActive(false);
        Lose.gameObject.SetActive(false);
        
    }

    public void RestartGame()
    {
        GameManager.EnemyUnits.Clear();
        GameManager.Units.Clear();
        isStarted = false;
        Forms.SetActive(false);
        InfoPlate.SetActive(false);
        Win.gameObject.SetActive(false);
        Lose.gameObject.SetActive(false);
        Mose.SelectedUnits.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenCloseGameMenu()
    {
        GameMenu.SetActive(!GameMenu.activeSelf);
        Time.timeScale = Math.Abs(Time.timeScale - 1);
    }
    

    void Update()
    {
        /*if (GameController.EnemyUnits.Count == 0)
        {
            WinGame();
        }

        if (GameController.Units.Count == 0)
        {
            LoseGame();
        }*/

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OpenCloseGameMenu();
        }
        
        
        if (Mose.SelectedUnits.Count == 1)
        {
            if (Mose.SelectedUnits[0].GetComponent<Villager>())
            {
                return;
            }
            
            InfoPlate.SetActive(true);
            Forms.SetActive(false);
            UnitList.SetActive(false);
            
            var war = Mose.SelectedUnits[0].GetComponent<Warrior>();
            
            UnitName.text = Mose.SelectedUnits[0].name;
            Health.text = Mathf.RoundToInt(war.HP) + " / " + war.MaxHP;
            XP.text = war.xp + " / " + war.xpNeed;
            
            BlockChance.text = " BlockChance: " + Mathf.RoundToInt(war.BlockChance * 100) + "%";

            if (war.gameObject.CompareTag("Team2"))
            {
                HpBar.fillRect.GetComponent<Image>().color = hpBarRedColor;
                HpBarRegenable.fillRect.GetComponent<Image>().color = hpBarRedColorRegen;
            }
            else if (war.gameObject.CompareTag("Team1"))
            {
                HpBar.fillRect.GetComponent<Image>().color = hpBarGreenColor;
                HpBarRegenable.fillRect.GetComponent<Image>().color = hpBarGreenColorRegen;
            }
            HpBar.maxValue = war.MaxHP;
            HpBar.value = war.HP;
            HpBarRegenable.maxValue = HpBar.maxValue;
            HpBarRegenable.value = war.HpBarRegenable.value;
            
            XpBar.maxValue = war.xpNeed;
            XpBar.value = war.xp;
            
            LVL.text = "LVL: " + war.UnitLVL;
            Damage.text = " Damage: " + war.DMG;
            
            
        }
        else if (Mose.SelectedUnits.Count > 1)
        {
            InfoPlate.SetActive(false);
            Forms.SetActive(true);
            UnitList.SetActive(true);
        }
        else
        {
            InfoPlate.SetActive(false);
            Forms.SetActive(false);
            UnitList.SetActive(false);
        }
        
    }


    public void UnitPageRight()
    {
        if (UnitsPage * 40 < Mose.SelectedUnits.Count)
        {
            UnitsPage++;
        }
        ClearUnitList();
        SetUnitList(Mose.SelectedUnits);
    }

    public void UnitPageLeft()
    {
        if (UnitsPage > 1)
        {
            UnitsPage--;
        }
        ClearUnitList();
        SetUnitList(Mose.SelectedUnits);
    }

    public  void SetUnitList(List<GameObject> units)
    {
        GameObject newFastDelete = Instantiate(FastDelete);
        newFastDelete.transform.SetParent(UnitList.transform);
        newFastDelete.transform.localScale = new Vector3(1, 1, 1);
        newFastDelete.transform.localRotation = Quaternion.identity;
        FastDeleteSave = newFastDelete;
        for (int i = 40 * (UnitsPage - 1); i < Mathf.Min(units.Count, UnitsPage * 40); i++)
        {
            Vector3 xStep = StepX.position - FirstPos.position;
            Vector3 yStep = StepY.position - FirstPos.position;
            Vector3 pos = FirstPos.position + xStep * (i % 40 % 8) + yStep * (i % 40 / 8);
            
            GameObject newIcon = Instantiate(UnitIcon);
            newIcon.transform.SetParent(newFastDelete.transform);
            newIcon.transform.position = pos;
            newIcon.transform.localScale = UnitIcon.transform.localScale;
            newIcon.transform.localRotation = Quaternion.identity;
            newIcon.GetComponent<Icon>().Unit = units[i];
            IconsList.Add(newIcon);
        }
    }

    public void ClearUnitList()
    {
        Destroy(FastDeleteSave.gameObject);
    }

    public void SetFormLine()
    {
        ArmyManager.formation = ArmyManager.Formation.Line;
        LineForm.GetComponent<Image>().color = Color.green;
        SquareForm.GetComponent<Image>().color = Color.white;
        JapanForm.GetComponent<Image>().color = Color.white;
        TriangleForm.GetComponent<Image>().color = Color.white;
    }
    public void SetFormSquare()
    {
        ArmyManager.formation = ArmyManager.Formation.Square;
        LineForm.GetComponent<Image>().color = Color.white;
        SquareForm.GetComponent<Image>().color = Color.green;
        JapanForm.GetComponent<Image>().color = Color.white;
        TriangleForm.GetComponent<Image>().color = Color.white;
    }
    public void SetFormChaotic()
    {
        ArmyManager.formation = ArmyManager.Formation.Chaotic;
        LineForm.GetComponent<Image>().color = Color.white;
        SquareForm.GetComponent<Image>().color = Color.white;
        JapanForm.GetComponent<Image>().color = Color.green;
        TriangleForm.GetComponent<Image>().color = Color.white;
    }
    public void SetFormTriangle()
    {
        ArmyManager.formation = ArmyManager.Formation.Triangle;
        LineForm.GetComponent<Image>().color = Color.white;
        SquareForm.GetComponent<Image>().color = Color.white;
        JapanForm.GetComponent<Image>().color = Color.white;
        TriangleForm.GetComponent<Image>().color = Color.green;
    }
    
}
