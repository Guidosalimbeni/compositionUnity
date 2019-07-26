using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using System.IO;
using OpenCVForUnity.ImgprocModule;

public class InferenceScoreFeatures : MonoBehaviour
{
    // https://github.com/shaqian/TF-Unity
    // https://github.com/shaqian/TF-Unity-ARFoundation/blob/master/Assets/Scripts/CameraController.cs

    [SerializeField]
    TextAsset model;

 

    ClassifierScoreFeatures classifierScoreFeatures;

    private float scoreAllscorefeatures;
    private ScoreCalculator scoreCalculator;

    public event Action<float> OnScoreAllscoresfeatures;

    void OnEnable()
    {
        InitTF();
    }

    private void Awake()
    {
        scoreCalculator = FindObjectOfType<ScoreCalculator>();
    }

    void OnDisable()
    {
        CloseTF();
    }

    // call from leantouch and population manager one during breeding and one for last move
    // also called from the AGENTCompAi to update the reward on decision on demand..
    public void CallTOCalculateFeaturesAllScores()
    {
        // 'scorePixelsBalance', 'scoreUnityVisual', 'scoreLawOfLever', 'scoreIsolationBalance'
        
        float i0 = scoreCalculator.visualScoreBalancePixelsCount;
        float i1 = scoreCalculator.scoreUnityVisual;
        float i2 = scoreCalculator.scoreLawOfLever;
        float i3 = scoreCalculator.scoreIsolationBalance;

        float[] scores = new float[] { i0,i1,i2,i3 };

        MakePredictionScoreFeatures(scores);


    }

    private void MakePredictionScoreFeatures(float[] scores)
    {
         //////////

        scoreAllscorefeatures = classifierScoreFeatures.PredictScoreFeatures(scores);

        if (OnScoreAllscoresfeatures != null)
        {
            OnScoreAllscoresfeatures(scoreAllscorefeatures); // need to implement all the events registrations..
        }
    }

    public void InitTF()
    {

        classifierScoreFeatures = new ClassifierScoreFeatures(model,
                                                        input: "dense_1_input", output: "dense_3/Sigmoid");
                                                        
    }

    public void CloseTF()
    {
        classifierScoreFeatures.Close();


    }
}
