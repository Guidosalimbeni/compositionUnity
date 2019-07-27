using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InferenceFinalOut : MonoBehaviour
{

    [SerializeField]
    TextAsset model;

    private ClassificationFinalOut classificationFinalOut;
    private float scoreFinalOut;
    private ScoreCalculator scoreCalculator;

    public event Action<float> OnScoreFinalOutChanged;

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
    public void CallTOCalculateFinalOutScore()
    {

        float i0 = scoreCalculator.scoreMobileNet;
        float i1 = scoreCalculator.scoreNNFrontTop;
        float i2 = scoreCalculator.scoreAllscorefeatures;
        

        float[] scores = new float[] { i0, i1, i2 };

        MakePredictionForFinalOut(scores);

    }

    private void MakePredictionForFinalOut(float[] scores)
    {

        scoreFinalOut = classificationFinalOut.PredictFinalOutScore(scores);

        if (OnScoreFinalOutChanged != null)
        {
            OnScoreFinalOutChanged(scoreFinalOut);
        }
    }

    public void InitTF()
    {
        classificationFinalOut = new ClassificationFinalOut(model,
                                                        input: "dense_1_input", output: "dense_3/Sigmoid");

    }

    public void CloseTF()
    {
        classificationFinalOut.Close();
    }
}
