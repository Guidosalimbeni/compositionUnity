using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCompositionImage : MonoBehaviour
{

    public CompositionImage mainCompImage224;
    public RenderTexture camRenderTexture; // this is 224 * 224 for mobile net

    public void AssignCompRenderToTexture()
    {
        Texture2D imgTexture = ToTexture2D(camRenderTexture);
        mainCompImage224.CompImage = imgTexture;
    }


    private Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new UnityEngine.Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
