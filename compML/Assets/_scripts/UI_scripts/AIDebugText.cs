using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AIDebugText : MonoBehaviour
{
    public bool DebugIsON = false;
    public TextMeshProUGUI RewardText;
    public TextMeshProUGUI ScoreText;
    private CompAiAgent[] agents;
    private float totalRewardNow;
    private float scoreUnityVisual;
    private ScoreCalculator scoreCalculator;
    [SerializeField]
    private List<float> cumulativeRewards; // for see in the inspector

    private void Start()
    {
        cumulativeRewards = new List<float>();
        agents = FindObjectsOfType<CompAiAgent>();
        scoreCalculator = FindObjectOfType<ScoreCalculator>();
    }

    private void Update()
    {
        if (DebugIsON)
        {
            foreach (var a in agents)
            {
                cumulativeRewards.Clear();
                CompAiAgent agentAi = a.GetComponent<CompAiAgent>();
                cumulativeRewards.Add(agentAi.GetCumulativeReward());
            }

            totalRewardNow = 0.0f;

            foreach (var a in agents)
            {
                CompAiAgent agentAi = a.GetComponent<CompAiAgent>();

                totalRewardNow += agentAi.GetReward();
                totalRewardNow = totalRewardNow / agents.Length;
            }

            scoreUnityVisual = scoreCalculator.scoreUnityVisual;
            UpdateAIDebugText(totalRewardNow, scoreUnityVisual);
        }
    }

    public void UpdateAIDebugText(float reward, float score)
    {
        RewardText.text = reward.ToString();
        ScoreText.text = score.ToString();
    }
}
