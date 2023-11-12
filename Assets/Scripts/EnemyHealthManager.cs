using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    private WaveController waveController;


    public Slider slider; //reference to the healthbar
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthbar();

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy ["+gameObject.GetInstanceID()+"] took damage");
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        waveController = FindFirstObjectByType<WaveController>();
        Debug.Log("A dude DIED, active enemies BEFORE processing:"+waveController.GetActiveEnemies().Count);                    
        waveController.RemoveEnemy(gameObject);
        Debug.Log("A dude DIED, active enemies AFTER processing:"+waveController.GetActiveEnemies().Count);        
        Destroy(gameObject); //Destroy the enemy game object
    }

    void UpdateHealthbar()
    {
        slider.value = currentHealth; //Update the value of the healthbar
    }

}
