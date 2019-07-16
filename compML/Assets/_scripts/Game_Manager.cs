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
    private CalculateCollisionDistanceVisualUnity calculateCollisionDistanceVisualUnity;

    public event Action<float> OnScoreBoundsBalanceChanged;

    public event Action<float> OnScoreUnityVisualChanged;


    private void Awake()
    {
        calculateBalanceAreaBounds = GetComponent<CalculateBalanceAreaBounds>();
        calculateCollisionDistanceVisualUnity = GetComponent<CalculateCollisionDistanceVisualUnity>();
        PopulateListOfItemInComposition();
        SetLayerToForeground();
        SetIdsToElementsOfCompositions();
    }

    // call from leantouch and population manager one during breeding and one for last move
    public void CallTOCalculateNOTOpenCVScores()
    {
        UpdateBalanceAreaBoundsShapes();
        UpdateScoreUnityVisual();



    }

    private void UpdateScoreUnityVisual()
    {
        float ScoreUnityVisual = calculateCollisionDistanceVisualUnity.CalculateUnityVisualScoreAndDrawLine();

        if (OnScoreUnityVisualChanged != null)
            OnScoreUnityVisualChanged(ScoreUnityVisual);
    }

    private void UpdateBalanceAreaBoundsShapes()
    {
        AreasOfObjects = calculateBalanceAreaBounds.CalculateBondsAreas();
        float leftWeight = AreasOfObjects.ObjectsLeftAreaPercentage;
        float RightWeight = AreasOfObjects.ObjectsRightAreaPercentage;
        float DifferenceBetweenLeftandRight = Mathf.Abs(leftWeight - RightWeight);
        float visualScoreBalanceBoundsShapes = 1 - ((DifferenceBetweenLeftandRight) / (leftWeight + RightWeight));

        if (OnScoreBoundsBalanceChanged != null)
            OnScoreBoundsBalanceChanged(visualScoreBalanceBoundsShapes);
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

// to fix this since already in population manager...
