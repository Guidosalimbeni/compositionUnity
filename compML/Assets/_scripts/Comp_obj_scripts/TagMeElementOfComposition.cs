using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagMeElementOfComposition : MonoBehaviour, IElementOfComposition
{
    public GameObject TagMe()
    {
        return gameObject;
    }
    
}
