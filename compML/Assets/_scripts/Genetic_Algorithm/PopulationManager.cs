using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour {

    public bool triggerAI = false;

	public GameObject Individual;
	public int populationSize = 10;
	List<GameObject> population = new List<GameObject>();

    private int counterForPopulation = 0;
    private bool timeForSpawnIsPassed = true;
    private float secondToWaitForPopGeneration = 0.5f;

    private float trialTime = 5;
    private float elapsed = 0;
    [SerializeField]
    private GameObject elemComp_a;
    [SerializeField]
    private GameObject elemComp_b;
    [SerializeField]
    private GameObject elemComp_c;


    private void FixedUpdate()
    {
        if (triggerAI == true) 
        {
            // move out of fixed upadte when set the trigger properly TODO
            elemComp_a.GetComponent<ClickToMoveBySelection>().AIisPlaying = true;
            elemComp_b.GetComponent<ClickToMoveBySelection>().AIisPlaying = true;
            elemComp_c.GetComponent<ClickToMoveBySelection>().AIisPlaying = true;

            if (timeForSpawnIsPassed == true)
            {
                if (counterForPopulation < populationSize)
                {
                    StartCoroutine(CreatePopulation());
                    
                    counterForPopulation++;
                }
                else
                {
                    counterForPopulation = 0;
                }
            }

            elapsed += Time.deltaTime;

            if (elapsed > trialTime)
            {
                BreedNewPopulation();
                elapsed = 0;
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
        timeForSpawnIsPassed = false;
        GameObject IndividualCompositionSet = Instantiate(Individual, this.transform.position, this.transform.rotation);
        IndividualCompositionSet.GetComponent<Brain>().Init();
        population.Add(IndividualCompositionSet);
        yield return new WaitForSeconds(secondToWaitForPopGeneration);
        timeForSpawnIsPassed = true;


    }


    void BreedNewPopulation()
    {

        Debug.Log("breeeedddd");
        /*
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain>().timeAlive).ToList();

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
        */
    }



    /*
    GameObject Breed(GameObject parent1, GameObject parent2)
	{
        
		Vector3 startingPos = new Vector3(this.transform.position.x + Random.Range(-2,2),
												this.transform.position.y,
												this.transform.position.z + Random.Range(-2,2));
		GameObject offspring = Instantiate(botPrefab, startingPos, this.transform.rotation);
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
		return offspring;
    }
        
     */

}

