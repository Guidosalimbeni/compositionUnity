﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePopulationController : MonoBehaviour
{
    
    public float MaxValues_x = 2.0f;
    public float MinValues_z = -2.0f;
    public Transform parent;
    public List<GameObject> ElementsCompositions { get; set; } 

    private void Awake() // important to come first
    {
        if (GameManagerCompML.Instance.ElementsCompositionsSelected != null)
        {
            List<GameObject> prefabs = GameManagerCompML.Instance.ElementsCompositionsSelected;

            foreach (GameObject prefab in prefabs)
            {
                GameObject go = Instantiate(prefab, parent);
            }
        }

        
        // instantiate from game manager... (if there already in scene for debugging ... no problem...)

        PopulateTheElementsOfCompositionInTheScene();
        SetLayerToForeground();
    }

    private void PopulateTheElementsOfCompositionInTheScene() 
    {

        ElementsCompositions = new List<GameObject>();

        TagMeElementOfComposition[] elementstags = FindObjectsOfType<TagMeElementOfComposition>();
        foreach (var elementtag in elementstags)
        {
            GameObject go = elementtag.gameObject;
            ElementsCompositions.Add(go);
        }

        //debug
        foreach (var g in ElementsCompositions)
        {
            Debug.Log(g.name);
        }

    }

    private void SetLayerToForeground()
    {
        foreach (var item in ElementsCompositions)
        {
            item.layer = 9; // set layer to Foreground
        }
    }

}
