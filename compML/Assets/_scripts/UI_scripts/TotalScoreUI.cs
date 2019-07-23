using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalScoreUI : MonoBehaviour
{
    private OpenCVManager openCvManager;
    private GameVisualManager gameManagerNotOpenCV;

    public float TOTALSCOREDebugging;

    float a;
    float b;
    float c;
    float d;

    private void Awake()
    {

        openCvManager = FindObjectOfType<OpenCVManager>();
        gameManagerNotOpenCV = FindObjectOfType<GameVisualManager>();

        openCvManager.OnPixelsCountBalanceChanged += HandleOnPixelsCountBalanceChanged;
        gameManagerNotOpenCV.OnScoreBoundsBalanceChanged += Handle_OnScoreBoundsBalanceChanged;
        gameManagerNotOpenCV.OnScoreUnityVisualChanged += Handle_OnScoreUnityVisualChanged;
        gameManagerNotOpenCV.OnScoreLawOfLeverChanged += Handlex_OnScoreLawOfLeverChanged;
    }

    private void Handlex_OnScoreLawOfLeverChanged(float obj)
    {
        d = obj;
    }

    private void Update()
    {
        TOTALSCOREDebugging = a + b + c + d;
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
    }

}
