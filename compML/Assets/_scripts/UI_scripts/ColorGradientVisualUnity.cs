using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ColorGradientVisualUnity : MonoBehaviour
{
    public RawImage VisualUnityUI;
    private CalculateCollisionDistanceVisualUnity calculateCollisionDistanceVisualUnity; //
    private Color lerpedColor = Color.white;
    private float VisualUnityScore;
    private Game_Manager gamemanager;

    private void Awake()
    {
        VisualUnityUI = VisualUnityUI.GetComponent<RawImage>();
        gamemanager = FindObjectOfType<Game_Manager>();
        calculateCollisionDistanceVisualUnity = gamemanager.GetComponent<CalculateCollisionDistanceVisualUnity>();
    }

    private void Update()
    {
        VisualUnityScore = calculateCollisionDistanceVisualUnity.VisualUnityScore;
        UpdateBalancePixelsUI(VisualUnityScore);
    }

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
