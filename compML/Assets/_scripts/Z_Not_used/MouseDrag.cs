using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    public Camera miniCamera;

    [SerializeField]
    private float x_dumb = 0.1f;
    [SerializeField]
    private float z_dump = 0.1f;

    void OnMouseDown()
    {
        screenPoint = miniCamera.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - miniCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
    }

    void OnMouseDrag()
    {
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        Vector3 cursorPosition = miniCamera.ScreenToWorldPoint(cursorPoint) + offset;
        cursorPosition.x *= x_dumb;
        cursorPosition.z *= z_dump;
        transform.position = cursorPosition;
    }
}
