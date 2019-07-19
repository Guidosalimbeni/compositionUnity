﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    public float visualScoreBalancePixelsCount { private set; get; } //
    public float scoreBoundsBalance { private set; get; } //
    public float scoreUnityVisual { private set; get; } //

    private OpenCVManager openCvManager;
    private GameVisualManager gameManagerNotOpenCV;
    private InferenceCompositionML inferenceCompositionML;


    private void Awake()
    {
        openCvManager = FindObjectOfType<OpenCVManager>();
        gameManagerNotOpenCV = FindObjectOfType<GameVisualManager>();
        inferenceCompositionML = FindObjectOfType<InferenceCompositionML>();

        openCvManager.OnPixelsCountBalanceChanged += HandleOnPixelsCountBalanceChanged;
        gameManagerNotOpenCV.OnScoreBoundsBalanceChanged += Handle_OnScoreBoundsBalanceChanged;
        gameManagerNotOpenCV.OnScoreUnityVisualChanged += Handle_OnScoreUnityVisualChanged;
        
    }

    private void Handle_OnScoreUnityVisualChanged(float ScoreUnityVisualPassed)
    {
        scoreUnityVisual = ScoreUnityVisualPassed;
    }

    private void Handle_OnScoreBoundsBalanceChanged(float visualScoreBalanceBoundsShapes)
    {
        scoreBoundsBalance = visualScoreBalanceBoundsShapes;
    }

    private void HandleOnPixelsCountBalanceChanged(float scoreOnpixelscountbalance)
    {
        visualScoreBalancePixelsCount = scoreOnpixelscountbalance;
    }

    private void OnDisable()
    {
        openCvManager.OnPixelsCountBalanceChanged -= HandleOnPixelsCountBalanceChanged;
        gameManagerNotOpenCV.OnScoreBoundsBalanceChanged -= Handle_OnScoreBoundsBalanceChanged;
        gameManagerNotOpenCV.OnScoreUnityVisualChanged -= Handle_OnScoreUnityVisualChanged;
    }

    public void GetTheScoreFromTheMobileNet()
    {
        inferenceCompositionML.MakePrecitionCompMLMobileNet();
    }

}
