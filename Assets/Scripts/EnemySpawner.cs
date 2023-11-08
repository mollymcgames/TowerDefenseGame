using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; //The enemy prefab to be spawned

    public Vector3[] spawnPositions; //The positions at which the enemy can be spawned
    public float spawnInterval = 5.0f; //The interval between enemy spawns
    public bool isSpawning = true; //Check if the enemy is spawning this is true by default

    public int amountOfEnemiesSpawnedPerWave = 1; //The amount of enemies spawned per wave
    private int enemiesPerWaveIncrement = 1; //The amount of enemies to be incremented per wave
    private int currentEnemiesPerWave = 1; //The current amount of enemies per wave

    public List<GameObject> activeEnemies;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("INITIAL Active enemies IN SPAWNER:"+activeEnemies.Count);        
        StartCoroutine(SpawnEnemy()); //Start spawning enemies
    }

    public IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(0.5f); //Wait for half a second to let it load in @TODO might have to fix this later so no delay is present but enemy was invisible otherwise
        System.Random random = new System.Random(); //Create a new random number generator for the enemies to be spawned at random positions
        int totalEnemiesSpawned = 1; //The total amount of enemies spawned
        while (isSpawning) //Check if the enemy is spawning
        {
            for (int i = 0; i < currentEnemiesPerWave; i++) //Loop through the amount of enemies to be spawned per wave
            {
                int spawnIndex = random.Next(spawnPositions.Length); //Get a random index for the spawn position
                Vector3 spawnPosition = spawnPositions[spawnIndex]; //Get the spawn position at the random index
                GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); //Spawn the enemy at the specified position
                newEnemy.SetActive(true); //Set the enemy to active
                activeEnemies.Add(newEnemy);
                Debug.Log("SPAWNER  Active enemies: "+activeEnemies.Count);                        
                totalEnemiesSpawned++; //Increment the total amount of enemies spawned
            }
            currentEnemiesPerWave += enemiesPerWaveIncrement; //Increment the current amount of enemies per wave
            yield return new WaitForSeconds(spawnInterval); //Wait for the spawn interval
        }
    }

    //Function to stop spawning enemies
    public void StopSpawning()
    {
        isSpawning = false; //Set isSpawning to false
    }
}
