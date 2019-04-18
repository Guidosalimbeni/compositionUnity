using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Image Matrix", menuName = "CNN Data", order = 51)]

public class ImageMatrixData : ScriptableObject
{
    [SerializeField]
    private string imagePixelsList;

    public string ImagePixelsList { get { return imagePixelsList; } set { imagePixelsList = value; } }
}

