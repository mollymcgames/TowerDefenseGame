using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveController : MonoBehaviour
{

    private int currentWave = 1; //The current wave number we assume starts from 1
    private int maxWaves = 3; //The maximum number of waves in the game in this level we assume 1 level so far
    public TextMeshProUGUI waveText; // Reference to the wave count text
    public EnemySpawner enemySpawner; // Reference to the enemy spawner

    // Start is called before the first frame update
    void Start()
    {
        UpdateWaveText(); //Update the wave count text in the canvas UI from the start
        StartCoroutine(SpawnWave()); //Start spawning enemies
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpawnWave()
    {
        while (currentWave < maxWaves) //Check if the current wave is less than the maximum number of waves
        {
            yield return new WaitForSeconds(5.0f); //Wait for 5 seconds
            enemySpawner.SpawnEnemy(); //Spawn an enemy
            currentWave++; //Increment the current wave
            UpdateWaveText(); //Update the wave count text in the canvas UI
        }
    }

    private void UpdateWaveText()
    {
        waveText.text = "Wave: " + currentWave + "/" + maxWaves; //Update the wave count text in the canvas UI
        // waveText.text = currentWave.ToString(); //Update the wave count text in the canvas UI WILL CHANGE LATER
    }
}
