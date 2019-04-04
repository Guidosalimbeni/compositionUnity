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

    private bool BreedNewPopulation_Trigger = false;
    bool breedingHalf_1 = true;
    bool breedingHalf_2 = false;
    private int indexOfHoldSortedListForBreeding = 0;
    private bool SetNewIndexFirstHalf = true;
    
    private List<GameObject> sortedListHold;


    [SerializeField]
    private GameObject elemComp_a;
    [SerializeField]
    private GameObject elemComp_b;
    [SerializeField]
    private GameObject elemComp_c;

    private int generation = 0;

    private GameObject offspring;

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

            if (BreedNewPopulation_Trigger == true)
            {
                BreedNewPopulation();

                // might have to set here that we need to destroy the new generations?
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
        Debug.Log("First");
        IndividualCompositionSet.GetComponent<Brain>().Init();
        population.Add(IndividualCompositionSet);
        yield return new WaitForSeconds(secondToWaitForPopGeneration);
        
        if (counterForPopulation < populationSize - 1) 
        {
            BreedNewPopulation_Trigger = false;
            AICreatesPopulationTurn = true;
            counterForPopulation++;
        }
        else
        {
            AICreatesPopulationTurn = false; // STOP FIRST INITIAL POPULATION
            counterForPopulation = 0;
            BreedNewPopulation_Trigger = true; // START NEW BREEDING to produce offsprings

            /////////////// might need counter for created offsprings if not equal to original population count?

        }

    }

    void BreedNewPopulation()
    {
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain>().TotalScore).ToList(); 

        if (SetNewIndexFirstHalf == true)
        {
            indexOfHoldSortedListForBreeding = (int)(sortedList.Count / 2.0f); 
            sortedListHold = sortedList;
            population.Clear();  // might not want to clear before end of second half?
            SetNewIndexFirstHalf = false;
        }

        BreedNewPopulationOnSortedList(sortedListHold);

        generation++;
        //Debug.Log(generation); ////
    }

    private void BreedNewPopulationOnSortedList(List<GameObject> sortedListHold)
    {
        // we are still from FixedUpdate 

        if (breedingHalf_1 == true)
        {
            StartCoroutine(BreedHalfSortedList_PreviousNext(sortedListHold, indexOfHoldSortedListForBreeding));
            /*
            Debug.Log(indexOfHoldSortedListForBreeding + "    first half indexsortedlist");
            if (indexOfHoldSortedListForBreeding + 1 >= sortedListHold.Count)
            {
                SetNewIndexFirstHalf = true; /////////////// might not need this bit at all at this point....????
            }
            
            indexOfHoldSortedListForBreeding++;
            */
            indexOfHoldSortedListForBreeding++;
        }

        if (breedingHalf_2 == true)
        {
            StartCoroutine(BreedHalfSortedList_NextPrevious(sortedListHold, indexOfHoldSortedListForBreeding));

            Debug.Log(indexOfHoldSortedListForBreeding + "    sedong half indexsortedlist");
            if (indexOfHoldSortedListForBreeding + 1 >= sortedListHold.Count)
            {
                Debug.Log("didi I get here? +++++++++++++++++++++        "  + sortedListHold.Count); //////////
                BreedNewPopulation_Trigger = false; // this might have to place after the second half is done? or look for generation?
                // to extract?
                for (int i = 0; i < sortedListHold.Count; i++)
                {
                    Destroy(sortedListHold[i]);
                }
                SetNewIndexFirstHalf = true; ///////////////
                

            }

            indexOfHoldSortedListForBreeding++;

        }

    }

    // to implement BreedHalfSortedList_NextPrevious ///////////////// TODO
    // then when first generation is done ,,, need to set repeat of everything for each new generation count...at the top
    // of the loop after first population and somehow in the middle of trigger AI

    private IEnumerator BreedHalfSortedList_PreviousNext(List<GameObject> sortedList, int InternalIndex)
    {
        breedingHalf_1 = false;

        offspring = Breed(sortedList[InternalIndex - 1], sortedList[InternalIndex]); //
        Debug.Log("firstHalf    " + (sortedList.Count - 1) + "internal index " + InternalIndex);
        population.Add(offspring);
        yield return new WaitForSeconds(secondToWaitForPopGeneration);
        
        if (InternalIndex >= sortedList.Count - 1)
        {
            Debug.Log(" I AM HERE CONDITION MET             " + sortedListHold.Count);
            breedingHalf_1 = false;

            breedingHalf_2 = true; // becomes true when 1 is set to false

            indexOfHoldSortedListForBreeding = (int)(sortedList.Count / 2.0f);
        }
        else
        {
            if (breedingHalf_2 == false)
            {
                breedingHalf_1 = true;
            }
            // if breedingHalf_2 == false ... but if it is true breedingHalf_1 becomes false..
        }
    }

    private IEnumerator BreedHalfSortedList_NextPrevious(List<GameObject> sortedList, int InternalIndex)
    {
        breedingHalf_2 = false;

        offspring = Breed(sortedList[InternalIndex], sortedList[InternalIndex - 1]); //
        Debug.Log("SecondHalf");
        population.Add(offspring);
        yield return new WaitForSeconds(secondToWaitForPopGeneration);

        if (InternalIndex >= sortedList.Count - 1)
        {
            //Debug.Log(" I AM HERE CONDITION MET");
            breedingHalf_2 = false;
        }
        else
        {
            breedingHalf_2 = true;
            // if breedingHalf_2 == false ... but if it is true breedingHalf_1 becomes false..

        }


    }

    // if generation > x then BreedNewPopulation_Trigger to false...and true...

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

       //Debug.Log(offspring.GetComponent<Brain>().TotalScore); ///////////////////
       return offspring;

    }
}

