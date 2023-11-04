using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    // public Slider slider; //reference to the healthbar

    void Start()
    {
        currentHealth = maxHealth;
        // UpdateHealthbar();

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
        Destroy(gameObject); //Destroy the enemy game object
    }

    // void UpdateHealthbar()
    // {
    //     slider.value = currentHealth; //Update the value of the healthbar
    // }

}
