using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(CalculateCollisionDistanceVisualUnity))]

public class Game_Manager : MonoBehaviour
{
    public AreasObjects AreasOfObjects { get; private set; }
    public List<GameObject> ElementsOfComposition { get; private set; }
    private CalculateBalanceAreaBounds calculateBalanceAreaBounds;
    
    private void Awake()
    {

        calculateBalanceAreaBounds = GetComponent<CalculateBalanceAreaBounds>();
        PopulateListOfItemInComposition();
        SetLayerToForeground();
        SetIdsToElementsOfCompositions();
        
    }

    private void Update()
    {
        AreasOfObjects = calculateBalanceAreaBounds.CalculateBondsAreas();
    }

    private void PopulateListOfItemInComposition()
    {
        ElementsOfComposition = new List<GameObject>();
        var ss = FindObjectsOfType<MonoBehaviour>().OfType<IElementOfComposition>();
        foreach (IElementOfComposition s in ss)
        {
            ElementsOfComposition.Add(s.TagMe());
        }
    }

    private void SetLayerToForeground()
    {
        foreach (var item in ElementsOfComposition)
        {
            item.layer = 9; // set layer to Foreground
        }
    }

    private void SetIdsToElementsOfCompositions()
    {
        int ID = 0;

        foreach (var item in ElementsOfComposition)
        {
            
            var TagMeComponent = item.GetComponent<TagMeElementOfComposition>();
            TagMeComponent.SetIdElementOfComposition(ID);
            ID++;
        }

    }

    public List<GameObject> getListOfItemInComposition()
    {
        return ElementsOfComposition;
    }

}
