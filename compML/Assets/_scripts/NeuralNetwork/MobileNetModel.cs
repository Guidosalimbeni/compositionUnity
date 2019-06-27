using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using Barracuda;

public class MobileNetModel : MonoBehaviour
{
    [SerializeField]
    private CompositionImage CompImage224;

    
    private CollectCompositionImage collectCompositionImage;

    private IWorker worker;

    // https://github.com/Unity-Technologies/ml-agents/blob/e3b86a27a2547cdd177bf9e848b4337aa71b887a/UnitySDK/Assets/ML-Agents/Plugins/Barracuda.Core/Barracuda.md


    private void Start()
    {
        bool verbose = false;
        string modelName = "ModelMorandi_all";
        Model model = ModelLoader.LoadFromStreamingAssets(modelName + ".bytes", verbose);
        worker = BarracudaWorkerFactory.CreateWorker(BarracudaWorkerFactory.Type.Compute, model);

        collectCompositionImage = FindObjectOfType<CollectCompositionImage>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UseTexture();
        }
    }

    private void UseTexture()
    {
        collectCompositionImage.AssignCompRenderToTexture();
        // you can treat input pixels as 1 (grayscale), 3 (color) or 4 (color with alpha) channels
        Texture2D renderView224MobileNet = CompImage224.CompImage;

        int channels = 1;
        Tensor tensor = new Tensor(renderView224MobileNet, channels);
        worker.Execute(tensor);
        Tensor outputMobileNet = worker.Fetch();


        Debug.Log("this is the ouput of the MobileNEt    " + outputMobileNet);

        outputMobileNet.Dispose();
        worker.Dispose();


    }

}
