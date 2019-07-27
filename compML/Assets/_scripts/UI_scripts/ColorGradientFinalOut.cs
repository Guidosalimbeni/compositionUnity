﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGradientFinalOut : MonoBehaviour
{
    public RawImage FINALout;
    public Text text;
    private Color lerpedColor = Color.white;

    private InferenceFinalOut inferenceFinalOut;

    private void Awake()
    {
        inferenceFinalOut = FindObjectOfType<InferenceFinalOut>();

        inferenceFinalOut.OnScoreFinalOutChanged += Handle_OnScoreFinalOutChanged;
    }

    private void Handle_OnScoreFinalOutChanged(float scoreFinalPassed)
    {
        UpdateMobileScoreUI(scoreFinalPassed);
        if (text != null)
            text.text = scoreFinalPassed.ToString();
    }

    public void UpdateMobileScoreUI(float score)
    {
        float weight = 1.0f;

        if (score < 0.90f)
        {
            weight = 0.8f;
        }

        if (score < 0.85f)
        {
            weight = 0.5f;
        }
        if (score < 0.5f)
        {
            weight = 0.2f;
        }

        lerpedColor = Color.Lerp(Color.black, Color.white, score * weight);
        FINALout.color = lerpedColor;
    }

    private void OnDisable()
    {
        inferenceFinalOut.OnScoreFinalOutChanged -= Handle_OnScoreFinalOutChanged;
    }
}
