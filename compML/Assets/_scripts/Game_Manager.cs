using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Game_Manager : MonoBehaviour
{
    public AreasObjects areasObjects { get; private set; }
    public static Game_Manager Instance { get; private set; }
    public List<GameObject> elementsOfComposition { get; private set; }

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
            PopulateListOfItemInComposition();
            SetLayerToForeground();

            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        areasObjects = calculateBalanceAreaBounds.CalculateBondsAreas();
    }

    private void PopulateListOfItemInComposition()
    {
        elementsOfComposition = new List<GameObject>();
        var ss = FindObjectsOfType<MonoBehaviour>().OfType<IElementOfComposition>();
        foreach (IElementOfComposition s in ss)
        {
            elementsOfComposition.Add(s.TagMe());
        }
    }

    private void SetLayerToForeground()
    {
        foreach (var item in elementsOfComposition)
        {
            item.layer = 9; // set layer to Foreground
        }
    }
}
