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

    public ImageMatrixData imageMatrixData;
    public float resizefactor = 10.0f;
    private RenderTexture camRenderTexture; // 240 * 180  // mobile net works with 224, 224
    private OpenCVManager opencvmanager;
    [SerializeField]
    private RenderTexture ImageNNFrontViewRT; // 20 x 20
    [SerializeField]
    private RenderTexture ImageNNtopViewRT; // 20 x 20

    private void Awake()
    {
        opencvmanager = FindObjectOfType<OpenCVManager>();
        camRenderTexture = opencvmanager.camRenderTexture;
        imageMatrixData.ImagePixelsListMainPaintView = "";
        imageMatrixData.ImageNNFrontView = "";
        imageMatrixData.ImageNNtopView = "";
    }

    public void CollectPixelsValuesFromImageForMainViewRecordInDatabase()
    {
        Texture2D imgTexture = ToTexture2D(camRenderTexture);
        Mat ImageMatrixOpenCV = new Mat(imgTexture.height, imgTexture.width, CvType.CV_8UC1); // 180 * 240 * cv_8uc1
        
        Utils.texture2DToMat(imgTexture, ImageMatrixOpenCV);

        int rows = ImageMatrixOpenCV.rows();
        int cols = ImageMatrixOpenCV.cols();
        int ch = ImageMatrixOpenCV.channels();
        Size sz = new Size((int)(cols / resizefactor),(int)(rows/ resizefactor)); // left as 18 * 24 and i have another render texture just for the mobile net

        Imgproc.resize(ImageMatrixOpenCV, ImageMatrixOpenCV, sz);
        Imgproc.cvtColor(ImageMatrixOpenCV, ImageMatrixOpenCV, Imgproc.COLOR_RGB2GRAY);
        string dataFromMat = ImageMatrixOpenCV.reshape(1, 1).dump();
        imageMatrixData.ImagePixelsListMainPaintView = dataFromMat;
    }

    public void CollectPixelsValuesFromImageForNeuralNetworkDNNOfflineTraining()
    {
        Texture2D ImageNNFrontViewTexture = ToTexture2D(ImageNNFrontViewRT);
        Mat ImageMatrixNNfrontView = new Mat(ImageNNFrontViewTexture.height, ImageNNFrontViewTexture.width, CvType.CV_8UC1); // 180 * 240 * cv_8uc1
        Utils.texture2DToMat(ImageNNFrontViewTexture, ImageMatrixNNfrontView);
        
        Imgproc.cvtColor(ImageMatrixNNfrontView, ImageMatrixNNfrontView, Imgproc.COLOR_RGB2GRAY);
        string dataFromMatFrontView = ImageMatrixNNfrontView.reshape(1, 1).dump();
        imageMatrixData.ImageNNFrontView = dataFromMatFrontView;

        Texture2D ImageNNtopViewTexture = ToTexture2D(ImageNNtopViewRT);
        Mat ImageMatrixNNtopView = new Mat(ImageNNtopViewTexture.height, ImageNNtopViewTexture.width, CvType.CV_8UC1); // 180 * 240 * cv_8uc1
        Utils.texture2DToMat(ImageNNtopViewTexture, ImageMatrixNNtopView);

        Imgproc.cvtColor(ImageMatrixNNtopView, ImageMatrixNNtopView, Imgproc.COLOR_RGB2GRAY);
        string dataFromMatTopView = ImageMatrixNNtopView.reshape(1, 1).dump();
        imageMatrixData.ImageNNtopView = dataFromMatTopView;

    }

    private Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new UnityEngine.Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    public byte[] CollectPNG()
    {
        Texture2D tex = ToTexture2D(camRenderTexture);

        // Read screen contents into the texture
        tex.ReadPixels(new UnityEngine.Rect(0, 0, camRenderTexture.width, camRenderTexture.height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);

        return bytes;
    }

}
