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

    public float g0; ///
    public float g1;
    public float g2;
    public float g3;
    public float g4;
    public float g5;

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
       
        //TODO obviously fix this bit!!!!
        GameObject[] elementsInComp = GameObject.FindGameObjectsWithTag("ItemsInComposition");
        elemComp_a = elementsInComp[0];
        elemComp_b = elementsInComp[1];
        elemComp_c = elementsInComp[2];

        sendtodatabase = FindObjectOfType<SendToDatabase>();

        MoveComposition(); //////
    }

    public void InitForBreed()
    {
        dna = new DNA(DNALength, population_manager.MaxValues_x, population_manager.MinValues_z);

        GameObject[] elementsInComp = GameObject.FindGameObjectsWithTag("ItemsInComposition");
        elemComp_a = elementsInComp[0];
        elemComp_b = elementsInComp[1];
        elemComp_c = elementsInComp[2];

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
            /*
            g0 = dna.GetGene(0); ///
            g1 = dna.GetGene(1);
            g2 = dna.GetGene(2);
            g3 = dna.GetGene(3);
            g4 = dna.GetGene(4);
            g5 = dna.GetGene(5);
            */
            scoreNN = brainNN_compML.PredictFromNN(g0, g1, g2, g3, g4, g5);  // might want to MULTIPLY
            

            TotalScore = sendtodatabase.scorePixelsBalance + sendtodatabase.scoreUnityVisual + sendtodatabase.scoreBoundsBalance + scoreNN;
            scorePixelsBalance = sendtodatabase.scorePixelsBalance;
            scoreUnityVisual = sendtodatabase.scoreUnityVisual;
            scoreBoundsBalance = sendtodatabase.scoreBoundsBalance;
            
        }

    }

    public void MoveComposition()
    {
        elemComp_a.transform.position = new Vector3(dna.GetGene(0), 0, dna.GetGene(1));
        elemComp_b.transform.position = new Vector3(dna.GetGene(2), 0, dna.GetGene(3));
        elemComp_c.transform.position = new Vector3(dna.GetGene(4), 0, dna.GetGene(5));
        g0 = dna.GetGene(0); ///
        g1 = dna.GetGene(1);
        g2 = dna.GetGene(2);
        g3 = dna.GetGene(3);
        g4 = dna.GetGene(4);
        g5 = dna.GetGene(5);

    }

    public void MoveCompositionOfBestFitAfterAIfinishedIsTurn(float g0, float g1, float g2, float g3, float g4, float g5)
    {

        elemComp_a.transform.position = new Vector3(g0, 0, g1);
        elemComp_b.transform.position = new Vector3(g2, 0, g3);
        elemComp_c.transform.position = new Vector3(g4, 0, g5);

        //TODO to delete if lean touch works well

        //ClickToMoveBySelection ClickToMoveBySelection_A = elemComp_a.GetComponent<ClickToMoveBySelection>();
        //ClickToMoveBySelection ClickToMoveBySelection_B = elemComp_b.GetComponent<ClickToMoveBySelection>();
        //ClickToMoveBySelection ClickToMoveBySelection_C = elemComp_c.GetComponent<ClickToMoveBySelection>();

        //if (ClickToMoveBySelection_A != null && ClickToMoveBySelection_B != null && ClickToMoveBySelection_C != null )
        //{
        //    ClickToMoveBySelection_A.SetLastDestinationPositionCorrectlyFromAI(g0, 0.0f, g1);
        //    ClickToMoveBySelection_B.SetLastDestinationPositionCorrectlyFromAI(g2, 0.0f, g3);
        //    ClickToMoveBySelection_C.SetLastDestinationPositionCorrectlyFromAI(g4, 0.0f, g5);
        //}
        
    }

}
