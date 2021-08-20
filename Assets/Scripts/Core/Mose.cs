using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Mose : MonoBehaviour
{
    private Camera mainCamera;
    public static Vector3 point;

    public static String ControlledTeam;

    public static List<GameObject> SelectedUnits = new List<GameObject>();
    public GameObject[] SelectedUnits1;
    
    //private Rect rect;
    public LayerMask terr;

    
    public GameObject arrow;
    
    
    public bool Selected;
    public bool FindUnit;
    public Texture BoxSelect;
    private float _wight;
    private float _height;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Rect s_rect;
    public GameManager gc;
    public ArmyManager AM;
    public Interface _interface;


    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        ControlledTeam = "Team1";
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            foreach (var u in SelectedUnits)
            {
                u.GetComponent<Unit>().selected = false;
            }
            SelectedUnits.Clear();
            
            if (ControlledTeam[4] == '1')
            {
                ControlledTeam = "Team2";
            }
            else if (ControlledTeam[4] == '2')
            {
                ControlledTeam = "Team1";
            }
        }
        
        
        if (!Interface.isStarted)
        {
            return;
        }
        
        SelectedUnits1 = SelectedUnits.ToArray();
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(cameraRay, out hit, Mathf.Infinity, terr))
        {
            point = hit.point;
            Debug.DrawLine(cameraRay.origin, point, Color.black);
            
        }
        
        
        if (Input.GetMouseButtonDown(1))
        {
            arrow.transform.position = new Vector3(point.x, point.y + 0.1f, point.z);
            startPoint = Input.mousePosition;
            if (SelectedUnits.Count > 1)
            {
                arrow.SetActive(true);
            }
            
        }

        if (Input.GetMouseButton(1))
        {
            endPoint = Input.mousePosition;
            if (Vector3.Distance(startPoint, endPoint) > 30 && SelectedUnits.Count > 1)
            {
                arrow.transform.LookAt(new Vector3(point.x, 0.1f, point.z));
                arrow.SetActive(true);
            }
            else
            {
                arrow.SetActive(false);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            arrow.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(cameraRay, out hit, Mathf.Infinity, terr))
        {
            if (hit.collider.gameObject.layer.Equals(5))
            {
                return;
            }
            startPoint = Input.mousePosition;
            Selected = true;
        }

        if (Selected)
        {
            endPoint = Input.mousePosition;
            if (Input.GetMouseButtonUp(0))
            {
                float MinDist = 25;
                if (!Input.GetKey(KeyCode.LeftShift) && Vector3.Distance(startPoint,endPoint) > MinDist)
                {
                    for (int i = 0; i < SelectedUnits.Count; i++)
                    {
                        SelectedUnits[i].GetComponent<Unit>().selected = false;
                    }
                    SelectedUnits.Clear();
                }
                s_rect = SelectRect(startPoint, endPoint);
                FindUnit = true;
                Selected = false;
            }
        }
        else if (FindUnit)
        {
            if (ControlledTeam[4] == '1')
            {
                foreach (GameObject tmp in gc.Units1)
                {
                    var pos = mainCamera.WorldToScreenPoint(tmp.transform.position);
                    pos.y = Screen.height - pos.y;
                    if (s_rect.Contains(pos) && !SelectedUnits.Contains(tmp))
                    {
                        tmp.GetComponent<Unit>().selected = true;
                        SelectedUnits.Add(tmp);
                    }
                }
            }
            else if (ControlledTeam[4] == '2')
            {
                foreach (GameObject tmp in gc.EnemyUnits1)
                {
                    var pos = mainCamera.WorldToScreenPoint(tmp.transform.position);
                    pos.y = Screen.height - pos.y;
                    if (s_rect.Contains(pos) && !SelectedUnits.Contains(tmp))
                    {
                        tmp.GetComponent<Unit>().selected = true;
                        SelectedUnits.Add(tmp);
                    }
                }
            }
            _interface.ClearUnitList();
            if (SelectedUnits.Count > 1)
            {
                _interface.UnitsPage = 1;
                _interface.SetUnitList(SelectedUnits);
            }

            FindUnit = false;
            
        }

        
    }

    private Rect SelectRect(Vector3 _start, Vector3 _end)
    {
        if (_wight < 0)
        {
            _wight = Mathf.Abs(_wight);
        }
        if (_height < 0)
        {
            _height = Mathf.Abs(_height);
        }

        if (endPoint.x < startPoint.x)
        {
            _start.z = _start.x;
            _start.x = _end.x;
            _end.x = _start.z;
        }

        if (endPoint.y > startPoint.y)
        {
            _start.z = _start.y;
            _start.y = _end.y;
            _end.y = _start.z;
        }
        return new Rect(_start.x, (Screen.height - _start.y), _wight, _height);
    }

    private void OnGUI()
    {
        if (Selected)
        {
            _wight = endPoint.x - startPoint.x;
            _height = Screen.height - endPoint.y - (Screen.height - startPoint.y);
            GUI.DrawTexture(new Rect(startPoint.x, (Screen.height - startPoint.y), _wight, _height), BoxSelect);
        }
    }
} 
  
