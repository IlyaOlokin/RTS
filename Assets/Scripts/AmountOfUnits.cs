using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AmountOfUnits : MonoBehaviour
{
    public GameObject restartSessionZone;
    private RestartSessionAndStore rsz;
    private Text txt;
    void Start()
    {
        rsz = restartSessionZone.GetComponent<RestartSessionAndStore>();
        txt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = "Swordsmen: " + rsz.SavedSwords.Sum() + System.Environment.NewLine +
                   "Spearmen: " + rsz.SavedSpears.Sum() + System.Environment.NewLine +
                   "Archers: " + rsz.SavedArchers.Sum() + System.Environment.NewLine +
                   "Axemen: " + rsz.SavedAxes.Sum();
    }
}
