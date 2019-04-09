using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// read the DNA and tells what to do to the character


public class Brain : MonoBehaviour
{
    public GameObject elemComp_a;
    public GameObject elemComp_b;
    public GameObject elemComp_c;
    public SendToDatabase sendtodatabase;

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

    public void Init()
	{
		//initialise DNA
        //0 obj1 x
        //1 obj1 z
        //2 obj2 x
        //3 obj2 z
        //4 obj3 x
        //5 obj3 z
        dna = new DNA(DNALength,1f,-1f); 
        //TotalScore = 0;
       
        //TODO obviously fix this bit!!!!
        GameObject[] elementsInComp = GameObject.FindGameObjectsWithTag("ItemsInComposition");
        elemComp_a = elementsInComp[0];
        elemComp_b = elementsInComp[1];
        elemComp_c = elementsInComp[2];

        sendtodatabase = FindObjectOfType<SendToDatabase>();

        MoveComposition(); ////// to fix!!!!!!!!!! or extract total score... 


    }

    public void InitForBreed()
    {
        dna = new DNA(DNALength, 1f, -1f);

        GameObject[] elementsInComp = GameObject.FindGameObjectsWithTag("ItemsInComposition");
        elemComp_a = elementsInComp[0];
        elemComp_b = elementsInComp[1];
        elemComp_c = elementsInComp[2];

        sendtodatabase = FindObjectOfType<SendToDatabase>();

    }

    // to fix scriptable variable???
    public void CalculateTotalScore()
    {
        if (sendtodatabase != null)
        {
            TotalScore = sendtodatabase.scorePixelsBalance + sendtodatabase.scoreUnityVisual + sendtodatabase.scoreBoundsBalance;
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
        elemComp_a.GetComponent<ClickToMoveBySelection>().SetLastDestinationPositionCorrectlyFromAI(g0, 0.0f, g1);
        elemComp_b.GetComponent<ClickToMoveBySelection>().SetLastDestinationPositionCorrectlyFromAI(g2, 0.0f, g3);
        elemComp_c.GetComponent<ClickToMoveBySelection>().SetLastDestinationPositionCorrectlyFromAI(g4, 0.0f, g5);
    }

}
