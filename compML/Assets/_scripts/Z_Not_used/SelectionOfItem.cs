using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionOfItem : MonoBehaviour
{
    static public int elementToMove;

    [SerializeField]
    private GameObject selectionHighlight;

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
                    Transform ElemPosition = hit.collider.GetComponent<Transform>();
                    HighlightSelectedObject(ElemPosition);
                }
            }
        }
    }

    private void HighlightSelectedObject(Transform ElemPosition)
    {
        Mesh mesh = ElemPosition.GetComponent<MeshFilter>().mesh;
        Bounds bounds = mesh.bounds;
        float diameter = bounds.size.x;
        diameter *= 1.50f;
        selectionHighlight.SetActive(true);
        selectionHighlight.transform.localScale = new Vector3(diameter, 1, diameter);
        selectionHighlight.transform.position = ElemPosition.position;
        selectionHighlight.transform.SetParent(ElemPosition);
    }

}
