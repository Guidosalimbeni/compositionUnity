using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CompAiAcademy : Academy
{

    // not sure if makes sense to use reset parameters as in grid search example
    public int StepToWait = 20;

    private List<GameObject> ElementsCompositions;
    private int NumberOfAgents;
    private int step;
    private int time;
    //public Transform parentAgentTransform;
    
    //  Prepare the environment the first time it launches.
    public override void InitializeAcademy()
    {
        //this below should be copied for the INFERENCE

        //if (GameManagerCompML.Instance.ElementsCompositionsSelected != null)
        //{
        //    List<GameObject> prefabs = GameManagerCompML.Instance.ElementsCompositionsSelected;

        //    foreach (GameObject prefab in prefabs)
        //    {
        //        GameObject go = Instantiate(prefab, parentAgentTransform);
        //    }
        //}

        //ElementsCompositions = new List<GameObject>();
        //TagMeElementOfComposition[] elementstags = FindObjectsOfType<TagMeElementOfComposition>();
        //foreach (var elementtag in elementstags)
        //{
        //    GameObject go = elementtag.gameObject;
        //    ElementsCompositions.Add(go);
        //}

        //NumberOfAgents = ElementsCompositions.Count;

        //foreach (var elem in ElementsCompositions)
        //{
        //    elem.GetComponent<CompAiAgent>().MyTurn = false;
        //}
    }


    // Prepare the environment and Agents for the next training episode.
    // Use this function to place and initialize entities in the scene as necessary.
    public override void AcademyReset()
    {
        // this can bu useful to change the prefab in the scene... but in this case need to detached
        // from initial selection in selection scene

        // or can randomly select the number of items and the items based on the selection of selection
        // scene

        step = 0;
        time = 0;
    }

    // Prepare the environment for the next simulation step. 
    // The base Academy class calls this function before calling any AgentAction() methods for the current step. 
    // You can use this function to update other objects in the scene before the Agents take their actions. 
    // Note that the Agents have already collected their observations and 
    // chosen an action before the Academy invokes this method.

    public override void AcademyStep()
    {
        //if (time % StepToWait == 0)
        //{
        //    if (step % NumberOfAgents == 0)
        //    {
        //        step = 0;
        //    }
        //    ElementsCompositions[step].GetComponent<CompAiAgent>().MyTurn = true;
        //    step++;
        //}

        //time++;


        //Debug.Log(" i am a step in the academy");
        // this can be used to change the light ? 
        // the color of the object?
        // change the camera position for collecting observation ?


    }




}
