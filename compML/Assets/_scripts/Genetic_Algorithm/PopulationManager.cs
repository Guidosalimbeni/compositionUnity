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

    private GameObject parent1; 
    private GameObject parent2;

    private int generation = 0;
    private int counterForPopulation = 0;
    private int CounterOffsprings = 0;
    private bool AICreatesInitialPopulationTurn = true; //
    private bool GenerateNewPopulatoinOffsprings_trigger = false;

    private List<GameObject> population;
    private GameObject offspring;

    private List<GameObject> sortedList; // public for debugging

    private bool sortNewGeneration = true;
    //private SendToDatabase sendtodatabase;
    private OpenCVManager openCVmanager;
    private GameVisualManager gameManagerNotOpenCV;
    private InferenceNNfomDATABASE inferenceNNfomDATABASE;
    private InferenceCompositionML inferenceCompositionML;
    private InferenceScoreFeatures inferenceScoreFeatures;
    private InferenceFinalOut inferenceFinalOut;

    private CalculateCollisionDistanceVisualUnity calculateCollisionDistanceVisualUnity;

    private void Awake()
    {
        //sendtodatabase = FindObjectOfType<SendToDatabase>();
    }

    private void Start()
    {
        openCVmanager = FindObjectOfType<OpenCVManager>();
        gameManagerNotOpenCV = FindObjectOfType<GameVisualManager>();
        calculateCollisionDistanceVisualUnity = FindObjectOfType<CalculateCollisionDistanceVisualUnity>();
        inferenceNNfomDATABASE = FindObjectOfType<InferenceNNfomDATABASE>();
        inferenceCompositionML = FindObjectOfType<InferenceCompositionML>();
        inferenceScoreFeatures = FindObjectOfType<InferenceScoreFeatures>();
        inferenceFinalOut = FindObjectOfType<InferenceFinalOut>();
    }

    public void TriggerAIfromButton()
    {
        triggerAI = true;
        calculateCollisionDistanceVisualUnity.drawRenderedLinesDebug = false;
        sortedList = new List<GameObject>();
        population = new List<GameObject>();
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

        counterForPopulation++; // COUNTER

        yield return new WaitForSeconds(secondToWaitForPopGeneration);

        openCVmanager.CallForOpenCVCalculationUpdates(); // to update the score pixels balance of opencv..
        gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores();

        inferenceScoreFeatures.CallTOCalculateFeaturesAllScores();
        inferenceNNfomDATABASE.CallTOCalculateNNFrontTopcore();
        inferenceCompositionML.CallTOCalculateMobileNetScore();
        inferenceFinalOut.CallTOCalculateFinalOutScore();

        IndividualCompositionSet.GetComponent<BrainGA>().CalculateTotalScore(); /// this is where the score is updated
        population.Add(IndividualCompositionSet);


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
                //Debug.Log(bestScore);
                population.Clear(); // this is the list that contains all the prefabs individual in the scene

            }
            sortNewGeneration = false;
        }

        StartCoroutine(Breed()); 
    }

    private IEnumerator Breed()
    {
        // it can happen that father is equal to mother,,, too bad...
        int InternalIndex_parent1 = Random.Range((int)(sortedList.Count / 1.6), sortedList.Count);
        int InternalIndex_parent2 = Random.Range((int)(sortedList.Count / 1.6), sortedList.Count);
        parent1 = sortedList[InternalIndex_parent1];
        parent2 = sortedList[InternalIndex_parent2];

        GameObject offspring = Instantiate(Individual, this.transform.position, this.transform.rotation);

        BrainGA b = offspring.GetComponent<BrainGA>();
        if (Random.Range(0, 2) == 1) //mutate 1 in 2
        {
            b.InitForBreed();
            b.dna.Mutate();
            b.MoveComposition(); // this is where the moved 
        }
        else
        {
            b.InitForBreed();
            b.dna.Combine(parent1.GetComponent<BrainGA>().dna, parent2.GetComponent<BrainGA>().dna);
            b.MoveComposition(); // this is where the moved 
        }

        yield return new WaitForSeconds(secondToWaitForPopGeneration);

        openCVmanager.CallForOpenCVCalculationUpdates(); // to update the score pixels balance of opencv..
        gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores();
        inferenceScoreFeatures.CallTOCalculateFeaturesAllScores();
        inferenceNNfomDATABASE.CallTOCalculateNNFrontTopcore();
        inferenceCompositionML.CallTOCalculateMobileNetScore();
        inferenceFinalOut.CallTOCalculateFinalOutScore();

        offspring.GetComponent<BrainGA>().CalculateTotalScore(); // are updated and score

        population.Add(offspring);
        GenerateNewPopulatoinOffsprings_trigger = true;

        CounterOffsprings++;                                           // counter
        
        if (CounterOffsprings == populationSize)
        {
            DestroyPopulationStack();

            sortNewGeneration = true;
            CounterOffsprings = 0;
            generation++;                                             // counter
        }

        if (generation == NumberOfGeneration)
        {
            GenerateNewPopulatoinOffsprings_trigger = false;
            triggerAI = false;
            calculateCollisionDistanceVisualUnity.drawRenderedLinesDebug = true;  //////
            AICreatesInitialPopulationTurn = true;


            sortedList = population.OrderBy(o => o.GetComponent<BrainGA>().TotalScore).ToList();
            List<float> genes = new List<float>();
            genes.Clear();
            genes = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().genes;
            sortedList[sortedList.Count - 1].GetComponent<BrainGA>().MoveCompositionOfBestFitAfterAIfinishedIsTurn(genes);

            bestScore = sortedList[sortedList.Count - 1].GetComponent<BrainGA>().TotalScore; // for debugging only

            generation = 0;
            CounterOffsprings = 0;
            counterForPopulation = 0;

            DESTROYPopulationOBJECTinSCENE();

            sortNewGeneration = true;

            yield return new WaitForSeconds(secondToWaitForPopGeneration);

            openCVmanager.CallForOpenCVCalculationUpdates(); // to update the score pixels balance of opencv..
            gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores();
            inferenceScoreFeatures.CallTOCalculateFeaturesAllScores();
            inferenceNNfomDATABASE.CallTOCalculateNNFrontTopcore();
            inferenceCompositionML.CallTOCalculateMobileNetScore();
            inferenceFinalOut.CallTOCalculateFinalOutScore();
        }
    }

    private void DESTROYPopulationOBJECTinSCENE()
    {
        BrainGA[] individualsInScene = FindObjectsOfType<BrainGA>();

        for (int i = 0; i < individualsInScene.Length; i++)
        {
            Destroy(individualsInScene[i].gameObject);
        }
    }


    private void DestroyPopulationStack()
    {
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i].gameObject);
        }

    }
}

