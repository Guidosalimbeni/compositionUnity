using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;

public class ClassifierScoreFeatures : MonoBehaviour
{
    TFGraph graph;
    TFSession session;
    //string[] labels;

    readonly string inputName;
    readonly string outputName;
    readonly int input_dim;
    

    public ClassifierScoreFeatures(TextAsset modelFile,
                      //TextAsset labelFile,
                      string input = "input",
                      string output = "output",
                      int input_dimension = 4)
    {
#if UNITY_ANDROID
        TensorFlowSharp.Android.NativeBinding.Init ();
#endif
        graph = new TFGraph();
        graph.Import(modelFile.bytes);
        session = new TFSession(graph);

        //labels = labelFile.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        inputName = input;
        outputName = output;

        input_dim = input_dimension;
       
    }

    public void Close()
    {
        session?.Dispose();
        graph?.Dispose();
        //labels = null;
    }

    public float PredictScoreFeatures(float[] data)
    {
        //var shape = new TFShape(1, input_dim);
        var input = graph[inputName][0];

        //TFTensor (double[] data);

        var shape = new TFShape(1, input_dim);

        TFTensor inputTensor = null;
        //new TFTensor(data); ; //////////////////////

        var runner = session.GetRunner(); ///


        inputTensor = TFTensor.FromBuffer(shape, data, 0, data.Length);

        runner.AddInput(input, inputTensor);
        runner.Fetch(graph[outputName][0]);

        var output = runner.Run()[0];

        // Fetch the results from output:
        //TFTensor result = output[0];

        Debug.Log(output.Shape);

        var outputs = output.GetValue() as float[,];

        Debug.Log(outputs[0,0]);

        inputTensor.Dispose();
        output.Dispose();

        //outputScore

        //byte[] imgData = new byte[height * width * 3];

        return 0.0f;
    }



}
