using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGradientBoundShapeBalance : MonoBehaviour
{
    public RawImage balanceBoundsShapesUI;
    Color lerpedColor = Color.white;
    private AreasObjects areaobjects;

    private void Awake()
    {
        balanceBoundsShapesUI = balanceBoundsShapesUI.GetComponent<RawImage>();
    }

    private void Update()
    {
        areaobjects = Game_Manager.Instance.AreasOfObjects;
        float leftWeight = areaobjects.ObjectsLeftAreaPercentage;
        float RightWeight = areaobjects.ObjectsRightAreaPercentage;

        float DifferenceBetweenLeftandRight = Mathf.Abs(leftWeight - RightWeight);
        float visualScoreBalanceBoundsShapes = 1 - ((DifferenceBetweenLeftandRight) / (leftWeight + RightWeight));

        UpdateBalancePixelsUI(visualScoreBalanceBoundsShapes);

    }


    public void UpdateBalancePixelsUI(float score)
    {
        //balancePixelsUI.color = new Color(147f, 244f, 196f, 100f);
        //lerpedColor = Color.Lerp(Color.red, Color.green, Mathf.PingPong(Time.time, 1));

        if (score < 0.8f)
        {
            lerpedColor = Color.Lerp(Color.red * 0.8f, Color.yellow, score);
            balanceBoundsShapesUI.color = lerpedColor;
        }
        else
        {
            lerpedColor = Color.Lerp(Color.yellow, Color.green, score);
            balanceBoundsShapesUI.color = lerpedColor;
        }


    }
}
