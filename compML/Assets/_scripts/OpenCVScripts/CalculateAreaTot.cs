using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;

public class CalculateAreaTot : MonoBehaviour
{

    private Mat imageMat;

    private void LateUpdate()
    {
        imageMat = OpenCVManager.instanceCV2.imgMat;
        //Debug.Log("from calculate area totoal imgMat.ToString() " + imageMat.ToString());
        
        
        // apply threshold
        // find contours
        // might worth to create a masked render and called imgmat masked

    }


}
