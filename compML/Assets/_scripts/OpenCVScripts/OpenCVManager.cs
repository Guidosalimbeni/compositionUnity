using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using System.IO;
using OpenCVForUnity.ImgprocModule;

public class OpenCVManager : MonoBehaviour
{

    public Mat ImageMatrixOpenCV;
    public RenderTexture camRenderTexture;
    public GameObject VisualisationSurface;
    public bool calcAreaBalanceOpenCV = true;

    private Texture2D textureContours;
    public bool calcThreshold = true;

    private void OnPostRender()
        {
            //if true, The error log of the Native side OpenCV will be displayed on the Unity Editor Console.
            Utils.setDebugMode(true);

            Texture2D imgTexture = ToTexture2D(camRenderTexture);
        
            ImageMatrixOpenCV = new Mat(imgTexture.height, imgTexture.width, CvType.CV_8UC1);

            Utils.texture2DToMat(imgTexture, ImageMatrixOpenCV);
            
            if (calcAreaBalanceOpenCV == true)
            {
                CalculateAreaBalanceUsingOpenCV(imgTexture);
            }
            if (calcThreshold == true)
            {
                CalculateThreshold(imgTexture);
            }

            Utils.setDebugMode(false);

        }

    private Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new UnityEngine.Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    public void SaveTexturePNG(RenderTexture rTex)
    {
        Texture2D tex = ToTexture2D(rTex);
        byte[] bytes = tex.EncodeToPNG(); // Encode texture into PNG
        Object.Destroy(tex);
        File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);
    }

    public void CalculateThreshold(Texture2D srcTexture)
    {
        Texture2D imgTexture = ToTexture2D(camRenderTexture); //

        Mat imgMat = new Mat(imgTexture.height, imgTexture.width, CvType.CV_8UC1);

        Utils.texture2DToMat(imgTexture, imgMat);
        Debug.Log("imgMat.ToString() " + imgMat.ToString());


        Imgproc.threshold(imgMat, imgMat, 1, 255, Imgproc.THRESH_BINARY);


        Texture2D texture = new Texture2D(imgMat.cols(), imgMat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(imgMat, texture);

        VisualisationSurface.GetComponent<Renderer>().material.mainTexture = texture;
    }

    public void CalculateAreaBalanceUsingOpenCV(Texture2D srcTexture)
    {
        Mat srcMat = new Mat(srcTexture.height, srcTexture.width, CvType.CV_8UC1);
        Utils.texture2DToMat(srcTexture, srcMat);
        Imgproc.threshold(srcMat, srcMat, 1, 255, Imgproc.THRESH_BINARY);


        List<MatOfPoint> srcContours = new List<MatOfPoint>();
        Mat srcHierarchy = new Mat();
        
        Imgproc.findContours(srcMat, srcContours, srcHierarchy, Imgproc.RETR_CCOMP, Imgproc.CHAIN_APPROX_NONE);

        //Debug.Log("srcContours.Count " + srcContours.Count);
        //dstMat
        Texture2D textureDestinationContours = new Texture2D(srcMat.cols(), srcMat.rows(), TextureFormat.RGBA32, false);
        Mat dstMat = new Mat(srcTexture.height, srcTexture.width, CvType.CV_8UC3);
        Utils.texture2DToMat(textureDestinationContours, dstMat);

        for (int i = 0; i < srcContours.Count; i++)
        {
            Imgproc.drawContours(dstMat, srcContours, i, new Scalar(150, 150, 150), 2, 8, srcHierarchy, 0, new Point());

        }

        
        textureContours = new Texture2D(srcMat.cols(), srcMat.rows(), TextureFormat.RGBA32, false);

        Utils.matToTexture2D(dstMat, textureDestinationContours);

        VisualisationSurface.GetComponent<Renderer>().material.mainTexture = textureDestinationContours;
    }

}
