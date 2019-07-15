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

    public List<GameObject> elementsCompositions;


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

    private void Start()
    {
        PopulateTheElementsOfCompositionInTheScene();
    }

    private void PopulateTheElementsOfCompositionInTheScene()
    {

        elementsCompositions = new List<GameObject>();

        TagMeElementOfComposition[] elementstags = FindObjectsOfType<TagMeElementOfComposition>();
        foreach (var elementtag in elementstags)
        {
            GameObject go = elementtag.gameObject;
            elementsCompositions.Add(go);
        }
    }

    private void Update()
    {
        if (triggerAI == true) 
        {
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

        //if (triggerAI == false)
        //{
        //    Debug.Log("trigger AI is off");
        //}
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

            List<float> genes = new List<float>();
            genes.Clear();
            genes = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().genes;

            sortedList[sortedList.Count - 1].GetComponent<BrainGA>().MoveCompositionOfBestFitAfterAIfinishedIsTurn(genes);
            generation = 0;
            CounterOffsprings = 0;
            counterForPopulation = 0;
   
            float scorePixelsBalance = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().scorePixelsBalanceIndividual;
            float scoreUnityVisual = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().scoreUnityVisualIndividual;
            float scoreBoundsBalance = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().scoreBoundsBalanceIndividual;

            sendtodatabase.PostDataFromAI(scorePixelsBalance, scoreUnityVisual, scoreBoundsBalance, genes); //

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

