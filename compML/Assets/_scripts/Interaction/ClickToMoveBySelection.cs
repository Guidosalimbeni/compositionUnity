using UnityEngine;
using System.Collections;

public class ClickToMoveBySelection : MonoBehaviour
{
    private Transform myTransform;              // this transform
    private Vector3 destinationPosition;        // The destination Point
    private float destinationDistance;          // The distance between myTransform and destinationPosition

    public float moveSpeed;                     // The Speed the character will move
    public float RotationSpeed = -90f;
    public float minThreshold = 0.05f;


    private void Start()
    {
        myTransform = transform;
        destinationPosition = myTransform.position;
    }

    void Update()
    {
        int elementToMove = SelectionOfItem.elementToMove;
     
        if (transform.GetComponent<TagMeElementOfComposition>().ElementOfCompositionID == elementToMove)
        {
            myTransform = transform;
            PerformMovement();
            
        }
        else
        {
            Debug.Log("no item selected");
        }
  
    }

    private void PerformMovement()
    {
        // keep track of the distance between this gameObject and destinationPosition
        destinationDistance = Vector3.Distance(destinationPosition, myTransform.position);

        if (destinationDistance < minThreshold)
        {       // To prevent shakin behavior when near destination
            moveSpeed = 0;
        }
        else if (destinationDistance > minThreshold)
        {           // To Reset Speed to default
            moveSpeed = 3;
        }

        // Moves the Player if the Left Mouse Button was clicked
        if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
        {

            Plane playerPlane = new Plane(Vector3.up, myTransform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;

            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                destinationPosition = ray.GetPoint(hitdist);
                //Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                //myTransform.rotation = targetRotation;
            }
        }

        // Moves the player if the mouse button is hold down
        else if (Input.GetMouseButton(0) && GUIUtility.hotControl == 0)
        {

            Plane playerPlane = new Plane(Vector3.up, myTransform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;

            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                destinationPosition = ray.GetPoint(hitdist);
                //Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                //myTransform.rotation = targetRotation;
            }
            //myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPosition, moveSpeed * Time.deltaTime);
        }

        // To prevent code from running if not needed
        if (destinationDistance > minThreshold)
        {
            myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPosition, moveSpeed * Time.deltaTime);
        }

        else if (Input.GetMouseButton(2) && GUIUtility.hotControl == 0)
        {
            myTransform.Rotate(0, Input.GetAxis("Mouse X") * Time.deltaTime * RotationSpeed, 0);

        }


    }

}


