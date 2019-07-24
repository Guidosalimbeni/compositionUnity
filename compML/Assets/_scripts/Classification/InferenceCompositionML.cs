using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InferenceCompositionML : MonoBehaviour
{
    // https://github.com/shaqian/TF-Unity
    // https://github.com/shaqian/TF-Unity-ARFoundation/blob/master/Assets/Scripts/CameraController.cs

    [SerializeField]
    TextAsset model;

    [SerializeField]
    TextAsset labels;

    public bool MobileNetScoring = true;

    public RenderTexture camRenderTexture; // this is 224 * 224 for mobile net

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

        if (MobileNetScoring == true)
        {
            MakePrecitionCompMLMobileNet();
        }
        

    }


    public void MakePrecitionCompMLMobileNet()
    {
        m_Texture = ToTexture2D(camRenderTexture);
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
        classifier = new ClassfierCompositionML(model, labels,input: "input_1_1",  output: "dense_2_1/Softmax");
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

    private Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new UnityEngine.Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

}



