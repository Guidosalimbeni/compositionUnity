using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MLAgents;
//using Barracuda;

public class MobileNetModel : MonoBehaviour
{
    //[SerializeField]
    //private CompositionImage CompImage224;

    
    //private CollectCompositionImage collectCompositionImage;

    //private IWorker worker;

    //// https://github.com/Unity-Technologies/ml-agents/blob/master/UnitySDK/Assets/ML-Agents/Plugins/Barracuda.Core/Barracuda.md


    //public NNModel modelSource;

    //private void Start()
    //{
    //    //bool verbose = false;
    //    //string modelName = "ModelMorandi_all";
    //    //Model model = ModelLoader.LoadFromStreamingAssets(modelName + ".bytes", verbose);  // not used
        
    //    var model = ModelLoader.Load(modelSource);
    //    worker = BarracudaWorkerFactory.CreateWorker(BarracudaWorkerFactory.Type.ComputePrecompiled, model);

    //    collectCompositionImage = FindObjectOfType<CollectCompositionImage>();
    //}


    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        UseTexture();
    //    }
    //}

    //private void UseTexture()
    //{

    //    collectCompositionImage.AssignCompRenderToTexture();
    //    // you can treat input pixels as 1 (grayscale), 3 (color) or 4 (color with alpha) channels
    //    Texture2D renderView224MobileNet = CompImage224.CompImage;

    //    Debug.Log(renderView224MobileNet.format);
    //    Debug.Log(renderView224MobileNet.width);
        
    //    Tensor tensor = new Tensor(renderView224MobileNet);
    //    worker.Execute(tensor);
    //    Tensor outputMobileNet = worker.Fetch();


    //    Debug.Log("this is the ouput of the MobileNEt    " + outputMobileNet);

    //    outputMobileNet.Dispose();
    //    worker.Dispose();


    //}

}
