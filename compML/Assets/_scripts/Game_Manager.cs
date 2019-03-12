using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public AreasObjects areasObjects { get; private set; }
    public static Game_Manager Instance { get; private set; }

    private GameManagerCalculateBalanceAreaBounds calculateBalanceAreaBounds;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            calculateBalanceAreaBounds = GetComponent<GameManagerCalculateBalanceAreaBounds>();

            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        areasObjects = calculateBalanceAreaBounds.CalculateBondsAreas();
    }
}
