using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// read the DNA and tells what to do to the character


public class BrainGA : MonoBehaviour
{
    public int NumberOfgenes = 2; // still need to hard code the genes anyway..
	public float TotalScore;  // THIS IS IMPORTANT SINCE LINKS TO THE SORTED POPULATION INDIVIDUAL BRAIN
	public DNA dna;

    public float scorePixelsBalanceIndividual { get; set; }
    public float scoreUnityVisualIndividual { get; set; }
    public float scoreBoundsBalanceIndividual { get; set; }

    public List<float> genes = new List<float>();

    private int DNALength;
    private ScoreCalculator scoreCalculator; 
    private BrainNN_CompML brainNN_compML;
    private GamePopulationController gamePopulationController;
    

    private void Awake()
    {
        brainNN_compML = FindObjectOfType<BrainNN_CompML>();
        scoreCalculator = FindObjectOfType<ScoreCalculator>();
        gamePopulationController = FindObjectOfType<GamePopulationController>();
        DNALength = gamePopulationController.ElementsCompositions.Count * NumberOfgenes;
        
    }

    public void Init()
	{
        dna = new DNA(DNALength, gamePopulationController.MaxValues_x, gamePopulationController.MinValues_z);
    }

    public void InitForBreed()
    {
        dna = new DNA(DNALength, gamePopulationController.MaxValues_x, gamePopulationController.MinValues_z);
    }

    public void MoveComposition()
    {
        int genePos = 0;
        foreach (var elementComp in gamePopulationController.ElementsCompositions)
        {
            elementComp.transform.position = new Vector3(dna.GetGene(genePos), 0, dna.GetGene(genePos + 1));
            genePos = genePos + NumberOfgenes;
        }
    }

    public void CalculateTotalScore()
    {
        if (scoreCalculator != null && brainNN_compML != null) 
        {

            // call to trigger the events
            // the other calls are from leantouch event trigger
            //openCVmanager.CallForOpenCVCalculationUpdates(); // to update the score pixels balance of opencv..
            //gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores();


            // TODO change this so that it takes the images and calculate the DNN inside unity
            // this is important and avoid me to use genes !!!!!!!!!!!!

            // calculate score comes after movement so genes are updated
            float scoreNN = 0;

            genes.Clear();
            genes = dna.GetGenesList();

            scoreNN = brainNN_compML.PredictFromNN(genes[0], genes[1], genes[2], genes[3], genes[4], genes[5]);  // might want to MULTIPLY
            
            scorePixelsBalanceIndividual = scoreCalculator.visualScoreBalancePixelsCount; 
            scoreBoundsBalanceIndividual = scoreCalculator.scoreBoundsBalance;
            scoreUnityVisualIndividual = scoreCalculator.scoreUnityVisual;

            // this is the MOBILE NET -- still need to add to the total score
            // not returning anything for now... need to implement it.
            scoreCalculator.GetTheScoreFromTheMobileNet();

            // add back the NEURAL NETWORK

            TotalScore = scorePixelsBalanceIndividual + scoreUnityVisualIndividual + scoreBoundsBalanceIndividual;// + scoreNN;
            
        }
    }

    public void MoveCompositionOfBestFitAfterAIfinishedIsTurn(List<float> genes)
    {
        int genePos = 0;
        foreach (var elementComp in gamePopulationController.ElementsCompositions)
        {
            elementComp.transform.position = new Vector3(genes[genePos], 0, genes[genePos + 1]);
            genePos = genePos + NumberOfgenes;
        }
    }
}
