using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveController : MonoBehaviour
{

    private int currentWave = 1; //The current wave number we assume starts from 1
    private int maxWaves = 5; //The maximum number of waves in the game in this level we assume 1 level so far
    public TextMeshProUGUI waveText; // Reference to the wave count text
    public EnemySpawner enemySpawner; // Reference to the enemy spawner

    // Start is called before the first frame update
    void Start()
    {
        UpdateWaveText(); //Update the wave count text in the canvas UI from the start
        StartCoroutine(SpawnWave()); //Start spawning enemies
    }

    public IEnumerator SpawnWave()
    {
        //wait for 2 seconds
        yield return new WaitForSeconds(0.5f); //Wait for half a second to let it load in @TODO might have to fix this later so no delay is present but enemy was invisible otherwise  
        while (currentWave < maxWaves) //Check if the current wave is less than the maximum number of waves
        {
            yield return new WaitForSeconds(5.0f); //Wait for 5 seconds
            enemySpawner.SpawnEnemy(); //Spawn an enemy
            currentWave++; //Increment the current wave
            UpdateWaveText(); //Update the wave count text in the canvas UI
        }
        if(currentWave == maxWaves)
        {
            Debug.Log("Reached max waves. stop spawning enemies");
            enemySpawner.StopSpawning(); //Stop spawning enemies
        }
    }

    private void UpdateWaveText()
    {
        waveText.text = "Wave: " + currentWave + "/" + maxWaves; //Update the wave count text in the canvas UI
        // waveText.text = currentWave.ToString(); //Update the wave count text in the canvas UI WILL CHANGE LATER
    }
}
