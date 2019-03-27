using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGradientPixelsBalance : MonoBehaviour
{
    public RawImage balancePixelsUI;
    Color lerpedColor = Color.white;
    private OpenCVManager openCvManager;

    private void Awake()
    {
        balancePixelsUI = balancePixelsUI.GetComponent<RawImage>();
        openCvManager = FindObjectOfType<OpenCVManager>();
    }

    private void Start()
    {
        openCvManager.OnPixelsCountBalanceChanged += HandlePixelsCountBalanceChanged;
    }

    private void HandlePixelsCountBalanceChanged(float score)
    {
        UpdateBalancePixelsUI(score);
    }

    public void UpdateBalancePixelsUI(float score)
    {
        //balancePixelsUI.color = new Color(147f, 244f, 196f, 100f);
        //lerpedColor = Color.Lerp(Color.red, Color.green, Mathf.PingPong(Time.time, 1));

        if (score < 0.8f)
        {
            lerpedColor = Color.Lerp(Color.red * 0.8f, Color.yellow, score);
            balancePixelsUI.color = lerpedColor;
        }
        else
        {
            lerpedColor = Color.Lerp(Color.yellow, Color.green, score);
            balancePixelsUI.color = lerpedColor;
        }

        
    }
    
}
