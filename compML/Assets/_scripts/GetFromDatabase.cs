using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GetFromDatabase : MonoBehaviour
{
    public bool triggerGetData = false;
    private string DataFromMysql = "http://www.guidosalimbeni.it/UnityComp/GetFromDatabase.php";

    [SerializeField]
    private DataForNeuralNetwork dataNN; // 1


    private void Update()
    {
        
        if (triggerGetData == true)
        {
            StartCoroutine(GetTopScores());
            triggerGetData = false;
        }

    }


    IEnumerator GetTopScores()
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

            List<string> namesList = new List<string>();
            List<float> g0 = new List<float>();
            List<float> g1 = new List<float>();
            List<float> g2 = new List<float>();
            List<float> g3 = new List<float>();
            List<float> g4 = new List<float>();
            List<float> g5 = new List<float>();
            List<int> judge = new List<int>();

            for (int i = 0; i < textlist.Length; i += 8)
            {
                namesList.Add(textlist[i]);
                g0.Add(float.Parse(textlist[i + 1]));
                g1.Add(float.Parse(textlist[i + 2]));
                g2.Add(float.Parse(textlist[i + 3]));
                g3.Add(float.Parse(textlist[i + 4]));
                g4.Add(float.Parse(textlist[i + 5]));
                g5.Add(float.Parse(textlist[i + 6]));
                judge.Add(int.Parse(textlist[i + 7]));
            }

            dataNN.Names = namesList;

            foreach (int name in judge)
            {
                //Debug.Log(name);
            }
        }
    }
}
