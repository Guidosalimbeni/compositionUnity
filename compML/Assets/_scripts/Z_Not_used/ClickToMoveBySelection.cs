using UnityEngine;
using System.Collections;

public class ClickToMoveBySelection : MonoBehaviour
{
    // NOT USED since I am using LEAN TOUCh
    // this need Selection of itme attached to  game manager if I want to put it back and not using LEan touch ... 
    // also in population manager many things to uncommment and put back to life in case..


    public float moveSpeed;                     // The Speed the character will move
    public float RotationSpeed = -90f;
    public float minThreshold = 0.05f;
    public bool AIisPlaying = false;
    private Transform myTransform;              // this transform
    private Vector3 destinationPosition;        // The destination Point
    private float destinationDistance;          // The distance between myTransform and destinationPosition

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
            myTransform = transform; /////

            if (AIisPlaying == false)
            {
                PerformMovement();
            }
            if (AIisPlaying == true)
            {
                destinationPosition = myTransform.position; /// 
            }
        }
    }

    public void SetLastDestinationPositionCorrectlyFromAI(float x, float y, float z)
    {
        destinationPosition = new Vector3 (x,y,z);
        myTransform.position = Vector3.MoveTowards(myTransform.position, destinationPosition, moveSpeed * Time.deltaTime);

    }

    private void PerformMovement()
    {
        destinationDistance = Vector3.Distance(destinationPosition, myTransform.position);

        if (destinationDistance < minThreshold)
        {       
            moveSpeed = 0.1f;
        }
        else if (destinationDistance > minThreshold)
        {           
            moveSpeed = 3;
        }
        

//#if UNITY_STANDALONE || UNITY_WEBGL
        if (Input.GetKeyDown(KeyCode.Mouse0) && GUIUtility.hotControl == 0) // Input.GetMouseButton(0)
        {

            Plane playerPlane = new Plane(Vector3.up, myTransform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;

            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                destinationPosition = ray.GetPoint(hitdist);
            }
        }
//#endif


/*
#if UNITY_ANDROID || UNITY_WEBGL


        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];
            //Check if the phase of that touch equals Began
            if (myTouch.phase == TouchPhase.Began)
            {
                Plane playerPlane = new Plane(Vector3.up, myTransform.position);
                Ray ray = Camera.main.ScreenPointToRay(myTouch.position);
                float hitdist = 0.0f;

                if (playerPlane.Raycast(ray, out hitdist))
                {
                    Vector3 targetPoint = ray.GetPoint(hitdist);
                    destinationPosition = ray.GetPoint(hitdist);

                }

            }

        }

        
#endif
*/
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


