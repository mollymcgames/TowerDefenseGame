using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; //The enemy prefab to be spawned
    public Vector3 spawnPosition; //The position at which the enemy is spawned
    public Transform[] spawnPoints; //The spawn points for the enemies
 
    public float spawnInterval = 5.0f; //The interval between enemy spawns
    public bool isSpawning = true; //Check if the enemy is spawning this is true by default
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy()); //Start spawning enemies
    }
 
    private IEnumerator SpawnEnemy()
    {
        while (isSpawning) //Check if the enemy is spawning
        {
            yield return new WaitForSeconds(spawnInterval); //Wait for the spawn interval
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); //Spawn the enemy at the specified position
        }
    }
 
    //Function to stop spawning enemies
    public void StopSpawning()
    {
        isSpawning = false; //Set isSpawning to false
    }
}
 