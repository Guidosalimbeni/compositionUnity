﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendToDatabase : MonoBehaviour
{
    // for opencv score usually I created an events from Opencv Manager
    // for not opencv score usually to access scores via components and methods from UI manager

    public OpenCVManager openCvManager;
    private float scorePixelsBalance;
    private ColorGradientBoundShapeBalance colorgradientBoundsShapeBalance;
    private ColorGradientVisualUnity colorGradientVisualUnity;

    private void Awake()
    {
        openCvManager.OnPixelsCountBalanceChanged += HandleOnPixelsCountBalanceChanged;
        colorgradientBoundsShapeBalance = GetComponent<ColorGradientBoundShapeBalance>();
        colorGradientVisualUnity = GetComponent<ColorGradientVisualUnity>();
    }

    private void HandleOnPixelsCountBalanceChanged(float scoreOnpixelscountbalance)
    {
        scorePixelsBalance = scoreOnpixelscountbalance;
    }

    public void PostDataFromButton()
    {
        StartCoroutine(PostData(LoadSceneInputUsername.username));
    }

    IEnumerator PostData(string username)
    {
        if (username == "" | username == null)
        {
            username = "Debugging";
        }

        float scoreBoundsBalance = colorgradientBoundsShapeBalance.GetvisualScoreBalanceBoundsShapes();
        float scoreUnityVisual = colorGradientVisualUnity.GetVisualUnityScore();

        WWWForm form = new WWWForm();
        form.AddField("name", username);
        form.AddField("filenameImg", "NotImplemented");
        form.AddField("scoreBoundsBalance", scoreBoundsBalance.ToString());
        form.AddField("scorePixelsBalance", scorePixelsBalance.ToString());
        form.AddField("scoreUnityVisual", scoreUnityVisual.ToString());

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
