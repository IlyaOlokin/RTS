using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Camp : EnemySpot
{
    void Start()
    {
        base.Start();
        SetVision();
    }


    void Update()
    {
        base.Update();
        
        foreach (var unit in UnitsInSpot)
        {
            if (Vector3.Distance(unit.transform.position, transform.position) > spotRange + 10)
            {
                ReturnToStartPositions(true);
                SetVision();
                aggred = false;
            }
        }

        if (!aggred)
        {
            SetVision();
        }
    }
    
    
}
