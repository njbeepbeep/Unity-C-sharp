using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Easy way to sort lists

public class PopulationManager : MonoBehaviour
{

    // Link to the person prefab
    public GameObject personPrefab;
    public int populationSize = 10;

    // Keep track of the people we create using a list
    List<GameObject> population = new List<GameObject>();

    // Timer
    public static float elapsed = 0;

    // Code to begin breeding
    int trialTime = 10;

    // Keep track of the generation
    int generation = 1;

    // Sets a GUI to track some of the data we've created
    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {
        guiStyle.fontSize = 31;
        guiStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(7, 10, 100, 20), "Generation: " + generation, guiStyle);
        GUI.Label(new Rect(7, 65, 100, 20), "Trial Time: " + (int)elapsed, guiStyle);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Will loop 10 times to work out a random position on the screen
        for (int i = 0; i < populationSize; i++) 
        {
            // Screen parameters (Random.Range(x, y, z) note z = 0 since we are working in 2D
            // The Range is captured by moving the character on screen and taking the x, y, z location values - your screen's game view might be different!
            Vector3 pos = new Vector3(Random.Range(-7f, 7f), Random.Range(-3f, 3f), 0);

            // Creates the person and instantiates it, the Quaternion gives us a zero rotational value default
            GameObject go = Instantiate(personPrefab, pos, Quaternion.identity);

            // Set the person's DNA by giving them a random colour value each between 0 - 1 (1 = white, 0 = black)
            go.GetComponent<DNA>().r = Random.Range(0.0f, 1.0f);
            go.GetComponent<DNA>().g = Random.Range(0.0f, 1.0f);
            go.GetComponent<DNA>().b = Random.Range(0.0f, 1.0f);

            // New GameObject is added to the population list
            population.Add(go);
        }
    }

    
    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        // Random location for new instantiated persons
        Vector3 pos = new Vector3(Random.Range(-7f, 7f), Random.Range(-3f, 3f), 0);
        GameObject offspring = Instantiate(personPrefab, pos, Quaternion.identity);

        // Here we inherit DNA from parents
        DNA dna1 = parent1.GetComponent<DNA>();
        DNA dna2 = parent2.GetComponent<DNA>();

        
        if(Random.Range(0,1000) > 5)
        {
            // 50% of the time the offspring will get parent1 r,b,g channel, and parent2 r,b,g channel the other 50% of the time
            // This inheritence code is the fundamental code for any genetic algorithm whether in robotics, economics, optimisation tasks, etc. this is the foundation of that code
            offspring.GetComponent<DNA>().r = Random.Range(0, 10) < 5 ? dna1.r : dna2.r;
            offspring.GetComponent<DNA>().g = Random.Range(0, 10) < 5 ? dna1.g : dna2.g;
            offspring.GetComponent<DNA>().b = Random.Range(0, 10) < 5 ? dna1.b : dna2.b;
        }
        // Mutation setup
        else
        {
            offspring.GetComponent<DNA>().r = Random.Range(0.0f, 1.0f);
            offspring.GetComponent<DNA>().g = Random.Range(0.0f, 1.0f);
            offspring.GetComponent<DNA>().b = Random.Range(0.0f, 1.0f);
        }
        return offspring;
    }

    void BreedNewPopulation()
    {
        // Creates a list to hold our new population
        List<GameObject> newPopulation = new List<GameObject>();

        // Remove unfit units
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<DNA>().timeToDie).ToList();
        
        population.Clear();

        // Breed the upper half of the �bermensch - we loop the list starting half way down (/ 2.0f), thus we end up with the top of the list after it loops around
        // We use i and i + 1 to intially breed the person half-way along the list, then the next person up 1 from that - this continues up the list i++
        for (int i = (int) (sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            // QUESTION: Why are we breeding the list twice with the code below? Clue: think about the symmetry in the brackets!
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        // ANS: We breed i with i+1, and vice versa -  this will give us enough of a population size to continue with evolution and keep enough of a population size to avoid an exploding population issue
        // Destroy all parents and previous population
            for(int i = 0; i < sortedList.Count; i ++)
            {
                Destroy(sortedList[i]);
            }
            // Updates generation count
            generation++;
    }

    // Update is called once per frame
    void Update()
    {
        // Update our timer
        elapsed += Time.deltaTime;
        if(elapsed > trialTime)
        {
            // New method runs to find out which memebers of hte "tribe" are the most fittest and then we breed them together, reset and cycle through for 10 more seconds
            BreedNewPopulation();
            elapsed = 0;
        }
    }
}
