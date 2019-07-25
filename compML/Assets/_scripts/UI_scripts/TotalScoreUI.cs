using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalScoreUI : MonoBehaviour
{
    private OpenCVManager openCvManager;
    private GameVisualManager gameManagerNotOpenCV;
    private InferenceCompositionML inferenceCompositionML;
    private InferenceNNfomDATABASE inferenceNNfomDATABASE;

    public float TOTALSCOREDebugging;

    float a;
    float b;
    float c;
    float d;
    float e;
    float f;

    private void Awake()
    {

        openCvManager = FindObjectOfType<OpenCVManager>();
        gameManagerNotOpenCV = FindObjectOfType<GameVisualManager>();
        inferenceCompositionML = FindObjectOfType<InferenceCompositionML>();
        inferenceNNfomDATABASE = FindObjectOfType<InferenceNNfomDATABASE>();

        openCvManager.OnPixelsCountBalanceChanged += HandleOnPixelsCountBalanceChanged;
        gameManagerNotOpenCV.OnScoreBoundsBalanceChanged += Handle_OnScoreBoundsBalanceChanged;
        gameManagerNotOpenCV.OnScoreUnityVisualChanged += Handle_OnScoreUnityVisualChanged;
        gameManagerNotOpenCV.OnScoreLawOfLeverChanged += Handlex_OnScoreLawOfLeverChanged;
        inferenceCompositionML.OnScorescoreMobileNetChanged += Handle_OnScorescoreMobileNetChanged;
        inferenceNNfomDATABASE.OnScorescoreNNFrontTopChanged += Handle_OnScorescoreNNFrontTopChanged;



    }

    private void Handle_OnScorescoreNNFrontTopChanged(float obj)
    {
        f = obj;
    }

    private void Handle_OnScorescoreMobileNetChanged(float obj)
    {
        e = obj;
    }

    private void Handlex_OnScoreLawOfLeverChanged(float obj)
    {
        d = obj;
    }

    private void Update()
    {
        TOTALSCOREDebugging = a + b + c + d + e + f;
    }

    private void Handle_OnScoreUnityVisualChanged(float obj)
    {
        a = obj;
    }

    private void Handle_OnScoreBoundsBalanceChanged(float obj)
    {
        b = obj;
    }

    private void HandleOnPixelsCountBalanceChanged(float obj)
    {
        c = obj;
    }

    private void OnDisable()
    {
        openCvManager.OnPixelsCountBalanceChanged -= HandleOnPixelsCountBalanceChanged;
        gameManagerNotOpenCV.OnScoreBoundsBalanceChanged -= Handle_OnScoreBoundsBalanceChanged;
        gameManagerNotOpenCV.OnScoreUnityVisualChanged -= Handle_OnScoreUnityVisualChanged;
        gameManagerNotOpenCV.OnScoreLawOfLeverChanged -= Handlex_OnScoreLawOfLeverChanged;
        inferenceCompositionML.OnScorescoreMobileNetChanged -= Handle_OnScorescoreMobileNetChanged;
        inferenceNNfomDATABASE.OnScorescoreNNFrontTopChanged -= Handle_OnScorescoreNNFrontTopChanged;
    }

}
