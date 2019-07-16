﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGradientBoundShapeBalance : MonoBehaviour
{
    public RawImage balanceBoundsShapesUI;

    private float visualScoreBalanceBoundsShapes;
    Color lerpedColor = Color.white;
    private AreasObjects areaobjects;

    private Game_Manager gameManagerNotOpenCV;

    private void Awake()
    {
        gameManagerNotOpenCV = FindObjectOfType<Game_Manager>();
        gameManagerNotOpenCV.OnScoreBoundsBalanceChanged += HandleOnScoreBoundsBalanceChanged;
        balanceBoundsShapesUI = balanceBoundsShapesUI.GetComponent<RawImage>();
    }

    private void HandleOnScoreBoundsBalanceChanged(float visualScoreBalanceBoundsShapes)
    {
        UpdateBalancePixelsUI(visualScoreBalanceBoundsShapes);
    }


    // need to subscribe to an event as I did for the open cv

    //private void Update()
    //{
    //    areaobjects = gameManagerNotOpenCV.AreasOfObjects;  // not really need to pass from game manager
    //    float leftWeight = areaobjects.ObjectsLeftAreaPercentage;
    //    float RightWeight = areaobjects.ObjectsRightAreaPercentage;

    //    float DifferenceBetweenLeftandRight = Mathf.Abs(leftWeight - RightWeight);
    //    visualScoreBalanceBoundsShapes = 1 - ((DifferenceBetweenLeftandRight) / (leftWeight + RightWeight));

    //    UpdateBalancePixelsUI(visualScoreBalanceBoundsShapes);

    //}

    public float GetvisualScoreBalanceBoundsShapes() // who is calling this?
    {
        return visualScoreBalanceBoundsShapes;
    }

    public void UpdateBalancePixelsUI(float score)
    {
        if (score < 0.8f)
        {
            lerpedColor = Color.Lerp(Color.red * 0.8f, Color.yellow, score);
            balanceBoundsShapesUI.color = lerpedColor;
        }
        else
        {
            lerpedColor = Color.Lerp(Color.yellow, Color.green, score);
            balanceBoundsShapesUI.color = lerpedColor;
        }
    }
}
