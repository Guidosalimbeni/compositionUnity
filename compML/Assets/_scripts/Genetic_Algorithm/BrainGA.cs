using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// read the DNA and tells what to do to the character


public class BrainGA : MonoBehaviour
{
    public int DNALength = 6;
	public float TotalScore;  //
	public DNA dna;

    public float scorePixelsBalanceIndividual { get; set; }
    public float scoreUnityVisualIndividual { get; set; }
    public float scoreBoundsBalanceIndividual { get; set; }

    public List<float> genes = new List<float>();

    private PopulationManager population_manager;
    private ScoreCalculator scoreCalculator;
    private BrainNN_CompML brainNN_compML;
    //private OpenCVManager openCVmanager;
    //private Game_Manager gameManagerNotOpenCV;

    private void Awake()
    {
        brainNN_compML = FindObjectOfType<BrainNN_CompML>();
        
        population_manager = FindObjectOfType<PopulationManager>();
        
        scoreCalculator = FindObjectOfType<ScoreCalculator>();

        //openCVmanager = FindObjectOfType<OpenCVManager>();
        //gameManagerNotOpenCV = FindObjectOfType<Game_Manager>();
    }

    public void Init()
	{
        dna = new DNA(DNALength, population_manager.MaxValues_x, population_manager.MinValues_z);
    }

    public void InitForBreed()
    {
        dna = new DNA(DNALength, population_manager.MaxValues_x, population_manager.MinValues_z);
    }

    public void MoveComposition()
    {
        int genePos = 0;
        foreach (var elementComp in population_manager.elementsCompositions)
        {
            elementComp.transform.position = new Vector3(dna.GetGene(genePos), 0, dna.GetGene(genePos + 1));
            genePos = genePos + 2;
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

            // calculate score comes after movement so genes are updated
            float scoreNN = 0;

            genes.Clear();
            genes = dna.GetGenesList();

            scoreNN = brainNN_compML.PredictFromNN(genes[0], genes[1], genes[2], genes[3], genes[4], genes[5]);  // might want to MULTIPLY
            
            scorePixelsBalanceIndividual = scoreCalculator.visualScoreBalancePixelsCount; 
            scoreBoundsBalanceIndividual = scoreCalculator.scoreBoundsBalance;
            scoreUnityVisualIndividual = scoreCalculator.scoreUnityVisual;

            // add back the NEURAL NETWORK

            TotalScore = scorePixelsBalanceIndividual + scoreUnityVisualIndividual + scoreBoundsBalanceIndividual;// + scoreNN;
            

        }

    }

    

    public void MoveCompositionOfBestFitAfterAIfinishedIsTurn(List<float> genes)
    {
        int genePos = 0;
        foreach (var elementComp in population_manager.elementsCompositions)
        {
            elementComp.transform.position = new Vector3(genes[genePos], 0, genes[genePos + 1]);
            genePos = genePos + 2;
        }
    }
}
