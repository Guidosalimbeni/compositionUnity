using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// not a monodeveloper
public class DNA {

	private List<float> genes = new List<float>();
	private int DnaLength = 0;
	private float MaxValues = 0;
    private float MinValue = 0;
    private int NUMMBEROFGENESMUTATED = 2;

    public DNA(int lenght, float Maxv, float MinV)
	{
		DnaLength = lenght;
		MaxValues = Maxv;  // + 1.5
        MinValue = MinV;  // - 1.5
        // TODO define for each axis
        SetRandom();
	}

	public void SetRandom()
	{
        // dna lenght is 4 eleme / x 3 genes = 12 for example
		genes.Clear();
		for(int i = 0; i < DnaLength; i++)
		{
            if ((i + 1)%3 == 0)
            {

                Debug.Log("got here");
                genes.Add(Random.Range(0, 360));
            }
            else
            {
                genes.Add(Random.Range(MinValue, MaxValues));
            }
			
		}
	}

	public void SetInt(int pos, int value)
	{
		genes[pos] = value;
	}

	public void Combine(DNA d1, DNA d2)
	{
        //string genseSeq = "";
        //foreach (float g in d1.genes)
        //{
        //    genseSeq += g + "  M  ";
        //}

        //foreach (float g in d2.genes)
        //{
        //    genseSeq += g + "  F  ";
        //}

        for (int i = 0; i < DnaLength; i++)
		{

            if (i <= DnaLength / 2.0) // hard coded <= since but to check if I will introduce rotation in the process ... so that one object is not mixed with rotation positoin etc..
            {
                float c = d1.genes[i];
                genes[i] = c;
            }
            else
            {
                float c = d2.genes[i];
                genes[i] = c;
            }

        }

        //foreach (float g in genes)
        //{
        //    genseSeq += g + "  Of  ";
        //}
        //Debug.Log(genseSeq);
    }

    public void Mutate() // mutation of a single genes...
	{
        for (int i = 0; i < NUMMBEROFGENESMUTATED; i++)
        {
            int mutationIndx = Random.Range(0, DnaLength);
            if ((mutationIndx + 1) % 3 == 0)
            {
                genes[mutationIndx] = Random.Range(0, 360);
            }
            else
            {
                genes[Random.Range(0, DnaLength)] = Random.Range(MinValue, MaxValues);
            }
        }

        
            
	}

	public float GetGene(int pos)
	{
		return genes[pos];
	}

    public List<float> GetGenesList()
    {
        return genes;
    }

}
