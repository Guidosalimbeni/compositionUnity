using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// read the DNA and tells what to do to the character


public class BrainGA : MonoBehaviour
{

    public GameObject elemComp_a;
    public GameObject elemComp_b;
    public GameObject elemComp_c;
    public SendToDatabase sendtodatabase;
    public BrainNN_CompML brainNN_compML;
    public OpenCVManager openCVmanager;

    public int DNALength = 6;
	public float TotalScore;  //
	public DNA dna;

    public float scorePixelsBalance;
    public float scoreUnityVisual;
    public float scoreBoundsBalance;

    //public float g0; ///
    //public float g1;
    //public float g2;
    //public float g3;
    //public float g4;
    //public float g5;

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
       
        ////TODO obviously fix this bit!!!!
        //GameObject[] elementsInComp = GameObject.FindGameObjectsWithTag("ItemsInComposition");
        //elemComp_a = elementsInComp[0];
        //elemComp_b = elementsInComp[1];
        //elemComp_c = elementsInComp[2];

        sendtodatabase = FindObjectOfType<SendToDatabase>();

        MoveComposition(); //////
        //UpdateGenes();
    }

    public void InitForBreed()
    {
        dna = new DNA(DNALength, population_manager.MaxValues_x, population_manager.MinValues_z);

        //GameObject[] elementsInComp = GameObject.FindGameObjectsWithTag("ItemsInComposition");
        //elemComp_a = elementsInComp[0];
        //elemComp_b = elementsInComp[1];
        //elemComp_c = elementsInComp[2];

        sendtodatabase = FindObjectOfType<SendToDatabase>();

    }

    // to fix scriptable variable???
    public void CalculateTotalScore()
    {
        if (sendtodatabase != null && brainNN_compML != null) 
        {


            openCVmanager.CallTOCalculateAreaLeftRight(); // to update the score pixels balance of opencv..
            // calculate score comes after movement so genes are updated
            float scoreNN = 0;

            genes.Clear();
            genes = dna.GetGenesList();

            scoreNN = brainNN_compML.PredictFromNN(genes[0], genes[1], genes[2], genes[3], genes[4], genes[5]);  // might want to MULTIPLY
            

            TotalScore = sendtodatabase.scorePixelsBalance + sendtodatabase.scoreUnityVisual + sendtodatabase.scoreBoundsBalance + scoreNN;
            scorePixelsBalance = sendtodatabase.scorePixelsBalance;
            scoreUnityVisual = sendtodatabase.scoreUnityVisual;
            scoreBoundsBalance = sendtodatabase.scoreBoundsBalance;
            
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

        //elemComp_a.transform.position = new Vector3(dna.GetGene(0), 0, dna.GetGene(1));
        //elemComp_b.transform.position = new Vector3(dna.GetGene(2), 0, dna.GetGene(3));
        //elemComp_c.transform.position = new Vector3(dna.GetGene(4), 0, dna.GetGene(5));
        

    }


    //private void UpdateGenes()
    //{
    //    g0 = dna.GetGene(0); 
    //    g1 = dna.GetGene(1);
    //    g2 = dna.GetGene(2);
    //    g3 = dna.GetGene(3);
    //    g4 = dna.GetGene(4);
    //    g5 = dna.GetGene(5);

    //}

    //public void MoveCompositionOfBestFitAfterAIfinishedIsTurn(float g0, float g1, float g2, float g3, float g4, float g5)
    //{

    //    // pass genes

    //    elemComp_a.transform.position = new Vector3(g0, 0, g1);
    //    elemComp_b.transform.position = new Vector3(g2, 0, g3);
    //    elemComp_c.transform.position = new Vector3(g4, 0, g5);

    //    //TODO to delete if lean touch works well

    //    //ClickToMoveBySelection ClickToMoveBySelection_A = elemComp_a.GetComponent<ClickToMoveBySelection>();
    //    //ClickToMoveBySelection ClickToMoveBySelection_B = elemComp_b.GetComponent<ClickToMoveBySelection>();
    //    //ClickToMoveBySelection ClickToMoveBySelection_C = elemComp_c.GetComponent<ClickToMoveBySelection>();

    //    //if (ClickToMoveBySelection_A != null && ClickToMoveBySelection_B != null && ClickToMoveBySelection_C != null )
    //    //{
    //    //    ClickToMoveBySelection_A.SetLastDestinationPositionCorrectlyFromAI(g0, 0.0f, g1);
    //    //    ClickToMoveBySelection_B.SetLastDestinationPositionCorrectlyFromAI(g2, 0.0f, g3);
    //    //    ClickToMoveBySelection_C.SetLastDestinationPositionCorrectlyFromAI(g4, 0.0f, g5);
    //    //}
        
    //}

    public void MoveCompositionOfBestFitAfterAIfinishedIsTurn(List<float> genes)
    {

        // pass genes
        int genePos = 0;
        foreach (var elementComp in population_manager.elementsCompositions)
        {

            elementComp.transform.position = new Vector3(genes[genePos], 0, genes[genePos + 1]);

            
            genePos = genePos + 2;
        }


        //elemComp_a.transform.position = new Vector3(g0, 0, g1);
        //elemComp_b.transform.position = new Vector3(g2, 0, g3);
        //elemComp_c.transform.position = new Vector3(g4, 0, g5);


    }

}
