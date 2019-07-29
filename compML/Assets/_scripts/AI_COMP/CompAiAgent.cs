﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;


// to run the train need to go into AAA folder of ML- agents
// activate ML agent and run the code in it
// enable tensor flow in unity
// the score calculation in tensor flow are only for the reward so once trained i can use barracuda in unity..
// uncheck control and go..

public class CompAiAgent : Agent
{
    // https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Learning-Environment-Design-Agents.md

    [Header("Specific to CompAI")]
    private CompAiAcademy academy;
    public float timeBetweenDecisionsAtInference;
    //public float TimeDelayForPredictionsAndScoreCalculation = 1.0f;

    [Tooltip("Because we want an observation right before making a decision, we can force " +
             "a camera to render before making a decision. Place the agentCam here if using " +
             "RenderTexture as observations.")]
    public Camera CameraTopView;
    public Camera CameraFrontView;

    public bool MyTurn { get; set; } // set in academy for turn

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
    private CollisionChecker collisionChecker;
    private ScoreCalculator scoreCalculator;
    private InferenceNNfomDATABASE inferenceNNfomDATABASE;
    private InferenceCompositionML inferenceCompositionML;
    private InferenceScoreFeatures inferenceScoreFeatures;
    private InferenceFinalOut inferenceFinalOut;

    float distanceToCenter = 0.0f;
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
        scoreCalculator = FindObjectOfType<ScoreCalculator>();
        inferenceNNfomDATABASE = FindObjectOfType<InferenceNNfomDATABASE>();
        inferenceCompositionML = FindObjectOfType<InferenceCompositionML>();
        inferenceScoreFeatures = FindObjectOfType<InferenceScoreFeatures>();
        inferenceFinalOut = FindObjectOfType<InferenceFinalOut>();
    }

    

    public override void CollectObservations()
    {
        distanceToCenter = Vector3.Distance (gameObject.transform.position , CenterOfScene);
        CenterOfMass = colliderBounds.GetCenterOfMass();
        SizeColliderBounds = colliderBounds.GetVectorSizeOfVolumeBounds();
        AngleRotY = transform.localEulerAngles.y;

        AddVectorObs(gameObject.transform.position.x / 2); //normalise ... normalizedValue = (currentValue - minValue)/(maxValue - minValue)
        AddVectorObs(gameObject.transform.position.z / 2);
        AddVectorObs(CompScreenAreaCalculation.percentageScreenOccupiedByItem / 100.0f); // is it 100 or 1?
        
        AddVectorObs(distanceToCenter);
        AddVectorObs(CenterOfMass);
        AddVectorObs(SizeColliderBounds);
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
        inferenceScoreFeatures.CallTOCalculateFeaturesAllScores();
        inferenceNNfomDATABASE.CallTOCalculateNNFrontTopcore();
        inferenceCompositionML.CallTOCalculateMobileNetScore();

        // add call for contour render / edges render / hog renderes...

        // plus nn ? or nn only for reward ?? see above comment per solution..
        inferenceFinalOut.CallTOCalculateFinalOutScore();

        CalculateRewards(); 


        // all the action wait for the decisions rules below
    }


    //IEnumerator WaitForModelToPredictForReward()
    //{

    //    yield return new WaitForSeconds(TimeDelayForPredictionsAndScoreCalculation);
        
    //}


    private void CalculateRewards()
    {
        // REMEMBER TO ADD PENALTIES FOT COLLISION TO OTHER OBJECTS
        // PENALTY FOR COLLISION WITH THE OUT OF FRAME

        float TotalOutScore = scoreCalculator.scoreFinalOut;

        //if (TotalOutScore < 0.05f)
        //{
        //    //Done();
        //    SetReward(-0.05f);
        //}
        //if (TotalOutScore > 0.1f)
        //{
        //    //Done();
        //    SetReward(0.1f);
        //}
        //if (TotalOutScore > 0.2f)
        //{
        //    //Done();
        //    SetReward(0.2f);
        //}
        if (TotalOutScore > 0.2f)
        {
            //Done();
            SetReward(0.5f);
        }
        if (TotalOutScore > 0.5f)
        {
            //Done();
            SetReward(1.0f);
        }

        // collision with other object and frames bounds
        if (collisionChecker.CollisionWithOtherBoundsFrames == true)
        {
            Done();
            SetReward(-1.0f);
        }

        // collision with other object and frames bounds
        //if (collisionChecker.CollisionWithOtherItemFoundForAIReward == true)
        //{
        //    SetReward(-0.1f);
        //}

        // REALLY NEED TO PASS TOP CAMERA VIEW TO OF SINGLE ATTACHED CAMERA AND 20 x 20 or 40 x 40 to vector non visual observation !!!
        // also add HOGS 

        // IMPORTANT THERE IS NO WAY TO KNOW IF OTHER OBJECTS ARE CLOSE UNLESS I USE NN from training not cnn from ml-agents 
        // if i want to collect observation with external NN in tensorflow I cannot then inference with both tensor flow and barracuda...
        // I check if there is only DNN in yaml option
        // https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Training-PPO.md
        // the trick is to pass the observation to vector observation so that they are not treated for cnn encoding!!!!
    }


    // changed so it is only for decision one after the other

    public void FixedUpdate()
    {
        //if (MyTurn == true)
        //{
        //    Debug.Log(MyTurn + "   " + gameObject.name);
            
        //    MyTurn = false;

        //}

        WaitTimeInference();

    }

    private void WaitTimeInference() // this is mainly for having the time for the MobileNet to make prediction and the NN, opencv..etc
    {
        if (CameraTopView != null && CameraFrontView != null)
        {
            CameraTopView.Render(); // might not need this call
            CameraFrontView.Render(); // might not need this call



        }

        else { Debug.Log("check that cameras are attached to game object AI agents scripts !!!!!!!!!!!!!!!!!"); }

        if (!academy.GetIsInference())  // so if get inference is true this is false and the agent makes decision normally otherwise uses this decision call
        {
            RequestDecision(); // eventually set a rule to wait other to make their action ... but maybe soved bt stacked .. or try to use recurrent..
        }
        else
        {

            if (timeSinceDecision >= timeBetweenDecisionsAtInference) // now is set in the academy
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
        float X = Random.Range(-1.0f, 1.0f);
        float Z = Random.Range(-1.0f, 1.0f);

        gameObject.transform.position = new Vector3(X, 0.0f, Z);
        // reset position if the item goes behond the fram boundary...

    }
}
