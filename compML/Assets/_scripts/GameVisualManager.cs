using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(CalculateCollisionDistanceVisualUnity))]

public class GameVisualManager : MonoBehaviour
{

    public bool CalculateAreaBounds = false;

    public AreasObjects AreasOfObjects { get; private set; }
    //public List<GameObject> ElementsOfComposition { get; private set; }

    private CalculateBalanceAreaBounds calculateBalanceAreaBounds;
    private CalculateCollisionDistanceVisualUnity calculateCollisionDistanceVisualUnity;
    private CalculateLawOfLever calculateLawOfLever;

    public event Action<float> OnScoreBoundsBalanceChanged;
    public event Action<float> OnScoreUnityVisualChanged;
    public event Action<float> OnScoreLawOfLeverChanged;

    private void Awake()
    {
        calculateBalanceAreaBounds = GetComponent<CalculateBalanceAreaBounds>();
        calculateCollisionDistanceVisualUnity = GetComponent<CalculateCollisionDistanceVisualUnity>();
        calculateLawOfLever = GetComponent<CalculateLawOfLever>();
    }

    // call from leantouch and population manager one during breeding and one for last move
    // also called from the AGENTCompAi to update the reward on decision on demand..
    public void CallTOCalculateNOTOpenCVScores()
    {
        if(CalculateAreaBounds == true)
        {
            UpdateBalanceAreaBoundsShapes();
        }
        
        UpdateScoreUnityVisual();
        UpdateLawOfLeverScore();




    }

    private void UpdateLawOfLeverScore()
    {
        float ScoreLawOfLever = calculateLawOfLever.CalculateLawOfLeverLeftRigth();
        if (OnScoreLawOfLeverChanged != null)
        {
            OnScoreLawOfLeverChanged(ScoreLawOfLever);
        }
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
}

// to fix this since already in population manager...
