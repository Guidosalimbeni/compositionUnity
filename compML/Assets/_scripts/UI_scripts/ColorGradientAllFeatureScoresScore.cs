﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGradientAllFeatureScoresScore : MonoBehaviour
{
    public RawImage AllFeaturesImg;
    public Text text;
    private Color lerpedColor = Color.white;

    private InferenceScoreFeatures inferenceScoreFeatures;

    private void Awake()
    {
        inferenceScoreFeatures = FindObjectOfType<InferenceScoreFeatures>();

        inferenceScoreFeatures.OnScoreAllscoresfeatures += Handle_OnScoreAllscoresfeatures;
    }

    private void Handle_OnScoreAllscoresfeatures(float scoreAllFeaturesScore)
    {
        UpdateLawOfLeverPixelsUI(scoreAllFeaturesScore);
        if (text != null)
            text.text = scoreAllFeaturesScore.ToString();
    }


    public void UpdateLawOfLeverPixelsUI(float score)
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
        AllFeaturesImg.color = lerpedColor;
    }

    private void OnDisable()
    {

        inferenceScoreFeatures.OnScoreAllscoresfeatures -= Handle_OnScoreAllscoresfeatures;

    }
}
