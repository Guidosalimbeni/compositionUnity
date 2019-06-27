using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using System.IO;
using OpenCVForUnity.ImgprocModule;

public class CollectDataRenderTexture: MonoBehaviour
{
    // this was probably only to store on the database some information about the image that has been evaluated. currently only scaled 10 times ... does it make any sense? try to increase?

    public ImageMatrixData imagePixelsValues;
    public float resizefactor = 10.0f;
    private Mat ImageMatrixOpenCV;
    private RenderTexture camRenderTexture; // 240 * 180  // mobile net works with 224, 224
    private OpenCVManager opencvmanager;

    private void Awake()
    {
        opencvmanager = FindObjectOfType<OpenCVManager>();
        camRenderTexture = opencvmanager.camRenderTexture;
        imagePixelsValues.ImagePixelsList = "";
    }

    public void CollectPixelsValuesFromImage()
    {
        Texture2D imgTexture = ToTexture2D(camRenderTexture);
        ImageMatrixOpenCV = new Mat(imgTexture.height, imgTexture.width, CvType.CV_8UC1); // 180 * 240 * cv_8uc1
        
        Utils.texture2DToMat(imgTexture, ImageMatrixOpenCV);
        

        int rows = ImageMatrixOpenCV.rows();
        int cols = ImageMatrixOpenCV.cols();
        int ch = ImageMatrixOpenCV.channels();
        Size sz = new Size((int)(cols / resizefactor),(int)(rows/ resizefactor)); // left as 18 * 24 and i have another render texture just for the mobile net

        Imgproc.resize(ImageMatrixOpenCV, ImageMatrixOpenCV, sz);
        Imgproc.cvtColor(ImageMatrixOpenCV, ImageMatrixOpenCV, Imgproc.COLOR_RGB2GRAY);
        string dataFromMat = ImageMatrixOpenCV.reshape(1, 1).dump();
        imagePixelsValues.ImagePixelsList = dataFromMat;
    }

    private Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new UnityEngine.Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

}
