using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInterface : MonoBehaviour
{
    private static RestartSessionAndStore rsas;
    private Camp spot;

    public Canvas canvas;
    public GameObject zone;
    private int UnitsInArea;
    
    

    public int SwordmanCost = 300;
    public int SpearmanCost = 400;
    public int ArcherCost = 500;
    public int AxemanCost = 500;
    

    public Button SwordmanButton;
    public Button SpearmanButton;
    public Button ArcherButton;
    public Button AxemanButton;
    
    
    private void Start()
    {
        rsas = FindObjectOfType<RestartSessionAndStore>();
        spot = GetComponent<Camp>();
        
        UpdateTexts();
    }

    private void Update()
    {
        
        canvas.gameObject.SetActive(UnitsInArea > 0 && spot.UnitsInSpot.Count == 0);
        zone.gameObject.SetActive(spot.UnitsInSpot.Count == 0);
    }

    void UpdateTexts()
    {
        SwordmanButton.GetComponentInChildren<Text>().text = SwordmanCost + "$";
        SpearmanButton.GetComponentInChildren<Text>().text = SpearmanCost + "$";
        AxemanButton.GetComponentInChildren<Text>().text = AxemanCost + "$";
        ArcherButton.GetComponentInChildren<Text>().text = ArcherCost + "$";
    }
    
    public void SwordmanBaseLvlUp()
    {
        if (RestartSessionAndStore.Money >= SwordmanCost)
        {
            rsas.BaseSwordmanLVL += 1;
            RestartSessionAndStore.Money -= SwordmanCost;
            SwordmanCost = Mathf.RoundToInt(SwordmanCost * 1.5f);
            UpdateTexts();
        }
    }
    
    public void SpearmanBaseLvlUp()
    {
        if (RestartSessionAndStore.Money >= SpearmanCost)
        {
            rsas.BaseSwordmanLVL += 1;
            RestartSessionAndStore.Money -= SpearmanCost;
            SpearmanCost = Mathf.RoundToInt(SpearmanCost * 1.5f);
            UpdateTexts();
        }
    }
    
    public void AxemanBaseLvlUp()
    {
        if (RestartSessionAndStore.Money >= AxemanCost)
        {
            rsas.BaseSwordmanLVL += 1;
            RestartSessionAndStore.Money -= AxemanCost;
            AxemanCost *= 2;
            UpdateTexts();
        }
    }
    
    public void ArcherBaseLvlUp()
    {
        if (RestartSessionAndStore.Money >= ArcherCost)
        {
            rsas.BaseSwordmanLVL += 1;
            RestartSessionAndStore.Money -= ArcherCost;
            ArcherCost *= 2;
            UpdateTexts();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Team1"))
        {
            UnitsInArea++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Team1"))
        {
            UnitsInArea--;
        }
    }
}
