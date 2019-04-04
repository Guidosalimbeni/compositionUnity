﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour {

    public bool triggerAI = false;
	public GameObject Individual;
    public int populationSize = 10;
    public int NumberOfGeneration = 4;
    public GameObject elemComp_a;
    public GameObject elemComp_b;
    public GameObject elemComp_c;

    private int generation = 0;
    private int counterForINITIALPopulation = 0;
    private bool AICreatesInitialPopulationTurn = true; //
    private float secondToWaitForPopGeneration = 0.5f;
    private List<GameObject> population = new List<GameObject>();
    private bool BreedNewPopulation_Trigger = false;
    private bool breedingHalf_1 = true;
    private bool breedingHalf_2 = false;
    private int indexOfHoldSortedListForBreeding = 0;
    private bool SetNewIndexToHandlePairingInSortedList = true;
    private List<GameObject> sortedListHold;
    private GameObject offspring;

    private void FixedUpdate()
    {
        if (triggerAI == true) // this is ok since the new trigger is on new type of learned scores from users..
        {
            elemComp_a.GetComponent<ClickToMoveBySelection>().AIisPlaying = true;
            elemComp_b.GetComponent<ClickToMoveBySelection>().AIisPlaying = true;
            elemComp_c.GetComponent<ClickToMoveBySelection>().AIisPlaying = true;

            if (AICreatesInitialPopulationTurn == true)
            {
                StartCoroutine(CreatePopulation());
            }

            if (BreedNewPopulation_Trigger == true)
            {
                BreedNewPopulation();
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
        AICreatesInitialPopulationTurn = false;
        GameObject IndividualCompositionSet = Instantiate(Individual, this.transform.position, this.transform.rotation);
        Debug.Log("First");
        IndividualCompositionSet.GetComponent<Brain>().Init();
        population.Add(IndividualCompositionSet);
        yield return new WaitForSeconds(secondToWaitForPopGeneration);
        
        if (counterForINITIALPopulation < populationSize - 1) // handle to counter for trigger...
        {
            BreedNewPopulation_Trigger = false;
            AICreatesInitialPopulationTurn = true; // KEEP running Start pop until hanlde says STOP
            counterForINITIALPopulation++;
        }
        else
        {
            AICreatesInitialPopulationTurn = false; // STOP FIRST INITIAL POPULATION
            counterForINITIALPopulation = 0;
            BreedNewPopulation_Trigger = true; // START NEW BREEDING to produce offsprings
            
        }
    }

    private void BreedNewPopulation()
    {
        //BreedNewPopulation_Trigger = false; //////// make it when generation.. play with this to cancel now..
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain>().TotalScore).ToList();

        if (population.Count == populationSize  )
        {
            population.Clear(); // if population count is greater than clean ???
            if (generation < NumberOfGeneration)
            {
                Debug.Log(generation + "     ------------------------------------------------");
                breedingHalf_1 = true; /// bread again change NAME into START FROM FIRST HALF and ON...
            }

        }
        

        Debug.Log(population.Count + " cooooounttt - -- - - -");
        

        if (SetNewIndexToHandlePairingInSortedList == true)
        {
            indexOfHoldSortedListForBreeding = (int)(sortedList.Count / 2.0f); 
            sortedListHold = sortedList;
            SetNewIndexToHandlePairingInSortedList = false;
            Debug.Log(sortedListHold.Count + "hold of sorted list count... this means is not repeated");
        }

        

        BreedNewPopulationOnSortedList(sortedListHold);
        
    }

    private void BreedNewPopulationOnSortedList(List<GameObject> sortedListHold)
    {
        if (breedingHalf_1 == true)
        {
            StartCoroutine(BreedHalfSortedList_PreviousNext(sortedListHold, indexOfHoldSortedListForBreeding));
            indexOfHoldSortedListForBreeding++;
        }

        if (breedingHalf_2 == true)
        {
            StartCoroutine(BreedHalfSortedList_NextPrevious(sortedListHold, indexOfHoldSortedListForBreeding));

            if (indexOfHoldSortedListForBreeding + 1 >= sortedListHold.Count)
            {
                
                for (int i = 0; i < sortedListHold.Count; i++)
                {
                    Destroy(sortedListHold[i]);
                }

                SetNewIndexToHandlePairingInSortedList = true; /////////////// ///////////////
                generation++;
                
                
            }

            indexOfHoldSortedListForBreeding++;
        }
    }

    private IEnumerator BreedHalfSortedList_PreviousNext(List<GameObject> sortedList, int InternalIndex)
    {
        breedingHalf_1 = false;
        offspring = Breed(sortedList[InternalIndex - 1], sortedList[InternalIndex]); 
        population.Add(offspring);

        yield return new WaitForSeconds(secondToWaitForPopGeneration);
        
        if (InternalIndex >= sortedList.Count - 1)
        {
            breedingHalf_1 = false;
            breedingHalf_2 = true; 
            indexOfHoldSortedListForBreeding = (int)(sortedList.Count / 2.0f);
        }
        else
        {
            if (breedingHalf_2 == false)  /// ??????????
            {
                breedingHalf_1 = true;
            }
        }
    }

    private IEnumerator BreedHalfSortedList_NextPrevious(List<GameObject> sortedList, int InternalIndex)
    {
        breedingHalf_2 = false;
        offspring = Breed(sortedList[InternalIndex], sortedList[InternalIndex - 1]); 
        population.Add(offspring);

        yield return new WaitForSeconds(secondToWaitForPopGeneration);

        if (InternalIndex >= sortedList.Count - 1)
        {
            breedingHalf_2 = false;
            Debug.Log(population.Count + " cooooounttt - -- - - -  999999999999999");
            //BreedNewPopulation_Trigger = false; /// stop breeding first generation of offsprings +++ put it after GENERATIOn Count full
        }
        else
        {
            breedingHalf_2 = true;
        }
    }

    GameObject Breed(GameObject parent1, GameObject parent2)
    {
       GameObject offspring = Instantiate(Individual, this.transform.position, this.transform.rotation);
       Brain b = offspring.GetComponent<Brain>();
       if (Random.Range(0, 20) == 1) //mutate 1 in 20
       {
           b.Init();
           b.dna.Mutate();
       }
       else
       {
           b.Init();
           b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
       }

       return offspring;
    }
}

