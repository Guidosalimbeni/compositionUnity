using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionOfItem : MonoBehaviour
{
    static public int elementToMove;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) //&& GUIUtility.hotControl == 0 || Input.GetMouseButtonDown(2)
        {
            Vector2 mousePosition = Input.mousePosition;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out hit, float.MaxValue))
            {
                TagMeElementOfComposition ElementOfComposition = hit.collider.GetComponent<TagMeElementOfComposition>();
                if (ElementOfComposition != null)
                {
                    elementToMove = ElementOfComposition.ElementOfCompositionID;
                }
            }

        }

    }

}
