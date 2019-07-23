using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    // this holds the score... woud be better to have the UI gradient to look at this values instead of subscribe to the events again...TODO

    public float visualScoreBalancePixelsCount { private set; get; } //
    public float scoreBoundsBalance { private set; get; } //
    public float scoreUnityVisual { private set; get; } //
    public float scoreLawOfLever { private set; get; } //
    public float scoreNNFrontTop { private set; get; } //

    private OpenCVManager openCvManager;
    private GameVisualManager gameManagerNotOpenCV;
    private InferenceNNfomDATABASE inferenceNNfomDATABASE;

    private InferenceCompositionML inferenceCompositionML;


    private void Awake()
    {
        openCvManager = FindObjectOfType<OpenCVManager>();
        gameManagerNotOpenCV = FindObjectOfType<GameVisualManager>();
        
        inferenceNNfomDATABASE = FindObjectOfType<InferenceNNfomDATABASE>();

        inferenceCompositionML = FindObjectOfType<InferenceCompositionML>();

        openCvManager.OnPixelsCountBalanceChanged += HandleOnPixelsCountBalanceChanged;
        gameManagerNotOpenCV.OnScoreBoundsBalanceChanged += Handle_OnScoreBoundsBalanceChanged;
        gameManagerNotOpenCV.OnScoreUnityVisualChanged += Handle_OnScoreUnityVisualChanged;
        gameManagerNotOpenCV.OnScoreLawOfLeverChanged += Handle_OnScoreLawOfLeverChanged;

        inferenceNNfomDATABASE.OnScorescoreNNFrontTopChanged += Handle_OnScorescoreNNFrontTopChanged;

    }

    private void Handle_OnScorescoreNNFrontTopChanged(float scoreNNFrontTopPassed)
    {
        scoreNNFrontTop = scoreNNFrontTopPassed;
    }

    private void Handle_OnScoreLawOfLeverChanged(float scoreLawOfLeverPassed)
    {
        scoreLawOfLever = scoreLawOfLeverPassed;
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
        inferenceNNfomDATABASE.OnScorescoreNNFrontTopChanged -= Handle_OnScorescoreNNFrontTopChanged;
    }

    public void GetTheScoreFromTheMobileNet()
    {

        // still need to add the last call from inference the same way i did for NN and check that is working properly

        //inferenceCompositionML.MakePrecitionCompMLMobileNet();

        Debug.Log("finish implementation here..");

    }

}
