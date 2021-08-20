using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ArmyManager : MonoBehaviour
{
    private Camera cam;
    public static float gap = 2;
    
    private Ray ray;
    private RaycastHit hit;

    private Vector3 Dest;
    private Vector3 dir;
    public static Formation formation;

    public bool CursorAttackState = false;

    public GameObject PointMoveAnimPrefab;



    public enum Formation
    {
        Line,
        Square,
        Chaotic,
        Triangle
    }
    


    void Start()
    {
        cam = Camera.main;
    }

    void MoveForOneUnit()
    {
        Mose.SelectedUnits[0].GetComponent<Unit>().AddPath(Mose.point, CursorAttackState, false, Input.GetKey(KeyCode.LeftShift));
        
        GetDestPoint();
        
        CreatePointMoveAnim(CursorAttackState);
        CursorAttackState = false;
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CursorAttackState = true;
        }

        if (Input.GetMouseButtonDown(1) && Mose.SelectedUnits.Count == 1)
        {
            CursorAttackState = false;
            MoveForOneUnit();
        }

        if (Input.GetMouseButtonDown(0) && CursorAttackState && Mose.SelectedUnits.Count == 1)
        {
            MoveForOneUnit();
        }
        
        if (Input.GetMouseButtonDown(1) && Mose.SelectedUnits.Count > 1)
        {
            CursorAttackState = false;
            GetDestPoint();
        }

        if (Input.GetMouseButtonDown(0) && CursorAttackState && Mose.SelectedUnits.Count > 1)
        {
            GetDestPoint();
            PathCalculation();
            CreatePointMoveAnim(true);
            CursorAttackState = false;
        }

        if (Input.GetMouseButtonUp(1) && Mose.SelectedUnits.Count > 1)
        {
            
            PathCalculation();
            
            
            CreatePointMoveAnim(false);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            CursorAttackState = false;
        }
        
    }

    void CreatePointMoveAnim(bool cursorAttackState)
    {
        GameObject newPoint = Instantiate(PointMoveAnimPrefab);
        newPoint.transform.position = new Vector3(Dest.x, Dest.y + 0.1f, Dest.z);
        newPoint.GetComponent<PointMoveAnim>().attacking = cursorAttackState;
    }
    
    void GetDestPoint()
    {
        Dest = Mose.point;
    }
    
    void PathCalculation()
    {
        dir = Mose.point - Dest;
        if (dir.magnitude == 0)
        {
            dir = (Dest - Mose.SelectedUnits[Mose.SelectedUnits.Count / 2].transform.position).normalized;
        }
        
        Vector3[] dirs;
        switch (formation)
        {
            case Formation.Line:
                dirs = GetLineFormation(Dest, dir, Mose.SelectedUnits.Count);
                break;
            case Formation.Square:
                dirs = GetSquareFormation(Dest, dir, Mose.SelectedUnits.Count);
                break;
            case Formation.Chaotic:
                dirs = GetChaoticallyFormation(Dest, dir, Mose.SelectedUnits.Count);
                break;
            case Formation.Triangle:
                dirs = GetTriangleFormation(Dest, dir, Mose.SelectedUnits.Count);
                break;
            default:
                dirs = GetSquareFormation(Dest, dir, Mose.SelectedUnits.Count);
                break;
        }
        
        NaivePath(dirs, Mose.SelectedUnits);
    }

    private void NaivePath(Vector3[] dirs, List<GameObject> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].GetComponent<Unit>().AddPath(dirs[i], CursorAttackState, false, Input.GetKey(KeyCode.LeftShift));
        }
        
        CursorAttackState = false;
    }
    

    private Vector3 Center(List<Vector3> units)
    {
        Vector3 center = new Vector3();
        foreach (var u in units)
        {
            center += u;
        }
        return center / units.Count;
    }
    
    
    
    
    private void PathCalculations(List<Vector3> dirs, List<GameObject> units)
    {
        Vector3 dirsCenter = Center(dirs);
        Vector3 unitCenter = Center(units.Select(u => u.transform.position).ToList());
        
        List<GameObject> orderedUnits = units.OrderBy(u => Vector3.Distance(dirsCenter, u.transform.position)).ToList();
        List<Vector3> orderedDirs = dirs.OrderByDescending(u => Vector3.Distance(unitCenter, u)).ToList();
        
        
        for (int i = 0; i < dirs.Count; i++)
        {
            orderedUnits[i].GetComponent<Unit>().AddPath(orderedDirs[i], CursorAttackState, false, false);
        }
    }

    public static Vector3[] GetSquareFormation(Vector3 dest, Vector3 dir, int selectedUnitsCount)
    {
        Vector3[] result = new Vector3[selectedUnitsCount];

        int side = Mathf.CeilToInt(Mathf.Sqrt(selectedUnitsCount));
        float offset = side / 2f * gap;
        Vector3 start = dest + Vector3.Cross(Vector3.up, dir).normalized * offset;
        var yStep = -dir.normalized * gap;
        var xStep = (dest - start).normalized * gap;
        
        for (int i = 0; i < selectedUnitsCount; i++)
        {
            result[i] = start + xStep * (i % side) + yStep * (i / side);
        }
        
        return result;
    }

    private Vector3[] GetChaoticallyFormation(Vector3 dest, Vector3 dir, int selectedUnitsCount)
    {
        Vector3[] result = new Vector3[selectedUnitsCount];
        
        
        Vector3 StartPoint = Center(Mose.SelectedUnits.Select(u => u.transform.position).ToList());
        
        for (int i = 0; i < selectedUnitsCount; i++)
        {
            Vector3 pos = Mose.SelectedUnits[i].transform.position - StartPoint;
            result[i] = dest + pos;
        }

        return result;
    }
    
    private Vector3[] GetLineFormation(Vector3 dest, Vector3 dir, int selectedUnitsCount)
    {
        float offset = (selectedUnitsCount - 1) / 2f * gap;
        Vector3 start = dest + Vector3.Cross(Vector3.up, dir).normalized * offset;
        var temp = (dest - start).normalized * gap;
        Vector3[] result = new Vector3[selectedUnitsCount]; 
        for (int i = 0; i < selectedUnitsCount; i++)
        {
            result[i] = start + temp * i;
        }
        return result;
    }

    private Vector3[] GetJapanFormation(Vector3 dest, Vector3 dir, int selectedUnitsCount)
    {
        if (selectedUnitsCount < 17)
        {
            return GetSquareFormation(dest, dir, selectedUnitsCount);
        }
        Vector3[] result = new Vector3[selectedUnitsCount];

        int side = Mathf.CeilToInt(Mathf.Sqrt(selectedUnitsCount + 8));
        float offset = side / 2f * gap;
        Vector3 start = dest + Vector3.Cross(Vector3.up, dir).normalized * offset;
        var yStep = -dir.normalized * gap;
        var xStep = (dest - start).normalized * gap;
        int curr = 0;
        int center = (side - 1) / 2;
        
        for (int i = 0; i < selectedUnitsCount + 8; i++)
        {
            int x = i % side;
            int y = i / side;
            if (Mathf.Abs(center - x) > 1 || Mathf.Abs(center - y) > 1 || (x == center && y == center))
            {
                result[curr] = start + xStep * x + yStep * y;
                curr++;
            }
        }
        return result;
    }
    
    private Vector3[] GetTriangleFormation(Vector3 dest, Vector3 dir, int selectedUnitsCount)
    {
        Vector3[] result = new Vector3[selectedUnitsCount];
        int curr = 0;
        var yStep = -dir.normalized * gap;
        var xStep = Vector3.Cross(Vector3.up, dir).normalized * gap;
        
        
        
        for (int i = 0; curr < selectedUnitsCount ; i++)
        {
            float offset = i / 2f;
            Vector3 start = dest - xStep * offset;
            
            for (int j = 0; j <= i && curr < selectedUnitsCount; j++)
            {
                result[curr] = start + xStep * j + yStep * i;
                curr++;
            }
        }
        return result;
    }
}
