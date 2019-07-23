using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using System;

public class SendToDatabase : MonoBehaviour
{
    // for opencv score usually I created an events from Opencv Manager
    // for not opencv score usually to access scores via components and methods from UI manager -- which I am fixing.. since do not make sense

    public ImageMatrixData imagePixelsValues;

    private CollectDataRenderTexture collectdatarendertexture;
    private GamePopulationController gamePopulationController;
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
    private List<float> ListOfInfoForDatabase;
    private float scorePixelsBalance;
    private float scoreBoundsBalance;
    private float scoreUnityVisual;
    private string genesString;
    private int numberOfItems;
    private string sequence;
    private float scoreLawOfLever;

    private void Awake()
    {
        collectdatarendertexture = FindObjectOfType<CollectDataRenderTexture>();
        scoreCalculator = FindObjectOfType<ScoreCalculator>();
        populationmanager = FindObjectOfType<PopulationManager>();
        gamePopulationController = FindObjectOfType<GamePopulationController>();
    }

    private void Start()
    {
        ListOfInfoForDatabase = new List<float>();

        if (gamePopulationController != null)
        {
            listOfelementsInComposition = gamePopulationController.ElementsCompositions; // to fix since it is probably already in composition -- I think I fixed
        }
    }

    public void SendNegativeJudge()
    {
        Debug.Log("button pressed");
        PostDataFromAFromNegativeJudge();
    }

    public void PostDataFromAFromNegativeJudge()
    {
        UpdateActualListOfInfoForDatabase();


        scoreBoundsBalance = scoreCalculator.scoreBoundsBalance;
        scoreUnityVisual = scoreCalculator.scoreUnityVisual;
        scorePixelsBalance = scoreCalculator.visualScoreBalancePixelsCount;
        scoreLawOfLever = scoreCalculator.scoreLawOfLever;

        g0 = ListOfInfoForDatabase[0];// I can delete this at one point
        g1 = ListOfInfoForDatabase[1];// I can delete this at one point
        g2 = ListOfInfoForDatabase[2];// I can delete this at one point
        g3 = ListOfInfoForDatabase[3];// I can delete this at one point
        g4 = ListOfInfoForDatabase[4];// I can delete this at one point
        g5 = ListOfInfoForDatabase[5]; // I can delete this at one point
        judge = 0;               

        ListOfInfoForDatabase.Clear();

        StartCoroutine(PostData(LoadSceneInputUsername.username));
    }

    public void PostDataForPositiveJudge()
    {
        UpdateActualListOfInfoForDatabase();

        scoreBoundsBalance = scoreCalculator.scoreBoundsBalance;
        scoreUnityVisual = scoreCalculator.scoreUnityVisual;
        scorePixelsBalance = scoreCalculator.visualScoreBalancePixelsCount;
        scoreLawOfLever = scoreCalculator.scoreLawOfLever;

        g0 = ListOfInfoForDatabase[0];// I can delete this at one point
        g1 = ListOfInfoForDatabase[1];// I can delete this at one point
        g2 = ListOfInfoForDatabase[2];// I can delete this at one point
        g3 = ListOfInfoForDatabase[3];// I can delete this at one point
        g4 = ListOfInfoForDatabase[4];// I can delete this at one point
        g5 = ListOfInfoForDatabase[5];// I can delete this at one point
        judge = 1;               

        ListOfInfoForDatabase.Clear();

        StartCoroutine(PostData(LoadSceneInputUsername.username)); //TO FIX

    }

    public void PostDataFromAI(float scorePixelsBalancefromAI, 
                                float scoreUnityVisualFromAI, 
                                float scoreBoundsBalancefromAI,
                                List<float> genesAI, // I can delete this at one point
                                float scoreLawOfLeverFromAI)
    {
        UpdateActualListOfInfoForDatabase();

        scorePixelsBalance = scorePixelsBalancefromAI;
        scoreUnityVisual = scoreUnityVisualFromAI;
        scoreBoundsBalance = scoreBoundsBalancefromAI;
        scoreLawOfLever = scoreLawOfLeverFromAI;

        g0 = genesAI[0];// I can delete this at one point
        g1 = genesAI[1];// I can delete this at one point
        g2 = genesAI[2];// I can delete this at one point
        g3 = genesAI[3];// I can delete this at one point
        g4 = genesAI[4];// I can delete this at one point
        g5 = genesAI[5];// I can delete this at one point
        judge = 0.5f;                  ////////////////////// TODO I can then validate or penalise with my vote...

        ListOfInfoForDatabase.Clear();

        StartCoroutine(PostData("AI_TURN"));
    }

    IEnumerator PostData(string username)
    {
        if (username == "" | username == null)
        {
            username = "Debugging";
        }

        collectdatarendertexture.CollectPixelsValuesFromImageForMainViewRecordInDatabase(); // this update the scriptable object with the image string list pixels values
        collectdatarendertexture.CollectPixelsValuesFromImageForNeuralNetworkDNNOfflineTraining(); // this also update the scriptable object
        byte[] bytesFullViewPaintCam = collectdatarendertexture.CollectPNGMainPaintCameraFullResolution();
        byte[] bytesFrontCam = collectdatarendertexture.CollectPNGFrontViewNN();
        byte[] bytesTopCam = collectdatarendertexture.CollectPNGFTopViewNN();
        WWWForm form = new WWWForm();

        //Debug.Log(genesString);

        form.AddField("name", username);
        form.AddField("filenameImg", "NotImplemented");
        form.AddField("numberOfItems", numberOfItems.ToString()); //
        form.AddField("sequence", sequence); // 
        form.AddField("genesString", genesString);
        form.AddField("scoreBoundsBalance", scoreBoundsBalance.ToString());
        form.AddField("scorePixelsBalance", scorePixelsBalance.ToString());
        form.AddField("scoreUnityVisual", scoreUnityVisual.ToString());
        form.AddField("scoreLawOfLever", scoreLawOfLever.ToString());

        form.AddField("NNtopView", imagePixelsValues.ImageNNtopView.ToString()); // not need anymore
        form.AddField("NNFrontView", imagePixelsValues.ImageNNFrontView.ToString()); // not need anymore
        form.AddField("g0", g0.ToString());// I can delete this at one point
        form.AddField("g1", g1.ToString());// I can delete this at one point
        form.AddField("g2", g2.ToString());// I can delete this at one point
        form.AddField("g3", g3.ToString());// I can delete this at one point
        form.AddField("g4", g4.ToString());// I can delete this at one point
        form.AddField("g5", g5.ToString());// I can delete this at one point
        form.AddField("ImagePixelsList", imagePixelsValues.ImagePixelsListMainPaintView.ToString());
        form.AddField("judge", judge.ToString());

        //form.AddBinaryData("image", bytes, "screenShot.png", "image/png");
        form.AddBinaryData("image", bytesFullViewPaintCam);
        form.AddBinaryData("imageFront", bytesFrontCam);
        form.AddBinaryData("imageTop", bytesTopCam);

        using (UnityWebRequest www = UnityWebRequest.Post("http://www.guidosalimbeni.it/UnityComp/AddToDatabase.php", form))
        {
            //System.Net.ServicePointManager.Expect100Continue = false; // to avoid some proxy blocking
            www.useHttpContinue = false;

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

    


    private void UpdateActualListOfInfoForDatabase()
    {
        sequence = "";
        genesString = "";


        foreach (GameObject element in listOfelementsInComposition)
        {
            ListOfInfoForDatabase.Add(element.transform.position.x);

            ListOfInfoForDatabase.Add(element.transform.position.z);
            ListOfInfoForDatabase.Add(element.transform.localEulerAngles.y);
            sequence += element.GetComponent<TagMeElementOfComposition>().ElementOfCompositionID.ToString() + ",";
            genesString += element.transform.position.x.ToString("0.000") + "," + element.transform.position.z .ToString("0.000") + "," + element.transform.localEulerAngles.y.ToString("0.000") + ",";

        }

        //genesString = string.Join(",", ListOfInfoForDatabase.ToArray());

        numberOfItems = listOfelementsInComposition.Count;
    }


}
