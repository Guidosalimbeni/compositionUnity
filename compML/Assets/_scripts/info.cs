using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class info : MonoBehaviour
{
    public static float number = 5;

    private void Start()
    {
        number = Random.Range(-10, 10);
    }

}
