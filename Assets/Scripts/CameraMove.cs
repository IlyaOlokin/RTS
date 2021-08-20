using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float speed;
    public float BorderThickness;
    public Vector2 Limit;
    public float miny;
    public float maxy;
    public float scrollSpeed;

    public bool camLocked = false;
   
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            camLocked = !camLocked;
        }

        if (!camLocked)
        {
            Vector3 pos = transform.position;
        
            if ((Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - BorderThickness) && Interface.isStarted )
            {
                pos.z += speed;
            }
            if ((Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= BorderThickness) && Interface.isStarted)
            {
                pos.z -= speed;
            }
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= BorderThickness) && Interface.isStarted)
            {
                pos.x -= speed;
            }
            if ((Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - BorderThickness) && Interface.isStarted)
            {
                pos.x += speed;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll * scrollSpeed * Time.deltaTime;

            pos.x = Mathf.Clamp(pos.x, -Limit.x, Limit.x);
            pos.y = Mathf.Clamp(pos.y, miny, maxy);
            pos.z = Mathf.Clamp(pos.z, -Limit.y, Limit.y);

            transform.position = pos;
        }
        
    }
}