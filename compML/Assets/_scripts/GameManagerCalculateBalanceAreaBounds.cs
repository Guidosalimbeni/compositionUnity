using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct AreasObjects
{
    public float ObjectsLeftAreaPercentage;
    public float ObjectsRightAreaPercentage;
}

public class GameManagerCalculateBalanceAreaBounds : MonoBehaviour
{
    private CompScreenAreaCalculation[] compositionObjects;
    private AreasObjects areasObjects;
    float totalLeftArea;
    float totalRightArea;

    private void Awake()
    {
        compositionObjects = FindObjectsOfType<CompScreenAreaCalculation>();
        areasObjects = new AreasObjects();
    }

    public AreasObjects CalculateBondsAreas()
    {
        foreach (var compObj in compositionObjects)
        {
            TypeOfScreenData typeofscreendata = compObj.CalcScreenPercentage();
            totalLeftArea += typeofscreendata.ScreenAreaLeftObject;
            totalRightArea += typeofscreendata.ScreenAreaRightObject;
        }
        areasObjects.ObjectsLeftAreaPercentage = totalLeftArea / (Screen.width * Screen.height);
        areasObjects.ObjectsRightAreaPercentage = totalRightArea / (Screen.width * Screen.height);
        totalLeftArea = 0;
        totalRightArea = 0;
        return areasObjects;
    }
}
