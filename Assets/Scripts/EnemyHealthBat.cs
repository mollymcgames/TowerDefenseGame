using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//@TODO: Detelte this later and use the other enemy health manager
public class EnemyHealthBat : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    // private WaveController waveController;


    public Slider slider; //reference to the healthbar
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthbar();

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // waveController = FindFirstObjectByType<WaveController>();
        // List<GameObject> activeEnemies = waveController.activeEnemies;
        // Debug.Log("A dude DIED, active enemies BEFORE processing:"+activeEnemies.Count);                    
        // activeEnemies.Remove(gameObject);
        // Debug.Log("A dude DIED, active enemies AFTER processing:"+activeEnemies.Count);        
        Destroy(gameObject); //Destroy the enemy game object
    }

    void UpdateHealthbar()
    {
        slider.value = currentHealth; //Update the value of the healthbar
    }

}
