using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GetFromDatabase : MonoBehaviour
{
    public bool triggerGetData = true;
    private string DataFromMysql = "http://www.guidosalimbeni.it/UnityComp/GetFromDatabase.php";

    private BrainNN_CompML brainNN_compML;

    [SerializeField]
    private DataForNeuralNetwork_CompML dataNN; // this is scribtable object to train the model.. for the only weigths

    private void Start() 
    {
        if (triggerGetData == true)
        {
            brainNN_compML = FindObjectOfType<BrainNN_CompML>();
            StartCoroutine(GetDataFromDatabase());
            //triggerGetData = false;
        }
    }

    IEnumerator GetDataFromDatabase()
    {
        WWW GetDataFromSQL = new WWW(DataFromMysql);
        yield return GetDataFromSQL;

        if (GetDataFromSQL.error != null)
        {
            Debug.Log("failed to get data from the server");
        }
        else
        {
            //Collect up all our data
            string[] textlist = GetDataFromSQL.text.Split(new string[] { "\n", "\t" }, System.StringSplitOptions.RemoveEmptyEntries);

            // this clear if pressed trigger again..not add up same data from sql
            List<string> namesList = new List<string>();
            List<Double> ScorePixelsBalance = new List<Double>();
            List<Double> ScoreUnityVisual = new List<Double>();
            List<Double> ScoreLawOfLever = new List<Double>();
            List<Double> ScoreNNFrontTop = new List<Double>();
            List<Double> ScoreMobileNet = new List<Double>();
            
            List<Double> judge = new List<Double>();

            int totalNumberOfDataCollected = 7;

            for (int i = 0; i < textlist.Length; i += totalNumberOfDataCollected)
            {
                namesList.Add(textlist[i]);
                ScorePixelsBalance.Add(Double.Parse(textlist[i + 1]));
                ScoreUnityVisual.Add(Double.Parse(textlist[i + 2]));
                ScoreLawOfLever.Add(Double.Parse(textlist[i + 3]));
                ScoreNNFrontTop.Add(Double.Parse(textlist[i + 4]));
                ScoreMobileNet.Add(Double.Parse(textlist[i + 5]));
                judge.Add(Double.Parse(textlist[i + 6]));
            }

            // store the data into the scriptable variable
            dataNN.Names = namesList;
            dataNN.ScorePixelsBalance = ScorePixelsBalance;
            dataNN.ScoreUnityVisual = ScoreUnityVisual;
            dataNN.ScoreLawOfLever = ScoreLawOfLever;
            dataNN.ScoreNNFrontTop = ScoreNNFrontTop;
            dataNN.ScoreMobileNet = ScoreMobileNet;

            dataNN.JUDGE = judge;

            //foreach (var name in namesList)
            //{
            //    Debug.Log(name);
            //}


            brainNN_compML.TrainNeuralNetworkForWeightScoreUpdateFromDatabase();

        }
    }
}
