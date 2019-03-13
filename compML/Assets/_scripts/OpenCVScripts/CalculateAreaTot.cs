using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ImgprocModule;

public class CalculateAreaTot : MonoBehaviour
{
    [SerializeField]
    private GameObject AreaPlaneObj;

    public void CalculateAreaBalanceUsingOpenCV(Texture2D srcTexture)
    {
        //srcMat
        Mat srcMat = new Mat(srcTexture.height, srcTexture.width, CvType.CV_8UC1);
        Utils.texture2DToMat(srcTexture, srcMat);
        //Debug.Log("srcMat.ToString() " + srcMat.ToString());
        Imgproc.threshold(srcMat, srcMat, 01, 255, Imgproc.THRESH_BINARY);
        //dstMat
        Mat dstMat = new Mat(srcTexture.height, srcTexture.width, CvType.CV_8UC3);
        //Debug.Log("dstMat.ToString() " + dstMat.ToString());
        List<MatOfPoint> srcContours = new List<MatOfPoint>();
        Mat srcHierarchy = new Mat();
        /// Find srcContours
        Imgproc.findContours(srcMat, srcContours, srcHierarchy, Imgproc.RETR_CCOMP, Imgproc.CHAIN_APPROX_NONE);

        Imgproc.drawContours(dstMat, srcContours, 0, new Scalar(255, 0, 0), 2);
        /*
        for (int i = 0; i < srcContours.Count; i++)
        {
            Imgproc.drawContours(dstMat, srcContours, i, new Scalar(255, 0, 0), 2);
        }
        */
        Texture2D texture = new Texture2D(dstMat.cols(), dstMat.rows(), TextureFormat.RGBA32, false);

        Utils.matToTexture2D(dstMat, texture);

        AreaPlaneObj.GetComponent<Renderer>().material.mainTexture = texture;
        
    }


}
