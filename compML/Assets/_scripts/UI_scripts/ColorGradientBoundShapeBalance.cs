using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorGradientBoundShapeBalance : MonoBehaviour
{
    public RawImage balanceBoundsShapesUI;

    private float visualScoreBalanceBoundsShapes;
    Color lerpedColor = Color.white;
    private AreasObjects areaobjects;

    private Game_Manager gamemanager;

    private void Awake()
    {
        gamemanager = FindObjectOfType<Game_Manager>();
        balanceBoundsShapesUI = balanceBoundsShapesUI.GetComponent<RawImage>();
    }

    private void Update()
    {
        areaobjects = gamemanager.AreasOfObjects;
        float leftWeight = areaobjects.ObjectsLeftAreaPercentage;
        float RightWeight = areaobjects.ObjectsRightAreaPercentage;

        float DifferenceBetweenLeftandRight = Mathf.Abs(leftWeight - RightWeight);
        visualScoreBalanceBoundsShapes = 1 - ((DifferenceBetweenLeftandRight) / (leftWeight + RightWeight));

        UpdateBalancePixelsUI(visualScoreBalanceBoundsShapes);

    }

    public float GetvisualScoreBalanceBoundsShapes()
    {
        return visualScoreBalanceBoundsShapes;
    }

    public void UpdateBalancePixelsUI(float score)
    {
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
