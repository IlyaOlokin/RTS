using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PermanentBuffs : MonoBehaviour
{
    public RestartSessionAndStore rsas;

    public Text swordHpTxt;
    public Text spearAsTxt;
    public Text archerMsTxt;
    public Text axeDmgTxt;

    public int swordHpCost;
    public int spearAsCost;
    public int archerMsCost;
    public int axeDmgCost;


    private void Start()
    {
        CostTextUpdate();
    }

    void CostTextUpdate()
    {
        swordHpTxt.text = swordHpCost.ToString();
        spearAsTxt.text = spearAsCost.ToString();
        archerMsTxt.text = archerMsCost.ToString();
        axeDmgTxt.text = axeDmgCost.ToString();
    }


    public void BarracksUP()
    {
        if (RestartSessionAndStore.Money >= swordHpCost)
        {
            rsas.SwordBonusHP += 50;
            RestartSessionAndStore.Money -= swordHpCost;
            swordHpCost *= 5;
            CostTextUpdate();
        }
    }
    public void TrainingAreaUP()
    {
        if (RestartSessionAndStore.Money >= spearAsCost)
        {
            rsas.SpearBonusAS += 0.4f;
            RestartSessionAndStore.Money -= spearAsCost;
            spearAsCost *= 5;
            CostTextUpdate();
        }
    }
    public void SawmillUP()
    {
        if (RestartSessionAndStore.Money >= archerMsCost)
        {
            rsas.ArcherBonusMS += 1.5f;
            RestartSessionAndStore.Money -= archerMsCost;
            archerMsCost *= 5;
            CostTextUpdate();
        }
    }
    public void ForgeUP()
    {
        if (RestartSessionAndStore.Money >= axeDmgCost)
        {
            rsas.AxeBonusDMG += 15;
            RestartSessionAndStore.Money -= axeDmgCost;
            axeDmgCost *= 5;
            CostTextUpdate();
        }
    }
}
