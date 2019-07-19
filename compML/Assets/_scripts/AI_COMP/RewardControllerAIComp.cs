using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardControllerAIComp : MonoBehaviour
{
    private ScoreCalculator scoreCalculator;
    private CollisionChecker collisionChecker;
    private float visualScoreBalancePixelsCount;
    private float scoreBoundsBalance;
    private float scoreUnityVisual;

    // add the other score later..
    // mobile net in primis...


    

    private void Awake()
    {
        scoreCalculator = FindObjectOfType<ScoreCalculator>();
    }

    
    // set negative reward for the trigger on stay...

    // and if it goes away from frame... in this case set done.. so it reset.. implement reset on done in agent...


    public float CalculateTheVisualReward()
    {
        visualScoreBalancePixelsCount = scoreCalculator.visualScoreBalancePixelsCount;
        scoreBoundsBalance = scoreCalculator.visualScoreBalancePixelsCount;
        scoreUnityVisual = scoreCalculator.scoreUnityVisual;


        return (visualScoreBalancePixelsCount + scoreBoundsBalance + scoreUnityVisual) / 3 ; // important to normalise here !!!

    }

    

}
