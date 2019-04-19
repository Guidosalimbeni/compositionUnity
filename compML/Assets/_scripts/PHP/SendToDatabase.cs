﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class SendToDatabase : MonoBehaviour
{
    // for opencv score usually I created an events from Opencv Manager
    // for not opencv score usually to access scores via components and methods from UI manager

    public OpenCVManager openCvManager;
    public ImageMatrixData imagePixelsValues;
    public float scorePixelsBalance { private set; get; }
    public float scoreBoundsBalance { private set; get; } //
    public float scoreUnityVisual { private set; get; } //
    public int MaxMoves = 4;
    public int sendaftereverymoves = 3;
    public GameObject gamemanager;

    private CollectDataRenderTexture collectdatarendertexture;
    private Game_Manager GM;
    private float CurrentTotalScore;
    private ColorGradientBoundShapeBalance colorgradientBoundsShapeBalance;
    private ColorGradientVisualUnity colorGradientVisualUnity;
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
    private int moves;

    private void Awake()
    {
        openCvManager.OnPixelsCountBalanceChanged += HandleOnPixelsCountBalanceChanged; //
        collectdatarendertexture = FindObjectOfType<CollectDataRenderTexture>();
        colorgradientBoundsShapeBalance = GetComponent<ColorGradientBoundShapeBalance>();
        colorGradientVisualUnity = GetComponent<ColorGradientVisualUnity>();
        populationmanager = FindObjectOfType<PopulationManager>();
        GM = gamemanager.GetComponent<Game_Manager>();
    }

    private void Start()
    {
        if (GM != null)
        {
            listOfelementsInComposition = GM.getListOfItemInComposition();
        }
    }

    private void Update()
    {
        scoreBoundsBalance = colorgradientBoundsShapeBalance.GetvisualScoreBalanceBoundsShapes();
        scoreUnityVisual = colorGradientVisualUnity.GetVisualUnityScore();
        CurrentTotalScore = scorePixelsBalance + scoreBoundsBalance + scoreUnityVisual;

        if (populationmanager.triggerAI == false && CurrentTotalScore != 0.0f)
        {

            if (Input.GetKeyDown(KeyCode.Mouse0) && EventSystem.current.IsPointerOverGameObject() == false && moves < MaxMoves)
            {
                if (moves != 0 && moves % sendaftereverymoves == 0)
                {
                    PostDataFromAttemptsOfMoves();
                }
                moves++;
            }
        }

        if (populationmanager.triggerAI == true)
        {
            moves = 0;
        }
    }
   
    private void HandleOnPixelsCountBalanceChanged(float scoreOnpixelscountbalance)
    {
        scorePixelsBalance = scoreOnpixelscountbalance;
    }


    public void PostDataFromAttemptsOfMoves()
    {
        listOfgenes = new List<float>();
        foreach (GameObject element in listOfelementsInComposition)
        {
            listOfgenes.Add(element.transform.position.x);
            listOfgenes.Add(element.transform.position.z);
        }

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

        g0 = listOfgenes[0];
        g1 = listOfgenes[1];
        g2 = listOfgenes[2];
        g3 = listOfgenes[3];
        g4 = listOfgenes[4];
        g5 = listOfgenes[5];
        judge = 1;               //////////////////

        listOfgenes.Clear();

        StartCoroutine(PostData(LoadSceneInputUsername.username));
        populationmanager.triggerAI = true;
    }

    public void PostDataFromAI(float scorePixelsBalancefromAI, 
                                float scoreUnityVisualFromAI, 
                                float scoreBoundsBalancefromAI,
                                float g0AI, float g1AI, float g2AI, float g3AI, float g4AI, float g5AI)
    {
        scorePixelsBalance = scorePixelsBalancefromAI;
        scoreUnityVisual = scoreUnityVisualFromAI;
        scoreBoundsBalance = scoreBoundsBalancefromAI;
        g0 = g0AI;
        g1 = g1AI;
        g2 = g2AI;
        g3 = g3AI;
        g4 = g4AI;
        g5 = g5AI;
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