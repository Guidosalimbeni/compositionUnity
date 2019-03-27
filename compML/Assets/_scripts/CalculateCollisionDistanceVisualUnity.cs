using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateCollisionDistanceVisualUnity : MonoBehaviour
{
    public bool drawRenderedLinesDebug;

    private LineRenderer lineRenderer;
    private List<List<GameObject>> PairwaiseElementsOfComposition;
    private List<GameObject> pair;
    private List<GameObject> LineRendererObjectsList;
    

    private void Start()
    {
        LineRendererObjectsList = new List<GameObject>();

        PairwaiseElementsOfComposition = PairwaiseOperation(Game_Manager.Instance.ElementsOfComposition);
        for (int i = 0; i < PairwaiseElementsOfComposition.Count; i++)
        {
            GameObject ObjectForLineRenderer = new GameObject("GameObject_LineRenderer_" + i.ToString());
            LineRenderer lineRenderer = ObjectForLineRenderer.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.widthMultiplier = 0.02f;

            LineRendererObjectsList.Add(ObjectForLineRenderer);
        }
    }
    
    void Update()
    {
        DrawAndCalculateDistances(PairwaiseElementsOfComposition);
    }

    void DrawAndCalculateDistances(List<List<GameObject>>  PairwaiseElementsOfComposition)
    {
        int i = 0;
        foreach (List<GameObject> pair in PairwaiseElementsOfComposition)
        {
            MeshCollider MeshColStart = pair[0].GetComponent<MeshCollider>();
            Vector3 MeshCollStart = MeshColStart.ClosestPointOnBounds(pair[1].transform.position);

            MeshCollider MeshColEnd = pair[1].GetComponent<MeshCollider>();
            Vector3 MeshCollEnd = MeshColEnd.ClosestPointOnBounds(pair[0].transform.position);

            CalculateDistanceOf2MeshColliders(MeshCollStart, MeshCollEnd);

            if(drawRenderedLinesDebug == true)
            {
                LineRenderer lineRendererPair = LineRendererObjectsList[i].GetComponent<LineRenderer>();
                lineRendererPair.SetPosition(0, MeshCollStart);
                lineRendererPair.SetPosition(1, MeshCollEnd);
                i++;
            }
            
        }
    }


    List<List<GameObject>> PairwaiseOperation(List<GameObject> ElementsOfComposition)
    {
        List<List<GameObject>>  result = new List<List<GameObject>>();

        for (int i = 0; i < ElementsOfComposition.Count; i++)
        {
            for (int j = i; j < ElementsOfComposition.Count; j++)
            {
                if (i != j)
                {
                    pair = new List<GameObject>();
                    pair.Add(ElementsOfComposition[i]);
                    pair.Add(ElementsOfComposition[j]);
                    result.Add(pair);
                }
            }
        }
        return result;
    }

    void CalculateDistanceOf2MeshColliders(Vector3 start, Vector3 end)
    {
        float dist = Vector3.Distance(start, end);
        Debug.Log(dist);
    }
}
