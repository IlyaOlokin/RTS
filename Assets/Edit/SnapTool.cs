using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[EditorTool("Snap", typeof(SnapableObj))]

public class SnapTool : EditorTool
{
    public Texture2D Icon;

    public override GUIContent toolbarIcon

    {

        get { return new GUIContent(Icon, "SnapTool"); }

    }

    public override void OnToolGUI(EditorWindow window)
    {
        Transform targerTransform = ((SnapableObj) target).transform;
        EditorGUI.BeginChangeCheck();
        Vector3 newPoint = Handles.PositionHandle(targerTransform.position, Quaternion.identity);
        Quaternion newRotation = Handles.RotationHandle(targerTransform.rotation, targerTransform.position);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(targerTransform, name: " ");
            MoveWithSnapping(targerTransform, newPoint, newRotation);
        }
    }

    private void MoveWithSnapping(Transform targetTransform, Vector3 position, Quaternion rot)
    {
        SnapPoint[] allPoints = FindObjectsOfType<SnapPoint>();
        SnapPoint[] targetPoints = targetTransform.GetComponentsInChildren<SnapPoint>();

        Vector3 bestPosition = position;
        float closestDistance = float.PositiveInfinity;

        foreach (SnapPoint point in allPoints)
        {
            if (point.transform.parent == targetTransform) continue;

            foreach (SnapPoint targPoint in targetPoints)
            {
                Vector3 tagetPos = point.transform.position - (targPoint.transform.position - targetTransform.position);
                float distance = Vector3.Distance(tagetPos, position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestPosition = tagetPos;
                }
            }
        }

        if (closestDistance < 0.5f)
        {
            targetTransform.position = bestPosition;
        }
        else
        {
            targetTransform.position = position;
        }

        targetTransform.rotation = rot;
    }
}
