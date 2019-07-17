using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorOfItemInCompositionStart : MonoBehaviour
{
    public List<GameObject> CompositionElementsPrefabs;

    private List<int> selectedItemIDS;
    private List<GameObject> CompositionElementsPrefabsSELECTED;

    private void Start()
    {
        selectedItemIDS = new List<int>();
    }

    public void PopulateSelectedItemIdsFromButton(int ID)
    {
        selectedItemIDS.Add(ID);
    }

    public void RemoveUnselectedItemIdsFromButton(int ID)
    {
        selectedItemIDS.Remove(ID);
    }

    public void PopulateElementsCompositionsSelected()
    {
        foreach( int id in selectedItemIDS)
        {
            CompositionElementsPrefabsSELECTED.Add(CompositionElementsPrefabs[id - 1]);
        }

        GameManagerCompML.Instance.PopulateSelectedElementOfComposition(CompositionElementsPrefabsSELECTED);
    }

    public void LoadLevel(int sceneID)
    {
        SceneManager.LoadScene(sceneID);

    }

}
