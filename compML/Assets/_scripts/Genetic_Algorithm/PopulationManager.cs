using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour {

    public bool triggerAI = false;
	public GameObject Individual;
    public int populationSize = 10;
    public int NumberOfGeneration = 4;
    public float secondToWaitForPopGeneration = 0.1f;
    public float bestScore; // for debugging only
    public List<float> debugingList; // for debugging only

    private int generation = 0;
    private int counterForPopulation = 0;
    private int CounterOffsprings = 0;
    private bool AICreatesInitialPopulationTurn = true; //
    private bool GenerateNewPopulatoinOffsprings_trigger = false;
    private List<GameObject> population = new List<GameObject>();
    private GameObject offspring;

    private List<GameObject> sortedList;
    private List<GameObject> populationToDelete = new List<GameObject>();
    private bool sortNewGeneration = true;
    private SendToDatabase sendtodatabase;
    private OpenCVManager openCVmanager;
    private GameVisualManager gameManagerNotOpenCV;
    private InferenceNNfomDATABASE inferenceNNfomDATABASE;

    private CalculateCollisionDistanceVisualUnity calculateCollisionDistanceVisualUnity;

    private void Awake()
    {
        sendtodatabase = FindObjectOfType<SendToDatabase>();
        sortedList = new List<GameObject>();
    }

    private void Start()
    {
        openCVmanager = FindObjectOfType<OpenCVManager>();
        gameManagerNotOpenCV = FindObjectOfType<GameVisualManager>();
        calculateCollisionDistanceVisualUnity = FindObjectOfType<CalculateCollisionDistanceVisualUnity>();
        inferenceNNfomDATABASE = FindObjectOfType<InferenceNNfomDATABASE>();
    }


    public void TriggerAIfromButton()
    {
        triggerAI = true;
        calculateCollisionDistanceVisualUnity.drawRenderedLinesDebug = false;
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

        IndividualCompositionSet.GetComponent<BrainGA>().Init(); // init initial 
        BrainGA b = IndividualCompositionSet.GetComponent<BrainGA>();
        b.MoveComposition();
        counterForPopulation++;
        yield return new WaitForSeconds(secondToWaitForPopGeneration);

        // to need another yield?

        openCVmanager.CallForOpenCVCalculationUpdates(); // to update the score pixels balance of opencv..
        gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores();
        inferenceNNfomDATABASE.CallTOCalculateNNFrontTopcore();


        IndividualCompositionSet.GetComponent<BrainGA>().CalculateTotalScore(); /// this is where the score is updated
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

            if (population.Count == populationSize)
            {
                bestScore = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().TotalScore; // for debugging only

                population.Clear(); // this is the list not the objects
            }
            sortNewGeneration = false;
        }
        StartCoroutine(Breed()); 
    }

    private IEnumerator Breed()
    {
       int InternalIndex_parent1 = Random.Range((int)(sortedList.Count / 2), sortedList.Count );
       int InternalIndex_parent2 = Random.Range((int)(sortedList.Count / 2), sortedList.Count );
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

        openCVmanager.CallForOpenCVCalculationUpdates(); // to update the score pixels balance of opencv..
        gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores();
        inferenceNNfomDATABASE.CallTOCalculateNNFrontTopcore();


        // to need another yield?



        offspring.GetComponent<BrainGA>().CalculateTotalScore(); // this is where the moved are updated and score
        population.Add(offspring);
        populationToDelete.Add(offspring);
        GenerateNewPopulatoinOffsprings_trigger = true;
        CounterOffsprings++; // counter
        
        if (CounterOffsprings == populationSize)
        {
            sortNewGeneration = true;
            CounterOffsprings = 0;
            generation++; // counter

        }

        if (generation == NumberOfGeneration)
        {
            bestScore = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().TotalScore; // for debugging only

            GenerateNewPopulatoinOffsprings_trigger = false;
            triggerAI = false;
            calculateCollisionDistanceVisualUnity.drawRenderedLinesDebug = true;  //////
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
            float scoreLawOfLever = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().scoreLawOfLeverIndividual;
            float scoreNNTopFront = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().scoreNNFrontTopIndividual;

            sendtodatabase.PostDataFromAI(scorePixelsBalance, scoreUnityVisual, scoreBoundsBalance, genes, scoreLawOfLever, scoreNNTopFront); //

            for (int i = 0; i < populationToDelete.Count; i++)
            {
                Destroy(populationToDelete[i]);
            }
            sortNewGeneration = true;
            population = new List<GameObject>();
            populationToDelete = new List<GameObject>();
            sortedList = new List<GameObject>();

            yield return new WaitForSeconds(secondToWaitForPopGeneration);

            openCVmanager.CallForOpenCVCalculationUpdates(); // to update the score pixels balance of opencv..
            gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores();
            inferenceNNfomDATABASE.CallTOCalculateNNFrontTopcore();

        }
    }
}

