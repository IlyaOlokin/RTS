using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpot : MonoBehaviour
{
    public List<GameObject> UnitsInSpot;
    public List<Vector3> StartPositions;
    
    public int spotRange;
    
    public bool aggred;
    
    
    protected void Start()
    {
        foreach (var unit in GameObject.FindGameObjectsWithTag("Team2"))
        {
            if (Vector3.Distance(unit.transform.position, transform.position) <= spotRange && unit.GetComponent<Warrior>())
            {
                unit.GetComponent<Unit>().NativeSpot = this;
                UnitsInSpot.Add(unit);
            }
        }
        for (int i = 0; i < UnitsInSpot.Count; i++)
        {
            StartPositions.Add(UnitsInSpot[i].transform.position);
        }
        
    }

    public void Update()
    {
        if (UnitsInSpot.Count == 0)
        {
            Destroy(this);
        }
    }

    public void ReturnToStartPositions(bool ifAttack)
    {
        for (int i = 0; i < UnitsInSpot.Count; i++)
        {
            UnitsInSpot[i].GetComponent<Unit>().AddPath(StartPositions[i], ifAttack, true, false);
        }
    }
    
    protected void SetVision()
    {
        foreach (GameObject unit in UnitsInSpot)
        {
            unit.GetComponent<Unit>().VisionPoint.transform.position = transform.position;
            unit.GetComponent<Unit>().vision = spotRange;
        }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spotRange);
    }
}
