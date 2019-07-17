using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// not a monodeveloper
public class DNA {

	private List<float> genes = new List<float>();
	private int DnaLength = 0;
	private float MaxValues = 0;
    private float MinValue = 0;

	public DNA(int lenght, float Maxv, float MinV)
	{
		DnaLength = lenght;
		MaxValues = Maxv;  // +2
        MinValue = MinV;  // -2
        // TODO define for each axis
        SetRandom();
	}

	public void SetRandom()
	{
		genes.Clear();
		for(int i = 0; i < DnaLength; i++)
		{
			genes.Add(Random.Range(MinValue, MaxValues));
		}
	}

	public void SetInt(int pos, int value)
	{
		genes[pos] = value;
	}

	public void Combine(DNA d1, DNA d2)
	{
		for(int i = 0; i < DnaLength; i++)
		{
			if(i < DnaLength/2.0)
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
	}

	public void Mutate()
	{
		genes[Random.Range(0,DnaLength)] = Random.Range(0, MaxValues);
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
