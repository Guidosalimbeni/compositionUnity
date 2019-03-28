using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CalculateCollisionDistanceVisualUnity : MonoBehaviour
{
    public bool drawRenderedLinesDebug;
    public float weight = 0.36f;

    private LineRenderer lineRenderer;
    private List<List<GameObject>> PairwaiseElementsOfComposition;
    private List<GameObject> pair;
    private List<GameObject> LineRendererObjectsList;
    private bool OverlappingColliders = false;

    public float VisualUnityScore { get; private set; }

    private Game_Manager gamemanager;

    private void Awake()
    {
        gamemanager = FindObjectOfType<Game_Manager>();
    }

    private void Start()
    {
        LineRendererObjectsList = new List<GameObject>();

        PairwaiseElementsOfComposition = PairwaiseOperation(gamemanager.ElementsOfComposition);
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
        float sumOfDistances = 0;
        int i = 0;
        foreach (List<GameObject> pair in PairwaiseElementsOfComposition)
        {
            MeshCollider MeshColStart = pair[0].GetComponent<MeshCollider>();
            Vector3 MeshCollStart = MeshColStart.ClosestPointOnBounds(pair[1].transform.position);

            MeshCollider MeshColEnd = pair[1].GetComponent<MeshCollider>();
            Vector3 MeshCollEnd = MeshColEnd.ClosestPointOnBounds(pair[0].transform.position);

            if (drawRenderedLinesDebug == true)
            {
                LineRenderer lineRendererPair = LineRendererObjectsList[i].GetComponent<LineRenderer>();
                lineRendererPair.SetPosition(0, MeshCollStart);
                lineRendererPair.SetPosition(1, MeshCollEnd);
                i++;
            }

            if (OverlappingColliders == false)
            {
                float dist = CalculateDistanceOf2MeshColliders(MeshCollStart, MeshCollEnd);
                sumOfDistances += dist;
            }

            else if (OverlappingColliders == true)
            {
                VisualUnityScore = 0;
                OverlappingColliders = false;
                return;
            }
        }
        VisualUnityScore = Mathf.Exp(-sumOfDistances * weight); 
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

    float CalculateDistanceOf2MeshColliders(Vector3 start, Vector3 end)
    {
        float dist = Vector3.Distance(start, end);
        return dist;
    }

    public void FoundCollisionOfCompositionalElements()
    {
        OverlappingColliders = true;
    }
}
