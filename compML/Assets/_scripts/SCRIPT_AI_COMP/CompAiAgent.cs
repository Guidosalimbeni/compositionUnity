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


    public override void InitializeAgent()
    {
        academy = FindObjectOfType(typeof(CompAiAcademy)) as CompAiAcademy;
    }

    public override void CollectObservations()
    {
        // There are no numeric observations to collect as this environment uses visual
        // observations.

    }

    // to be implemented by the developer
    public override void AgentAction(float[] vectorAction, string textAction)
    {






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
            RequestDecision();
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
