using UnityEngine;
using System.Collections;
using System;

public class GetFromDatabase : MonoBehaviour
{
    
    ///Fill in your server data here.

    private string DataFromMysql = "http://www.guidosalimbeni.it/UnityComp/GetFromDatabase.php";

    public bool triggerGetData = false;


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
            

            //Split it into two smaller arrays
            string[] Names = new string[Mathf.FloorToInt(textlist.Length / 8)];
            string[] g0 = new string[Names.Length];
            string[] g1 = new string[Names.Length];
            string[] g2 = new string[Names.Length];
            string[] g3 = new string[Names.Length];
            string[] g4 = new string[Names.Length];
            string[] g5 = new string[Names.Length];
            string[] judge = new string[Names.Length];

            int j = 0;

            for (int i = 0; i < textlist.Length; i++)
            {
                Debug.Log(textlist[i]);

                switch (j)
                {
                    case 0:
                        Names[i] = textlist[i];
                        break;

                    case 1:
                        Names[i] = textlist[i];
                        break;
                    case 2:
                        Names[i] = textlist[i];
                        break;
                    case 3:
                        Names[i] = textlist[i];
                        break;
                    case 4:
                        Names[i] = textlist[i];
                        break;
                    case 5:
                        Names[i] = textlist[i];
                        break;
                    case 6:
                        Names[i] = textlist[i];
                        break;
                    case 7:
                        Names[i] = textlist[i];
                        break;
                    default:
                        j = 0;
                        break;

                }

              
            }

            for (int i = 0; i < judge.Length; i++)
            {
                Debug.Log(judge[i]);
            }
        }

        
    }

}
