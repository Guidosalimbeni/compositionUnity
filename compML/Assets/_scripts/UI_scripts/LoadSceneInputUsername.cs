using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class LoadSceneInputUsername : MonoBehaviour
{
    private TMP_InputField usernameInputField;
    public int scene;

    static public string username;

    private void Start()
    {
        usernameInputField = GetComponent<TMP_InputField>();
        usernameInputField.onEndEdit.AddListener(TryLoadScene);
    }

    private void TryLoadScene(string username)
    {
        Debug.Log(username);
        SceneManager.LoadSceneAsync(scene);
    }
}
