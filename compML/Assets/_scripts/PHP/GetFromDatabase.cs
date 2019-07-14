using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GetFromDatabase : MonoBehaviour
{
    public bool triggerGetData = true;
    private string DataFromMysql = "http://www.guidosalimbeni.it/UnityComp/GetFromDatabase.php";

    [SerializeField]
    private DataForNeuralNetwork_CompML dataNN; // this is scribtable object to train the model..

    private void Update() //TODO turn this into a start function !!
    {
        if (triggerGetData == true)
        {
            StartCoroutine(GetDataFromDatabase());
            triggerGetData = false;
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
            List<Double> g0 = new List<Double>();
            List<Double> g1 = new List<Double>();
            List<Double> g2 = new List<Double>();
            List<Double> g3 = new List<Double>();
            List<Double> g4 = new List<Double>();
            List<Double> g5 = new List<Double>();
            List<Double> judge = new List<Double>();

            for (int i = 0; i < textlist.Length; i += 8)
            {
                namesList.Add(textlist[i]);
                g0.Add(Double.Parse(textlist[i + 1]));
                g1.Add(Double.Parse(textlist[i + 2]));
                g2.Add(Double.Parse(textlist[i + 3]));
                g3.Add(Double.Parse(textlist[i + 4]));
                g4.Add(Double.Parse(textlist[i + 5]));
                g5.Add(Double.Parse(textlist[i + 6]));
                judge.Add(Double.Parse(textlist[i + 7]));
            }

            // store the data into the scriptable variable
            dataNN.Names = namesList;
            dataNN.G0 = g0;
            dataNN.G1 = g1;
            dataNN.G2 = g2;
            dataNN.G3 = g3;
            dataNN.G4 = g4;
            dataNN.G5 = g5;
            dataNN.JUDGE = judge;


            foreach (int name in judge)
            {
                // Debug.Log(name);
            }
        }
    }
}
