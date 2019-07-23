using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGradientNNFrontTop : MonoBehaviour
{
    public RawImage NN_front_top_score_RawImage;
    private Color lerpedColor = Color.white;

    private InferenceNNfomDATABASE inferenceNNfomDATABASE;

    private void Awake()
    {
        inferenceNNfomDATABASE = FindObjectOfType<InferenceNNfomDATABASE>();

        inferenceNNfomDATABASE.OnScorescoreNNFrontTopChanged += Handle_OnScorescoreNNFrontTopChanged;
    }

    private void Handle_OnScorescoreNNFrontTopChanged(float scoreNNFrontTop)
    {
        UpdateLawOfLeverPixelsUI(scoreNNFrontTop);
    }

    public void UpdateLawOfLeverPixelsUI(float score)
    {

        if (score < 0.8f)
        {
            lerpedColor = Color.Lerp(Color.red * 0.8f, Color.yellow, score);
            NN_front_top_score_RawImage.color = lerpedColor;
        }
        else
        {
            lerpedColor = Color.Lerp(Color.yellow, Color.green, score);
            NN_front_top_score_RawImage.color = lerpedColor;
        }
    }

    private void OnDisable()
    {
        inferenceNNfomDATABASE.OnScorescoreNNFrontTopChanged -= Handle_OnScorescoreNNFrontTopChanged;
    }

}
