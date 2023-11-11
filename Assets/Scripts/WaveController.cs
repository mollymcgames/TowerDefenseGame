using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveController : MonoBehaviour
{

    private int currentWave = 1; //The current wave number we assume starts from 1
    private int maxWaves = 4; //The maximum number of waves in the game in this level we assume 1 level so far
    public TextMeshProUGUI waveText; // Reference to the wave count text
    // public EnemySpawner enemySpawner; // Reference to the enemy spawner

    public List<EnemySpawner> enemySpawners; // Reference to the enemy spawners

    public List<GameObject> activeEnemies;


    // Start is called before the first frame update
    void Start()
    {
        activeEnemies = new List<GameObject>();
        Debug.Log("INITIAL Active enemies:" + activeEnemies.Count);
        // enemySpawner.activeEnemies = activeEnemies;
        UpdateWaveText(); //Update the wave count text in the canvas UI from the start
        StartCoroutine(SpawnWave()); //Start spawning enemies
    }

    public IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(0.5f); //Wait for half a second to let it load in @TODO might have to fix this later so no delay is present but enemy was invisible otherwise  
        while (currentWave < maxWaves) //Check if the current wave is less than the maximum number of waves
        {
            foreach (EnemySpawner enemySpawner in enemySpawners)
            {
                yield return new WaitForSeconds(5.0f); //Wait for 5 seconds
                enemySpawner.SpawnEnemy(); //Spawn an enemy
                Debug.Log("Active enemies:" + activeEnemies.Count);
            }

            currentWave++; //Increment the current wave
            UpdateWaveText(); //Update the wave count text in the canvas UI
        }
        if(currentWave == maxWaves)
        {                
            yield return new WaitForSeconds(2.0f); //Wait for bug
            Debug.Log("Reached max waves. stop spawning enemies");
            foreach (EnemySpawner enemySpawner in enemySpawners)
            {
                enemySpawner.StopSpawning(); //Stop spawning enemies
            }
            // enemySpawner.StopSpawning(); //Stop spawning enemies
            yield return new WaitForSeconds(10.0f); //Wait for 6 seconds
            if (activeEnemies.Count == 0)
            {
                Debug.Log("Won it all!");


                //Get the name of the active scene
                string activeSceneName = SceneManager.GetActiveScene().name;

                //Load different scenes based on the active scene name
                switch (activeSceneName)
                {
                    case "Test":  //really need to change the name of this scene to Level1
                        SceneManager.LoadScene("LevelTwo"); //might need to change this name too to Level2
                        break;
                    case "LevelTwo":
                        SceneManager.LoadScene("LevelThree");
                        break;
                    case "LevelThree":
                        SceneManager.LoadScene("Win");
                        break;     
                }
                // SceneManager.LoadScene("Win"); //might need to change this logic as it loads the next screen in the build index
            }
        }

        Debug.Log("Game appears to be over, and you seem to have survived! The final enemy count: " + activeEnemies.Count);
    }

    private void UpdateWaveText()
    {
        waveText.text = "Wave: " + currentWave + "/" + maxWaves; //Update the wave count text in the canvas UI
    }
}
