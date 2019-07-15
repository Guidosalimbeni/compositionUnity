using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour {

    public float MaxValues_x = 2.0f;
    public float MinValues_z = -2.0f;

    public bool triggerAI = false;
	public GameObject Individual;
    public int populationSize = 10;
    public int NumberOfGeneration = 4;
    public float secondToWaitForPopGeneration = 0.1f;
    public GameObject elemComp_a;
    public GameObject elemComp_b;
    public GameObject elemComp_c;

    private float g0;
    private float g1;
    private float g2;
    private float g3;
    private float g4;
    private float g5;
    private float bestScore;

    private int generation = 0;
    private int counterForPopulation = 0;
    private int CounterOffsprings = 0;
    private bool AICreatesInitialPopulationTurn = true; //
    private bool GenerateNewPopulatoinOffsprings_trigger = false;
    private List<GameObject> population = new List<GameObject>();
    private GameObject offspring;
    List<GameObject> sortedList = new List<GameObject>();
    private List<GameObject> populationToDelete = new List<GameObject>();
    private bool sortNewGeneration = true;

    private SendToDatabase sendtodatabase;

    private void Awake()
    {
        sendtodatabase = FindObjectOfType<SendToDatabase>();
    }

    private void Update()
    {
        if (triggerAI == true) 
        {
            //elemComp_a.GetComponent<ClickToMoveBySelection>().AIisPlaying = true;
            //elemComp_b.GetComponent<ClickToMoveBySelection>().AIisPlaying = true;
            //elemComp_c.GetComponent<ClickToMoveBySelection>().AIisPlaying = true;

            if (AICreatesInitialPopulationTurn == true)
            {
                StartCoroutine(CreatePopulation());
            }

            if (GenerateNewPopulatoinOffsprings_trigger == true)
            {
                if (generation < NumberOfGeneration) ///
                {
                    GenerateNewPopulationOffsprings();
                }
            }
        }

        if (triggerAI == false)
        {
            //elemComp_a.GetComponent<ClickToMoveBySelection>().AIisPlaying = false;
            //elemComp_b.GetComponent<ClickToMoveBySelection>().AIisPlaying = false;
            //elemComp_c.GetComponent<ClickToMoveBySelection>().AIisPlaying = false;
        }
    }

    private IEnumerator CreatePopulation()
    {
        AICreatesInitialPopulationTurn = false;
        GameObject IndividualCompositionSet = Instantiate(Individual, this.transform.position, this.transform.rotation);
        IndividualCompositionSet.GetComponent<BrainGA>().Init();
        counterForPopulation++;
        yield return new WaitForSeconds(secondToWaitForPopGeneration);
        IndividualCompositionSet.GetComponent<BrainGA>().CalculateTotalScore(); /// this is where the moved are updated
        population.Add(IndividualCompositionSet);
        populationToDelete.Add(IndividualCompositionSet); 

        if (counterForPopulation < populationSize) 
        {
            AICreatesInitialPopulationTurn = true;
            GenerateNewPopulatoinOffsprings_trigger = false;
        }
        else
        {
            AICreatesInitialPopulationTurn = false; // STOP FIRST INITIAL POPULATION
            GenerateNewPopulatoinOffsprings_trigger = true; // START NEW BREEDING to produce offsprings
        }
    }

    private void GenerateNewPopulationOffsprings()
    {
        GenerateNewPopulatoinOffsprings_trigger = false; ///

        if (sortNewGeneration == true)
        {
            sortedList = population.OrderBy(o => o.GetComponent<BrainGA>().TotalScore).ToList();

            if (population.Count == 10)
            {
                bestScore = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().TotalScore;
                population.Clear(); // this is the list not the objects
            }
            sortNewGeneration = false;
        }
        StartCoroutine(Breed()); 
    }

    private IEnumerator Breed()
    {
        int InternalIndex_parent1 = Random.Range((int)(sortedList.Count / 2), sortedList.Count - 1);
        int InternalIndex_parent2 = Random.Range((int)(sortedList.Count / 2), sortedList.Count - 1);
        GameObject parent1 = sortedList[InternalIndex_parent1];
        GameObject parent2 = sortedList[InternalIndex_parent2];

       GameObject offspring = Instantiate(Individual, this.transform.position, this.transform.rotation);
       BrainGA b = offspring.GetComponent<BrainGA>();
       if (Random.Range(0, 10) == 1) //mutate 1 in 10
       {
           b.InitForBreed();
           b.dna.Mutate();
           b.MoveComposition(); 
        }
       else
       {
           b.InitForBreed();
           b.dna.Combine(parent1.GetComponent<BrainGA>().dna, parent2.GetComponent<BrainGA>().dna);
           b.MoveComposition(); 
        }

        yield return new WaitForSeconds(secondToWaitForPopGeneration);
        offspring.GetComponent<BrainGA>().CalculateTotalScore();
        population.Add(offspring);
        populationToDelete.Add(offspring);
        GenerateNewPopulatoinOffsprings_trigger = true;
        CounterOffsprings++;
        
        if (CounterOffsprings == populationSize)
        {
            sortNewGeneration = true;
            CounterOffsprings = 0;
            generation++; 
        }

        if (generation == NumberOfGeneration)
        {
            bestScore = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().TotalScore;
            GenerateNewPopulatoinOffsprings_trigger = false;
            triggerAI = false;
            AICreatesInitialPopulationTurn = true;
            g0 = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().g0;
            g1 = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().g1;
            g2 = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().g2;
            g3 = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().g3;
            g4 = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().g4;
            g5 = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().g5;
            sortedList[sortedList.Count - 1].GetComponent<BrainGA>().MoveCompositionOfBestFitAfterAIfinishedIsTurn(g0, g1, g2, g3, g4, g5);
            generation = 0;
            CounterOffsprings = 0;
            counterForPopulation = 0;

   
            float scorePixelsBalance = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().scorePixelsBalance;
            float scoreUnityVisual = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().scoreUnityVisual;
            float scoreBoundsBalance = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().scoreBoundsBalance;
            sendtodatabase.PostDataFromAI(scorePixelsBalance, scoreUnityVisual, scoreBoundsBalance, g0, g1, g2, g3, g4, g5); //


            for (int i = 0; i < populationToDelete.Count; i++)
            {
                Destroy(populationToDelete[i]);
            }
            sortNewGeneration = true;
            population = new List<GameObject>();
            populationToDelete = new List<GameObject>();
            sortedList = new List<GameObject>();
        }
    }
}

