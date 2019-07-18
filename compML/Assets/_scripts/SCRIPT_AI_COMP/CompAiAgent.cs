using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CompAiAgent : Agent
{
    // https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Learning-Environment-Design-Agents.md

    [Header("Specific to CompAI")]
    private CompAiAcademy academy;
    public float timeBetweenDecisionsAtInference;
    private float timeSinceDecision;

    [Tooltip("Because we want an observation right before making a decision, we can force " +
             "a camera to render before making a decision. Place the agentCam here if using " +
             "RenderTexture as observations.")]
    public Camera renderCamera;


    private CompScreenAreaCalculation CompScreenAreaCalculation;

    public override void InitializeAgent()
    {
        academy = FindObjectOfType(typeof(CompAiAcademy)) as CompAiAcademy;
        CompScreenAreaCalculation = GetComponent<CompScreenAreaCalculation>();
    }

    

    public override void CollectObservations()
    {
        AddVectorObs(gameObject.transform.position.x ); //normalise ...
        AddVectorObs(gameObject.transform.position.z);
        AddVectorObs(CompScreenAreaCalculation.percentageScreenOccupiedByItem); // is it 100 or 1?

    }

    // to be implemented by the developer
    public override void AgentAction(float[] vectorAction, string textAction)
    {


        //float torque_x = Mathf.Clamp(vectorAction[0], -1, 1) * 100f;
        //float torque_z = Mathf.Clamp(vectorAction[1], -1, 1) * 100f;
        //rbA.AddTorque(new Vector3(torque_x, 0f, torque_z));

        //torque_x = Mathf.Clamp(vectorAction[2], -1, 1) * 100f;
        //torque_z = Mathf.Clamp(vectorAction[3], -1, 1) * 100f;
        //rbB.AddTorque(new Vector3(torque_x, 0f, torque_z));

        // need to rescale to the dimension of the composition table...

    }


    public void FixedUpdate()
    {
        WaitTimeInference();
    }

    private void WaitTimeInference()
    {
        if (renderCamera != null)
        {
            renderCamera.Render();
        }

        if (!academy.GetIsInference())
        {
            RequestDecision(); // eventually set a rule to wait other to make their action ... but maybe soved bt stacked .. or try to use recurrent..
        }
        else
        {
            if (timeSinceDecision >= timeBetweenDecisionsAtInference)
            {
                timeSinceDecision = 0f;
                RequestDecision();
            }
            else
            {
                timeSinceDecision += Time.fixedDeltaTime;
            }
        }
    }


}
