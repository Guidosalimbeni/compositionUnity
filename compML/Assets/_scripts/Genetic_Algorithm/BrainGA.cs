using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// read the DNA and tells what to do to the character


public class BrainGA : MonoBehaviour
{

    //public GameObject elemComp_a_from_b; // do I still need this?
    //public GameObject elemComp_b_from_b;
    //public GameObject elemComp_c_from_b;

    private ScoreCalculator scoreCalculator;

    public BrainNN_CompML brainNN_compML;
    public OpenCVManager openCVmanager;

    public int DNALength = 6;
	public float TotalScore;  //
	public DNA dna;

    public float scorePixelsBalanceIndividual { get; set; }
    public float scoreUnityVisualIndividual { get; set; }
    public float scoreBoundsBalanceIndividual { get; set; }


    public List<float> genes = new List<float>();

    private PopulationManager population_manager;

    private void Awake()
    {
        brainNN_compML = FindObjectOfType<BrainNN_CompML>();
        openCVmanager = FindObjectOfType<OpenCVManager>();
        population_manager = FindObjectOfType<PopulationManager>();
    }

    public void Init()
	{
        dna = new DNA(DNALength, population_manager.MaxValues_x, population_manager.MinValues_z);
        scoreCalculator = FindObjectOfType<ScoreCalculator>();

        MoveComposition(); //////

    }

    public void InitForBreed()
    {
        dna = new DNA(DNALength, population_manager.MaxValues_x, population_manager.MinValues_z);
        scoreCalculator = FindObjectOfType<ScoreCalculator>();

    }


    public void CalculateTotalScore()
    {

        if (scoreCalculator != null && brainNN_compML != null) 
        {

            openCVmanager.CallTOCalculateAreaLeftRight(); // to update the score pixels balance of opencv..
            // calculate score comes after movement so genes are updated
            float scoreNN = 0;

            genes.Clear();
            genes = dna.GetGenesList();

            scoreNN = brainNN_compML.PredictFromNN(genes[0], genes[1], genes[2], genes[3], genes[4], genes[5]);  // might want to MULTIPLY

            TotalScore = scoreCalculator.scorePixelsBalance + scoreCalculator.scoreUnityVisual + scoreCalculator.scoreBoundsBalance + scoreNN;

            scorePixelsBalanceIndividual = scoreCalculator.scorePixelsBalance; 
            scoreUnityVisualIndividual = scoreCalculator.scoreUnityVisual;
            scoreBoundsBalanceIndividual = scoreCalculator.scoreBoundsBalance;
        }

    }

    public void MoveComposition()
    {
        int genePos = 0;
        foreach (var elementComp in population_manager.elementsCompositions)
        {
            
            elementComp.transform.position = new Vector3(dna.GetGene(genePos), 0, dna.GetGene(genePos + 1));
            genePos = genePos +2;
        }
    }

    public void MoveCompositionOfBestFitAfterAIfinishedIsTurn(List<float> genes)
    {

        // pass genes
        int genePos = 0;
        foreach (var elementComp in population_manager.elementsCompositions)
        {
            elementComp.transform.position = new Vector3(genes[genePos], 0, genes[genePos + 1]);
            genePos = genePos + 2;
        }
    }

}
