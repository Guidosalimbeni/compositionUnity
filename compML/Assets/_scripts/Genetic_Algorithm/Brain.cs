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
	public float TotalScore;  
	public DNA dna;

    [SerializeField]
    private float oneGenesForDebug;

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
        TotalScore = 0;
        oneGenesForDebug = dna.GetGene(0); // debugging tool

        //TODO obviously fix this bit!!!!
        GameObject[] elementsInComp = GameObject.FindGameObjectsWithTag("ItemsInComposition");
        elemComp_a = elementsInComp[0];
        elemComp_b = elementsInComp[1];
        elemComp_c = elementsInComp[2];

        

        sendtodatabase = FindObjectOfType<SendToDatabase>();

        MoveComposition();


    }

    

    private void MoveComposition()
    {
        elemComp_a.transform.position = new Vector3(dna.GetGene(0), 0, dna.GetGene(1));
        elemComp_b.transform.position = new Vector3(dna.GetGene(2), 0, dna.GetGene(3));
        elemComp_c.transform.position = new Vector3(dna.GetGene(4), 0, dna.GetGene(5));
        if (sendtodatabase != null)
        {
            TotalScore = sendtodatabase.scorePixelsBalance + sendtodatabase.scoreUnityVisual + sendtodatabase.scoreBoundsBalance;
        }
        
    }
}
