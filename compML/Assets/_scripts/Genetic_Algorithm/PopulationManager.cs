using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour {

    public bool triggerAI = false;
	public GameObject Individual;
    public int populationSize = 10;
    public int NumberOfGeneration = 4;

    private int counterForPopulation = 0;
    private bool AICreatesPopulationTurn = true; //
    private float secondToWaitForPopGeneration = 0.5f;
    private List<GameObject> population = new List<GameObject>();

    private float trialTime = 5;
    private bool startBreeding = false;
    [SerializeField]
    private GameObject elemComp_a;
    [SerializeField]
    private GameObject elemComp_b;
    [SerializeField]
    private GameObject elemComp_c;

    private int generation = 0;


    private void FixedUpdate()
    {
        if (triggerAI == true) 
        {
            // move out of fixed upadte when set the trigger properly TODO
            elemComp_a.GetComponent<ClickToMoveBySelection>().AIisPlaying = true;
            elemComp_b.GetComponent<ClickToMoveBySelection>().AIisPlaying = true;
            elemComp_c.GetComponent<ClickToMoveBySelection>().AIisPlaying = true;

            if (AICreatesPopulationTurn == true)
            {
                StartCoroutine(CreatePopulation());
            }

            if (startBreeding == true)
            {
                BreedNewPopulation();
                //StartCoroutine(BreedNewPopulation()); // TODO
                startBreeding = false;
            }
        }

        if (triggerAI == false)
        {
            elemComp_a.GetComponent<ClickToMoveBySelection>().AIisPlaying = false;
            elemComp_b.GetComponent<ClickToMoveBySelection>().AIisPlaying = false;
            elemComp_c.GetComponent<ClickToMoveBySelection>().AIisPlaying = false;
        }
        
        

    }

    private IEnumerator CreatePopulation()
    {
        AICreatesPopulationTurn = false;
        GameObject IndividualCompositionSet = Instantiate(Individual, this.transform.position, this.transform.rotation);
        IndividualCompositionSet.GetComponent<Brain>().Init();
        population.Add(IndividualCompositionSet);
        yield return new WaitForSeconds(secondToWaitForPopGeneration);
        
        if (counterForPopulation < populationSize)
        {
            startBreeding = false;
            AICreatesPopulationTurn = true;
            counterForPopulation++;
        }
        else
        {
            AICreatesPopulationTurn = false;
            counterForPopulation = 0;
            startBreeding = true;
        }
    }


    void BreedNewPopulation()
    {
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain>().TotalScore).ToList();

        //Debug.Log(sortedList[0]);

        population.Clear();
        
        for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }
        //destroy all parents and previous population
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
        Debug.Log(generation);
        
    }

    
    GameObject Breed(GameObject parent1, GameObject parent2)
	{

        GameObject offspring = Instantiate(Individual, this.transform.position, this.transform.rotation);

		Brain b = offspring.GetComponent<Brain>();
		if(Random.Range(0,100) == 1) //mutate 1 in 100
		{
			b.Init();
			b.dna.Mutate();
		}
		else
		{ 
			b.Init();
			b.dna.Combine(parent1.GetComponent<Brain>().dna,parent2.GetComponent<Brain>().dna);
		}

        Debug.Log("offspring generated");
		return offspring;

    }



}

