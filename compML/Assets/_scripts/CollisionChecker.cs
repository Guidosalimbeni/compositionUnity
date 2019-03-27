using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{

    private CalculateCollisionDistanceVisualUnity calculateCollisionDistanceVisualUnity;

    private void Awake()
    {
        calculateCollisionDistanceVisualUnity = FindObjectOfType<CalculateCollisionDistanceVisualUnity>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.GetComponent<TagMeElementOfComposition>() != null)
        {
            calculateCollisionDistanceVisualUnity.FoundCollisionOfCompositionalElements();
        }
    }

}
