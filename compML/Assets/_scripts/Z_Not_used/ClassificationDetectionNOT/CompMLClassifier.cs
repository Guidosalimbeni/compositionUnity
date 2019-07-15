using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TFClassify;
using TensorFlow;
using System.Text.RegularExpressions;
using System.Linq;
using System;

public class CompMLClassifier : MonoBehaviour
{

    public TextAsset modelFile;
    public TextAsset labelsFile;

    private Classifier classifier;
    private const int classifyImageSize = 224;


    private void Start()
    {
        LoadWorker();
    }


    private void LoadWorker()
    {
        try
        {
            LoadClassifier();
            
        }
        catch (TFException ex)
        {
            if (ex.Message.EndsWith("is up to date with your GraphDef-generating binary.)."))
            {
                Debug.Log("Error: TFException. Make sure you model trained with same version of TensorFlow as in Unity plugin.");
            }

            throw;
        }
    }

    private void LoadClassifier()
    {
        this.classifier = new Classifier(
            this.modelFile.bytes,
            Regex.Split(this.labelsFile.text, "\n|\r|\r\n")
                .Where(s => !String.IsNullOrEmpty(s)).ToArray(),
            classifyImageSize);
    }

}
