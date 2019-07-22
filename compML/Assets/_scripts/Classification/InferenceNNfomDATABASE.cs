using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using System.IO;
using OpenCVForUnity.ImgprocModule;

public class InferenceNNfomDATABASE : MonoBehaviour
{
    // https://github.com/shaqian/TF-Unity
    // https://github.com/shaqian/TF-Unity-ARFoundation/blob/master/Assets/Scripts/CameraController.cs

    [SerializeField]
    TextAsset model;

    [SerializeField]
    TextAsset labels;

    public RenderTexture camRenderFront; // this is 20 x 20 x 3 need to strip to 400 np.array 
    public RenderTexture camRenderTop; // this is 20 x 20 x 3 need to strip to 400 np.array  


    ClassifierNNDATABASE classifierNNDatabase;
    DetectorCompositionML detector;

    private IList outputs;
    private Texture2D m_TextureFront;
    private Texture2D m_TextureTop;

    void OnEnable()
    {
        InitTF();
    }

    void OnDisable()
    {
        CloseTF();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

            MakePrecitionNNDatabase();
        }
    }


    public void MakePrecitionNNDatabase()
    {
        m_TextureFront = ToTexture2D(camRenderFront);
        m_TextureTop = ToTexture2D(camRenderTop);



        Texture2D TextureConcatanate = Concat2Texture2D(m_TextureFront, m_TextureTop);

        //debug = TextureConcatanate; // to delete

        RunTFNN(TextureConcatanate);
    }

    public void RunTFNN(Texture2D TextureConcatanate)
    {
        // NN from database collected and trained offline
        outputs = classifierNNDatabase.ClassifyNN(TextureConcatanate, angle: 90, threshold: 0.05f);

        ShowOutputs(outputs);

    }

    private void ShowOutputs(IList outputs)
    {
        foreach (var o in outputs)
        {
            Debug.Log("from inference " + o);
        }
    }

    public void InitTF()
    {
        
           classifierNNDatabase = new ClassifierNNDATABASE(model, labels, input: "conv2d_1_input", output: "dense_3/Softmax");
    }

    public void CloseTF()
    {
        classifierNNDatabase.Close();


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

    private Texture2D Concat2Texture2D(Texture2D front, Texture2D top)
    {
        // https://stackoverflow.com/questions/42539257/unity2d-combine-sprites
        // https://stackoverflow.com/questions/20078875/merging-cvmat-horizontally

        Texture2D TextureConcatanate = new Texture2D(front.width * 2, front.height, TextureFormat.RGBA32, false);
        Mat imgMatTextureConcatanate = new Mat(front.height, front.width * 2,  CvType.CV_8UC1);
        Utils.texture2DToMat(TextureConcatanate, imgMatTextureConcatanate);
        Mat imgMatFront = new Mat(front.width, front.height, CvType.CV_8UC1);
        Utils.texture2DToMat(front, imgMatFront);
        Mat imgMatTop = new Mat(top.width, top.height, CvType.CV_8UC1);
        Utils.texture2DToMat(top, imgMatTop);

        List<Mat> ListToConcat = new List<Mat>();
        ListToConcat.Add(imgMatFront);
        ListToConcat.Add(imgMatTop);

        Core.hconcat(ListToConcat, imgMatTextureConcatanate);

        Utils.matToTexture2D(imgMatTextureConcatanate, TextureConcatanate);
        ListToConcat.Clear();

        return TextureConcatanate;

    }

    //Set these Textures in the Inspector
    //private Texture2D debug;

    //public GameObject plane;
    //Renderer m_Renderer;

    // Use this for initialization
    //private void Update()
    //{

    //    Debug.Log("I am hre");
    //    MakePrecitionNNDatabase();

    //    //Fetch the Renderer from the GameObject
    //    m_Renderer = plane.GetComponent<Renderer>();

    //    //Set the Texture you assign in the Inspector as the main texture (Or Albedo)
    //    m_Renderer.material.SetTexture("_MainTex", debug);
        
    //}
}



