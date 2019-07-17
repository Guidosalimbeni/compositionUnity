using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ColorGradientVisualUnity : MonoBehaviour
{
    public RawImage VisualUnityUI;
    private Color lerpedColor = Color.white;
    private float VisualUnityScore;
    private GameVisualManager gamemanagerNotOpenCV;

    private void Awake()
    {
        //VisualUnityUI = VisualUnityUI.GetComponent<RawImage>();
        gamemanagerNotOpenCV = FindObjectOfType<GameVisualManager>();
        gamemanagerNotOpenCV.OnScoreUnityVisualChanged += Handle_OnScoreUnityVisualChanged;
    }

    private void Handle_OnScoreUnityVisualChanged(float VisualUnityScore)
    {
        UpdateBalancePixelsUI(VisualUnityScore); ;
    }


    //public float GetVisualUnityScore() // do i need this ???
    //{
    //    return VisualUnityScore;
    //}

    public void UpdateBalancePixelsUI(float score)
    {
        if (score < 0.8f)
        {
            lerpedColor = Color.Lerp(Color.red * 0.8f, Color.yellow, score);
            VisualUnityUI.color = lerpedColor;
        }
        else
        {
            lerpedColor = Color.Lerp(Color.yellow, Color.green, score);
            VisualUnityUI.color = lerpedColor;
        }
    }
}
