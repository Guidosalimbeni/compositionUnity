using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalScoreUI : MonoBehaviour
{
    private OpenCVManager openCvManager;
    private Game_Manager gameManagerNotOpenCV;

    public float TOTALSCOREDebugging;

    float a;
    float b;
    float c;

    private void Awake()
    {

        openCvManager = FindObjectOfType<OpenCVManager>();
        gameManagerNotOpenCV = FindObjectOfType<Game_Manager>();

        openCvManager.OnPixelsCountBalanceChanged += HandleOnPixelsCountBalanceChanged;
        gameManagerNotOpenCV.OnScoreBoundsBalanceChanged += Handle_OnScoreBoundsBalanceChanged;
        gameManagerNotOpenCV.OnScoreUnityVisualChanged += Handle_OnScoreUnityVisualChanged;
    }

    private void Update()
    {
        TOTALSCOREDebugging = a + b + c;
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
}
