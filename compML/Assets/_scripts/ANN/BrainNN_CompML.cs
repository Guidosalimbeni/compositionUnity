using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainNN_CompML : MonoBehaviour
{

    [SerializeField]
    private DataForNeuralNetwork_CompML dataNN;
    [SerializeField]
    private DataScores nnScore;

    ANN ann;
    double sumSquareError = 0;

    void Start()
    {
        ann = new ANN(6, 1, 4, 4, 0.8);

        List<double> result;

        for (int i = 0; i < 1000; i++)
        {
            for (int j = 0; j < dataNN.Names.Count; j++)
            {
                double i0 = dataNN.G0[j];
                double i1 = dataNN.G1[j];
                double i2 = dataNN.G2[j];
                double i3 = dataNN.G3[j];
                double i4 = dataNN.G4[j];
                double i5 = dataNN.G5[j];
                double output = dataNN.JUDGE[j];

                sumSquareError = 0;
                result = Train(i0, i1, i2, i3, i4, i5, output);
                sumSquareError += Mathf.Pow((float)result[0] - 0, 2);
            }

            //Debug.Log("SSE: " + sumSquareError);
        }
        
    }

    List<double> Train(double i0, double i1, double i2, double i3, double i4, double i5, double output)
    {
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();
        inputs.Add(i0);
        inputs.Add(i1);
        inputs.Add(i2);
        inputs.Add(i3);
        inputs.Add(i4);
        inputs.Add(i5);
        outputs.Add(output);
        return (ann.Train(inputs, outputs));
    }


    public float PredictFromNN(double i0, double i1, double i2, double i3, double i4, double i5, double output = 0)
    {
        List<double> result;
        float ScoreNN = 0;

        result = Prediction(i0,i1,i2,i3,i4,i5);
        nnScore.NNSCORE = result[0]; /// not used
        ScoreNN = (float)(result[0]);

        return ScoreNN;
    }


    List<double> Prediction(double i0, double i1, double i2, double i3, double i4, double i5, double output = 0)
    {
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();
        inputs.Add(i0);
        inputs.Add(i1);
        inputs.Add(i2);
        inputs.Add(i3);
        inputs.Add(i4);
        inputs.Add(i5);
        outputs.Add(output);

        return (ann.CalcOutput(inputs, outputs));

    }

    // create then a script for prdiction...

    //brainNN_compML.PredictFromNN(genes[0], genes[1], genes[2], genes[3], genes[4], genes[5]);  // might want to MULTIPLY
}

