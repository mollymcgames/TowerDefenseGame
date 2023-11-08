using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; //The enemy prefab to be spawned

    public Vector3[] spawnPositions; //The positions at which the enemy can be spawned
    public float spawnInterval = 5.0f; //The interval between enemy spawns
    public bool isSpawning = true; //Check if the enemy is spawning this is true by default
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy()); //Start spawning enemies
    }
 
    public IEnumerator SpawnEnemy()
    {
        System.Random random = new System.Random(); //Create a new random number generator for the enemies to be spawned at random positions
        while (isSpawning) //Check if the enemy is spawning
        {
            yield return new WaitForSeconds(spawnInterval); //Wait for the spawn interval
            int spawnIndex = random.Next(spawnPositions.Length); //Get a random index for the spawn position
            Vector3 spawnPosition = spawnPositions[spawnIndex]; //Get the spawn position at the random index
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); //Spawn the enemy at the specified position
        }
    }
 
    //Function to stop spawning enemies
    public void StopSpawning()
    {
        isSpawning = false; //Set isSpawning to false
    }
}
 