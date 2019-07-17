using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePopulationController : MonoBehaviour
{
    
    public float MaxValues_x = 2.0f;
    public float MinValues_z = -2.0f;
    public List<GameObject> ElementsCompositions { get; set; }

    private void Start()
    {
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
    }

    private void SetLayerToForeground()
    {
        foreach (var item in ElementsCompositions)
        {
            item.layer = 9; // set layer to Foreground
        }
    }

}
