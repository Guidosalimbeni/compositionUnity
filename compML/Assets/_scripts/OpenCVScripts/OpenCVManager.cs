﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using System.IO;

public class OpenCVManager : MonoBehaviour
{
    public static OpenCVManager instanceCV2 { get; private set; }

    public Mat imgMat { get; private set; }
    public Mat imgMaskMat { get; private set; }

    public RenderTexture camRenderTexture;
    public RenderTexture camMaskTexture;

    public GameObject plane;
    public static Texture2D texture;

    public bool calcAreaBalanceOpenCV = true;

    [SerializeField]
    private CalculateAreaTot calculateAreaTotal;

    private void Awake()
    {
        if (instanceCV2 != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instanceCV2 = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void Update()
        {
            //if true, The error log of the Native side OpenCV will be displayed on the Unity Editor Console.
            Utils.setDebugMode(true);

            Texture2D imgTexture = toTexture2D(camRenderTexture);
            Texture2D imgMaskTexture = toTexture2D(camMaskTexture);
        
            imgMat = new Mat(imgTexture.height, imgTexture.width, CvType.CV_8UC4);

            Utils.texture2DToMat(imgTexture, imgMat);

            // for debugging TODO delete
            texture = new Texture2D(imgMat.cols(), imgMat.rows(), TextureFormat.RGBA32, false);
            Utils.matToTexture2D(imgMat, texture);
            plane.GetComponent<Renderer>().material.mainTexture = texture;
            //

            if (calcAreaBalanceOpenCV == true)
            {
                calculateAreaTotal.CalculateAreaBalanceUsingOpenCV(imgMaskTexture);
            }

            Utils.setDebugMode(false);

        }

    private Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new UnityEngine.Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    public void SaveTextureFromButton()
    {
        SaveTexturePNG(texture);
    }

    private void SaveTexturePNG(Texture2D tex)
    {
        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Object.Destroy(tex);

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);
    }
}
