using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    public float scorePixelsBalance { private set; get; }
    public float scoreBoundsBalance { private set; get; } //
    public float scoreUnityVisual { private set; get; } //

    //public float CurrentTotalScore;

    private OpenCVManager openCvManager;

    private ColorGradientBoundShapeBalance colorgradientBoundsShapeBalance; //
    private ColorGradientVisualUnity colorGradientVisualUnity; //

    private void Awake()
    {
        openCvManager = FindObjectOfType<OpenCVManager>();
        openCvManager.OnPixelsCountBalanceChanged += HandleOnPixelsCountBalanceChanged;
        
        colorgradientBoundsShapeBalance = GetComponent<ColorGradientBoundShapeBalance>();
        colorGradientVisualUnity = GetComponent<ColorGradientVisualUnity>();
    }
    

    private void HandleOnPixelsCountBalanceChanged(float scoreOnpixelscountbalance)
    {
        scorePixelsBalance = scoreOnpixelscountbalance;
    }

    private void Update()
    {
        scoreBoundsBalance = colorgradientBoundsShapeBalance.GetvisualScoreBalanceBoundsShapes();
        scoreUnityVisual = colorGradientVisualUnity.GetVisualUnityScore();

        //CurrentTotalScore = scorePixelsBalance + scoreBoundsBalance + scoreUnityVisual;
    }

}
