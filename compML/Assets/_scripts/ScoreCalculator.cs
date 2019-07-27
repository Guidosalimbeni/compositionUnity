﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    // this holds the score... woud be better to have the UI gradient to look at this values instead of subscribe to the events again...TODO

    public float visualScoreBalancePixelsCount { private set; get; } // this is going to be called screen space score to justify being inside the unity side of features extr scores
    public float scoreBoundsBalance { private set; get; } //
    public float scoreUnityVisual { private set; get; } //
    public float scoreLawOfLever { private set; get; } //
    public float scoreIsolationBalance { private set; get; } //

    public float scoreMobileNet { private set; get; } //
    public float scoreNNFrontTop { private set; get; } //
    public float scoreAllscorefeatures { private set; get; } //

    private OpenCVManager openCvManager;
    private GameVisualManager gameManagerNotOpenCV;
    private InferenceNNfomDATABASE inferenceNNfomDATABASE;
    private InferenceCompositionML inferenceCompositionML;
    private InferenceScoreFeatures inferenceScoreFeatures;

    private void Awake()
    {
        openCvManager = FindObjectOfType<OpenCVManager>();
        gameManagerNotOpenCV = FindObjectOfType<GameVisualManager>();
        
        inferenceNNfomDATABASE = FindObjectOfType<InferenceNNfomDATABASE>();

        inferenceCompositionML = FindObjectOfType<InferenceCompositionML>();
        inferenceScoreFeatures = FindObjectOfType<InferenceScoreFeatures>();

        openCvManager.OnPixelsCountBalanceChanged += HandleOnPixelsCountBalanceChanged;
        gameManagerNotOpenCV.OnScoreBoundsBalanceChanged += Handle_OnScoreBoundsBalanceChanged;
        gameManagerNotOpenCV.OnScoreUnityVisualChanged += Handle_OnScoreUnityVisualChanged;
        gameManagerNotOpenCV.OnScoreLawOfLeverChanged += Handle_OnScoreLawOfLeverChanged;
        gameManagerNotOpenCV.OnScoreIsolationBalanceChanged += Handle_OnScoreIsolationBalanceChanged;

        inferenceNNfomDATABASE.OnScorescoreNNFrontTopChanged += Handle_OnScorescoreNNFrontTopChanged;
        inferenceCompositionML.OnScorescoreMobileNetChanged += Handle_OnScorescoreMobileNetFrontTopChanged;

        inferenceScoreFeatures.OnScoreAllscoresfeatures += Handle_OnScoreAllscoresfeatures;

    }

    private void Handle_OnScoreAllscoresfeatures(float scoreAllscorefeaturesPassed)
    {
        scoreAllscorefeatures = scoreAllscorefeaturesPassed;
    }

    private void Handle_OnScorescoreMobileNetFrontTopChanged(float scoreMobileNetPassed)
    {
        scoreMobileNet = scoreMobileNetPassed;
    }

    private void Handle_OnScorescoreNNFrontTopChanged(float scoreNNFrontTopPassed)
    {
        scoreNNFrontTop = scoreNNFrontTopPassed;
    }

    private void Handle_OnScoreLawOfLeverChanged(float scoreLawOfLeverPassed)
    {
        scoreLawOfLever = scoreLawOfLeverPassed;
    }

    private void Handle_OnScoreIsolationBalanceChanged(float scoreIsolationBalancePassed)
    {
        scoreIsolationBalance = scoreIsolationBalancePassed;
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
        gameManagerNotOpenCV.OnScoreLawOfLeverChanged -= Handle_OnScoreLawOfLeverChanged;
        gameManagerNotOpenCV.OnScoreIsolationBalanceChanged -= Handle_OnScoreIsolationBalanceChanged;

        inferenceNNfomDATABASE.OnScorescoreNNFrontTopChanged -= Handle_OnScorescoreNNFrontTopChanged;
        inferenceCompositionML.OnScorescoreMobileNetChanged -= Handle_OnScorescoreMobileNetFrontTopChanged;
        inferenceScoreFeatures.OnScoreAllscoresfeatures -= Handle_OnScoreAllscoresfeatures;
    }

}
