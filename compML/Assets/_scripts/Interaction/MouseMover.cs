using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseMover : MonoBehaviour
{
    public float Speed = 0.05f;

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Vector2 mousePosition = Input.mousePosition;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out hit, float.MaxValue))
            {
                //float xAngle = Input.GetAxis("Mouse Y") * Speed * 120;
                float xAngle = 0;
                float yAngle = -Input.GetAxis("Mouse X") * Speed * 120;
                float zAngle = 0;

                MouseMover mouseMover = hit.collider.GetComponent<MouseMover>();
                if (mouseMover != null)
                {
                    hit.transform.Rotate(xAngle, yAngle, zAngle, Space.World);
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Vector2 mousePosition = Input.mousePosition;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out hit, float.MaxValue))
            {
                float xMovement = Input.GetAxis("Mouse X") * Speed;
                float yMovement = 0;
                float zMovement = Input.GetAxis("Mouse Y") * Speed; ;
                MouseMover mouseMover = hit.collider.GetComponent<MouseMover>();
                if (mouseMover != null)
                {
                    hit.transform.Translate(xMovement, yMovement, zMovement, Space.World);
                }
            }
        }
    }
}

