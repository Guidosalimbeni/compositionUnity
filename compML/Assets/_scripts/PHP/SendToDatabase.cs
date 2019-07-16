using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class SendToDatabase : MonoBehaviour
{
    // for opencv score usually I created an events from Opencv Manager
    // for not opencv score usually to access scores via components and methods from UI manager -- which I am fixing.. since do not make sense

    public ImageMatrixData imagePixelsValues;
    //public int MaxMoves = 4;
    //public int sendaftereverymoves = 3;
    public GameObject gamemanager;

    private CollectDataRenderTexture collectdatarendertexture;
    private Game_Manager GM;
    private ScoreCalculator scoreCalculator;
    private PopulationManager populationmanager;
    private float g0;
    private float g1;
    private float g2;
    private float g3;
    private float g4;
    private float g5;
    private float judge;
    private List<GameObject> listOfelementsInComposition;
    private List<float> listOfgenes;
    //private int moves;
    private float scorePixelsBalance;
    private float scoreBoundsBalance;
    private float scoreUnityVisual;

    private void Awake()
    {
        collectdatarendertexture = FindObjectOfType<CollectDataRenderTexture>();

        scoreCalculator = FindObjectOfType<ScoreCalculator>();
        populationmanager = FindObjectOfType<PopulationManager>();
        GM = gamemanager.GetComponent<Game_Manager>();
    }

    private void Start()
    {
        if (GM != null)
        {
            listOfelementsInComposition = GM.getListOfItemInComposition(); // to fix since it is probably already in composition
        }
    }

    public void SendNegativeJudge()
    {
        PostDataFromAttemptsOfMoves();

    }

    public void PostDataFromAttemptsOfMoves()
    {
        listOfgenes = new List<float>();
        foreach (GameObject element in listOfelementsInComposition)
        {
            listOfgenes.Add(element.transform.position.x);
            listOfgenes.Add(element.transform.position.z);
        }
        scoreBoundsBalance = scoreCalculator.scoreBoundsBalance;
        scoreUnityVisual = scoreCalculator.scoreUnityVisual;
        scorePixelsBalance = scoreCalculator.visualScoreBalancePixelsCount;
        g0 = listOfgenes[0];
        g1 = listOfgenes[1];
        g2 = listOfgenes[2];
        g3 = listOfgenes[3];
        g4 = listOfgenes[4];
        g5 = listOfgenes[5];
        judge = 0;               //////////////////

        listOfgenes.Clear();

        StartCoroutine(PostData(LoadSceneInputUsername.username));
    }

    public void PostDataFromButton()
    {
        listOfgenes = new List<float>();
        foreach (GameObject element in listOfelementsInComposition)
        {
            listOfgenes.Add(element.transform.position.x);
            listOfgenes.Add(element.transform.position.z);
        }
        scoreBoundsBalance = scoreCalculator.scoreBoundsBalance;
        scoreUnityVisual = scoreCalculator.scoreUnityVisual;
        scorePixelsBalance = scoreCalculator.visualScoreBalancePixelsCount;
        g0 = listOfgenes[0];
        g1 = listOfgenes[1];
        g2 = listOfgenes[2];
        g3 = listOfgenes[3];
        g4 = listOfgenes[4];
        g5 = listOfgenes[5];
        judge = 1;               //////////////////

        listOfgenes.Clear();

        StartCoroutine(PostData(LoadSceneInputUsername.username)); //TO FIX
        populationmanager.triggerAI = true;
    }

    public void PostDataFromAI(float scorePixelsBalancefromAI, 
                                float scoreUnityVisualFromAI, 
                                float scoreBoundsBalancefromAI,
                                List<float> genesAI)
    {
        scorePixelsBalance = scorePixelsBalancefromAI; 
        scoreUnityVisual = scoreUnityVisualFromAI;
        scoreBoundsBalance = scoreBoundsBalancefromAI;

        g0 = genesAI[0];
        g1 = genesAI[1];
        g2 = genesAI[2];
        g3 = genesAI[3];
        g4 = genesAI[4];
        g5 = genesAI[5];
        judge = 1;                  ////////////////////// TODO

        StartCoroutine(PostData("AI_TURN"));
    }

    IEnumerator PostData(string username)
    {
        if (username == "" | username == null)
        {
            username = "Debugging";
        }

        collectdatarendertexture.CollectPixelsValuesFromImage(); // this update the scriptable object with the image string list pixels values

        WWWForm form = new WWWForm();
        form.AddField("name", username);
        form.AddField("filenameImg", "NotImplemented");
        form.AddField("scoreBoundsBalance", scoreBoundsBalance.ToString());
        form.AddField("scorePixelsBalance", scorePixelsBalance.ToString());
        form.AddField("scoreUnityVisual", scoreUnityVisual.ToString());
        form.AddField("g0", g0.ToString());
        form.AddField("g1", g1.ToString());
        form.AddField("g2", g2.ToString());
        form.AddField("g3", g3.ToString());
        form.AddField("g4", g4.ToString());
        form.AddField("g5", g5.ToString());
        form.AddField("ImagePixelsList", imagePixelsValues.ImagePixelsList.ToString());
        form.AddField("judge", judge.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("http://www.guidosalimbeni.it/UnityComp/AddToDatabase.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!" + www.downloadHandler.text);
            }
        }
    }
}
