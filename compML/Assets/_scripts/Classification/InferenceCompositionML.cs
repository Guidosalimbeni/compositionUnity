using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;


public class InferenceCompositionML : MonoBehaviour
{
    // https://github.com/shaqian/TF-Unity
    // https://github.com/shaqian/TF-Unity-ARFoundation/blob/master/Assets/Scripts/CameraController.cs

    [SerializeField]
    TextAsset model;

    [SerializeField]
    TextAsset labels;

    public bool CustomCNNEnable = true;

    public RenderTexture camRenderTexture; // this is 64 x 64 changed using custom... but need this to come as 240 x 180 and then convert to 64 64

    public int W = 64;
    public int H = 64;

    ClassfierCompositionML classifier;
    DetectorCompositionML detector;

    private IList outputs;
    private Texture2D m_Texture;

    public event Action<float> OnScorescoreMobileNetChanged;


    void OnEnable()
    {
        InitTF();
    }

    void OnDisable()
    {
        CloseTF();
    }


    // call from leantouch and population manager one during breeding and one for last move
    // also called from the AGENTCompAi to update the reward on decision on demand..
    public void CallTOCalculateMobileNetScore()
    {

        if (CustomCNNEnable == true)
        {
            MakePrecitionCUSTOMCNNNet();
        }
        

    }


    public void MakePrecitionCUSTOMCNNNet()
    {
        m_Texture = ToTexture2DAndResize(camRenderTexture, W, H);
        RunTF(m_Texture);
    }

    public void RunTF(Texture2D texture)
    {
        // MobileNet
        float ScoreFromMobileNet = classifier.Classify(texture, angle: 90, threshold: 0.05f);


        if (OnScorescoreMobileNetChanged != null)
        {
            OnScorescoreMobileNetChanged(ScoreFromMobileNet);
        }

        // SSD MobileNet
        //outputs = detector.Detect(m_Texture, angle: 90, threshold: 0.6f);

        // Tiny YOLOv2
        //outputs = detector.Detect(m_Texture, angle: 90, threshold: 0.1f);

        //ShowOutputs(outputs);

    }

    //private void ShowOutputs(IList outputs)
    //{
    //    foreach (var o in outputs)
    //    {
    //        Debug.Log("from inference " + o);
    //    }
    //}

    public void InitTF()
    {
        // MobileNet
        //classifier = new ClassfierCompositionML(model, labels, output: "MobilenetV1/Predictions/Reshape_1");
        classifier = new ClassfierCompositionML(model, labels,input: "conv2d_1_input_1",  output: "dense_2_1/Softmax",
                                                height: H,
                                                width: W,
                                                mean: 127.5f,
                                                std: 127.5f);
        // SSD MobileNet
        //detector = new DetectorCompositionML(model, labels,input: "image_tensor");

        // Tiny YOLOv2
        //detector = new DetectorCompositionML(model, labels, DetectionModels.YOLO,
        //width: 416,
        //height: 416,
        //mean: 0,
        //std: 255);
    }

    public void CloseTF()
    {
        classifier.Close();


        if (detector != null)
            detector.Close();
    }

    private Texture2D ToTexture2DAndResize(RenderTexture rTex, int W, int H)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new UnityEngine.Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();

        Mat imgMat = new Mat(tex.height, tex.width, CvType.CV_8UC1);
        Utils.texture2DToMat(tex, imgMat);

        Size scaleSize = new Size(W, H);
        Imgproc.resize(imgMat, imgMat, scaleSize,0,0,interpolation: Imgproc.INTER_AREA );
        Texture2D resizedTexture = new Texture2D(W, H, TextureFormat.RGBA32, false);

        Utils.matToTexture2D(imgMat, resizedTexture);


        return resizedTexture;

    }

}



