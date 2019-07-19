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
    

    [Tooltip("Because we want an observation right before making a decision, we can force " +
             "a camera to render before making a decision. Place the agentCam here if using " +
             "RenderTexture as observations.")]
    public Camera renderCamera;

    // Speed of agent rotation.
    public float turnSpeed = 300;
    // Speed of agent movement.
    public float moveSpeed = 2;

    private float timeSinceDecision;
    private CompScreenAreaCalculation CompScreenAreaCalculation;
    private Rigidbody agentRb; // not used
    private ColliderBounds colliderBounds;
    // for reward calls
    private OpenCVManager openCVManager;
    private GameVisualManager gameVisualManager;
    private RewardControllerAIComp rewardControllerAIComp;
    private CollisionChecker collisionChecker;

    float distaceToCenter = 0.0f;
    Vector3 CenterOfScene = Vector3.zero;
    Vector3 CenterOfMass = Vector3.zero;
    Vector3 SizeColliderBounds = Vector3.zero;
    float AngleRotY = 0.0f;
    float VolumeOfTheItem = 0.0f;

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        academy = FindObjectOfType(typeof(CompAiAcademy)) as CompAiAcademy;
        CompScreenAreaCalculation = GetComponent<CompScreenAreaCalculation>();
        agentRb = GetComponent<Rigidbody>(); // not used
        colliderBounds = GetComponent<ColliderBounds>();
        collisionChecker = GetComponent<CollisionChecker>();
        openCVManager = FindObjectOfType<OpenCVManager>();
        gameVisualManager = FindObjectOfType<GameVisualManager>();
        rewardControllerAIComp = FindObjectOfType<RewardControllerAIComp>();

        // TODO the mobile net score might need to be separated since its call only .. or maybe I can also update UI? ,..  but another MobileNetManagerScript..with action...etc
    }

    

    public override void CollectObservations()
    {


        
        distaceToCenter = Vector3.Distance (gameObject.transform.position , CenterOfScene);
        CenterOfMass = colliderBounds.GetCenterOfMass();
        SizeColliderBounds = colliderBounds.GetVectorSizeOfVolumeBounds();
        AngleRotY = transform.localEulerAngles.y;

        AddVectorObs(gameObject.transform.position.x / 2); //normalise ... normalizedValue = (currentValue - minValue)/(maxValue - minValue)
        AddVectorObs(gameObject.transform.position.z / 2);
        AddVectorObs(CompScreenAreaCalculation.percentageScreenOccupiedByItem); // is it 100 or 1?
        Debug.Log(CompScreenAreaCalculation.percentageScreenOccupiedByItem);
        AddVectorObs(distaceToCenter);
        AddVectorObs(CenterOfMass);
        AddVectorObs(SizeColliderBounds / 2);
        AddVectorObs(AngleRotY);
        AddVectorObs(VolumeOfTheItem);

        // 1 + 1 + 1 + 1 + 3 + 3 + 1 + 1 = 12

        

    }

    // if I use one brain for all the item I will need to set a lot of action here.. for each ??? not sure..
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        var movement = (int)vectorAction[0];

        Vector3 dirToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;

        switch (movement)
        {
            case 0:
                // stay in place
                break;
            case 1:
                //dirToGo = transform.forward;
                dirToGo = new Vector3(0.0f, 0.0f, -1.0f);
                break;
            case 2:
                //dirToGo = -transform.forward;
                dirToGo = new Vector3(0.0f, 0.0f, 1.0f);
                break;
            case 3:
                //dirToGo = transform.right;
                dirToGo = new Vector3(1.0f, 0.0f, 0.0f);
                break;
            case 4:
                //dirToGo = -transform.right;
                dirToGo = new Vector3(-1.0f, 0.0f, 0.0f);
                break;
            case 5:
                rotateDir = -transform.up;
                break;
            case 6:
                rotateDir = transform.up;
                break;

        }

        transform.Rotate(rotateDir, Time.deltaTime * turnSpeed);
        //agentRb.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange);

        // banana collector is the good example here for alos changing to continuos values

        // I could use WITHOUT RIGID BODY

        transform.Translate(dirToGo * Time.deltaTime * moveSpeed, Space.World);


        // call here so that both for wait decision and for inference will update the UI and the scores
        openCVManager.CallForOpenCVCalculationUpdates();
        gameVisualManager.CallTOCalculateNOTOpenCVScores();

        // ADD THE CALL TO MOBILE NET HERE... need to wait here???

        // plus nn ? or nn only for reward ??

        // REMEMBER TO ADD PENALTIES FOT COLLISION TO OTHER OBJECTS
        // PENALTY FOR COLLISION WITH THE OUT OF FRAME

        float visualTotalReward = rewardControllerAIComp.CalculateTheVisualReward();

        AddReward(visualTotalReward);

        if (collisionChecker.CollisionWithOtherItemFoundForAIReward == true)
        {
            AddReward(-1.0f);
        }

        if (gameObject.transform.position.x < -2.0f || gameObject.transform.position.x > 2.0f)
        {
            AddReward(-1.0f);
            Done();
        }

        if (gameObject.transform.position.z < -2.0f || gameObject.transform.position.z > 2.0f)
        {
            AddReward(-1.0f);
            Done();
        }

        if (distaceToCenter > 1.5f)
        {
            AddReward(-0.1f);
        }

    }


    public void FixedUpdate()
    {
        WaitTimeInference();
    }

    private void WaitTimeInference() // this is mainly for having the time for the MobileNet to make prediction and the NN, opencv..etc
    {
        if (renderCamera != null)
        {
            renderCamera.Render();
        }

        if (!academy.GetIsInference())  // so if get inference is true this is false and the agent makes decision normally otherwise uses this decision call
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

    public override void AgentReset()
    {
        gameObject.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        // reset position if the item goes behond the fram boundary...


    }
}
