﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Game_Manager : MonoBehaviour
{
    public AreasObjects areasObjects { get; private set; }
    public static Game_Manager Instance { get; private set; }
    public List<GameObject> elementsOfComposition { get; private set; }

    private CalculateBalanceAreaBounds calculateBalanceAreaBounds;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            calculateBalanceAreaBounds = GetComponent<CalculateBalanceAreaBounds>();
            PopulateListOfItemInComposition();
            SetLayerToForeground();
            SetIdsToElementsOfCompositions();

            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        areasObjects = calculateBalanceAreaBounds.CalculateBondsAreas();
    }

    private void PopulateListOfItemInComposition()
    {
        elementsOfComposition = new List<GameObject>();
        var ss = FindObjectsOfType<MonoBehaviour>().OfType<IElementOfComposition>();
        foreach (IElementOfComposition s in ss)
        {
            elementsOfComposition.Add(s.TagMe());
        }
    }

    private void SetLayerToForeground()
    {
        foreach (var item in elementsOfComposition)
        {
            item.layer = 9; // set layer to Foreground
        }
    }

    private void SetIdsToElementsOfCompositions()
    {
        int ID = 0;

        foreach (var item in elementsOfComposition)
        {
            
            var TagMeComponent = item.GetComponent<TagMeElementOfComposition>();
            TagMeComponent.SetIdElementOfComposition(ID);
            ID++;
        }

    }

}
