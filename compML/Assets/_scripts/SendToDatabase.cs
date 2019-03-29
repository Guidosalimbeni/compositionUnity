using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendToDatabase : MonoBehaviour
{

    private void Start()
    {
        Debug.Log(LoadSceneInputUsername.username);
        StartCoroutine(PostData(LoadSceneInputUsername.username));
    }

    IEnumerator PostData(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", username);
        form.AddField("filenameImg", "value2");
        form.AddField("scoreBoundsBalance", 22 );
        form.AddField("scorePixelsBalance", 11);
        form.AddField("scoreUnityVisual", 72);

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
