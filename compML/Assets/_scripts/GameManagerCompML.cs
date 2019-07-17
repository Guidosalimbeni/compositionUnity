using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerCompML : MonoBehaviour
{

    public static GameManagerCompML Instance { get; private set; }


    public int Lives { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Lives = 0;
        }
    }


}
