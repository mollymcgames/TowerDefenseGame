using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyHealthManager : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;
    private MoneyCounter moneyCounter;

    private GoapAgent ga;


    private WaveController waveController;


    public Slider slider; 
    void Start()
    {
        ga = gameObject.GetComponent<CleverEnemy>();
        currentHealth = maxHealth;
        UpdateHealthbar();
        moneyCounter = FindFirstObjectByType<MoneyCounter>();

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        waveController = FindFirstObjectByType<WaveController>();
        waveController.RemoveEnemy(gameObject);
        moneyCounter.AddMoney(1);  //Add money when the enemy dies      
        Destroy(gameObject); //Destroy the enemy game object
    }

    void UpdateHealthbar()
    {
        slider.value = currentHealth; //Update the value of the healthbar
    }

}
