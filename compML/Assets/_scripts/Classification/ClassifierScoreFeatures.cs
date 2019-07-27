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

    readonly string inputName;
    readonly string outputName;
    readonly int input_dim;

    public ClassifierScoreFeatures(TextAsset modelFile,
                      
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
        inputName = input;
        outputName = output;
        input_dim = input_dimension;
    }

    public void Close()
    {
        session?.Dispose();
        graph?.Dispose();
    }

    public float PredictScoreFeatures(float[] data)
    {
        var input = graph[inputName][0];
        var shape = new TFShape(1, input_dim);
        TFTensor inputTensor = null;
        var runner = session.GetRunner(); ///
        inputTensor = TFTensor.FromBuffer(shape, data, 0, data.Length);
        runner.AddInput(input, inputTensor);
        runner.Fetch(graph[outputName][0]);
        var output = runner.Run()[0];
        var outputs = output.GetValue() as float[,];
        //Debug.Log(outputs[0,0]);
        float scoreAllscorefeatures = outputs[0, 0];
        inputTensor.Dispose();
        output.Dispose();

        return scoreAllscorefeatures;
    }
}
