using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BreederManager : MonoBehaviour
{
    //Singleton Class
    public static BreederManager BreederInstance;

    public GameObject BugPrefab;
    private Vector2 SpawnPoint;

    List<GameObject> BugsCollection = new List<GameObject>();
    private float time_left = 10; //In 10 seconds a new population is bred
    private int generation;

    public GameObject TreeCanvas;

    public Text generationTEXT;
    public Text time_leftTEXT;

    public float ReturnTime()
    {
        return time_left;
    }

    private void Awake()
    {
        BreederInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //For a population of 10
        for (int i = 0; i < 10; i++)
        {
            SpawnPoint = Camera.main.ScreenToWorldPoint(new Vector2( Random.Range(0, Screen.width), Random.Range(0, Screen.height)));

            //Debug.Log("Bug instantiated");
            GameObject bug = Instantiate(BugPrefab, SpawnPoint, Quaternion.identity);
            
            bug.GetComponent<DNAScript>().Red = Random.Range(0f, 1f);
            bug.GetComponent<DNAScript>().Green = Random.Range(0f, 1f);
            bug.GetComponent<DNAScript>().Blue = Random.Range(0f, 1f);
            bug.GetComponent<DNAScript>().SizeMultiplier = Random.Range(0.5f, 2f);

            BugsCollection.Add(bug);
        }
    }

    private void BreedNewPopulation()
    {
        //Don't override the Bugs Collection as the data there is needed so we make a new list
        List<GameObject> newBugsCollection = new List<GameObject>();

        //Timer starts from 10 -> 0, therefore order by descending, ie, bug with higer death time died earlier
        //Therefore, fittest individuals are at the bottom of the list
        List<GameObject> sortedBugsCollection = BugsCollection.OrderByDescending(order => order.GetComponent<DNAScript>().GetDeathTime()).ToList();

        BugsCollection.Clear();
        
        for(int i = (int) (sortedBugsCollection.Count/2f) - 1; i < sortedBugsCollection.Count - 1; i++)
        {
            GameObject offspring_firsthalf = Breed(sortedBugsCollection[i], sortedBugsCollection[i+1]); //This breeding gives half the original population
            GameObject offspring_secondhalf = Breed(sortedBugsCollection[i+1], sortedBugsCollection[i]); //This breeding gives another the original population

            BugsCollection.Add(offspring_firsthalf);
            BugsCollection.Add(offspring_secondhalf);
        }

        //Destroy the sorted Parent Bug list
        for(int i= 0; i < sortedBugsCollection.Count; i++)
        {
            Destroy(sortedBugsCollection[i]);
        }

        generation++;
    }

    #region Genetic ML

    private GameObject Breed(GameObject parent1, GameObject parent2)
    {

        DNAScript DNA_1 = parent1.GetComponent<DNAScript>();
        DNAScript DNA_2 = parent1.GetComponent<DNAScript>();

        SpawnPoint = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)));

        //Debug.Log("Bug instantiated");
        GameObject offspring = Instantiate(BugPrefab, SpawnPoint, Quaternion.identity);

        if (Random.Range(0, 100) > 10)
        {
            offspring.GetComponent<DNAScript>().Red = Random.Range(0, 10) < 5 ? DNA_1.Red : DNA_2.Red;
            offspring.GetComponent<DNAScript>().Green = Random.Range(0, 10) < 5 ? DNA_1.Green : DNA_2.Green;
            offspring.GetComponent<DNAScript>().Blue = Random.Range(0, 10) < 5 ? DNA_1.Blue : DNA_2.Blue;
            offspring.GetComponent<DNAScript>().SizeMultiplier = Random.Range(0, 10) < 5 ? DNA_1.SizeMultiplier : DNA_2.SizeMultiplier;
        }
        else
        {
            //Mutation with 10% chance
            offspring.GetComponent<DNAScript>().Red = Random.Range(0, 1f);
            offspring.GetComponent<DNAScript>().Green = Random.Range(0, 1f);
            offspring.GetComponent<DNAScript>().Blue = Random.Range(0, 1f);
            offspring.GetComponent<DNAScript>().SizeMultiplier = Random.Range(0.5f, 2f);
        }

        return offspring;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        time_left -= Time.deltaTime;

        if(time_left<=0)
        {
            BreedNewPopulation();
            generationTEXT.text = "Generation : " + generation.ToString();
            time_left = 10f;
        }

        time_leftTEXT.text = "Timer : " + time_left.ToString("0");
    }
}
