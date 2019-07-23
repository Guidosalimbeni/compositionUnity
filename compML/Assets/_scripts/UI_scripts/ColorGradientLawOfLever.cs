using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGradientLawOfLever : MonoBehaviour
{
    public RawImage LawOfLeverRawImage;
    private Color lerpedColor = Color.white;
    private GameVisualManager gameVisualManager;

    private void Awake()
    {
        gameVisualManager = FindObjectOfType<GameVisualManager>();
        gameVisualManager.OnScoreLawOfLeverChanged += Handle_OnScoreLawOfLeverChanged;
    }

    private void Handle_OnScoreLawOfLeverChanged(float scorelawOfLever)
    {
        UpdateLawOfLeverPixelsUI(scorelawOfLever);
    }

    public void UpdateLawOfLeverPixelsUI(float score)
    {

        if (score < 0.8f)
        {
            lerpedColor = Color.Lerp(Color.red * 0.8f, Color.yellow, score);
            LawOfLeverRawImage.color = lerpedColor;
        }
        else
        {
            lerpedColor = Color.Lerp(Color.yellow, Color.green, score);
            LawOfLeverRawImage.color = lerpedColor;
        }
    }

    private void OnDisable()
    {
        gameVisualManager.OnScoreLawOfLeverChanged -= Handle_OnScoreLawOfLeverChanged;
    }
}
