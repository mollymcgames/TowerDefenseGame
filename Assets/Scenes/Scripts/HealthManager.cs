using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{

    public int maxHealth;
    [SerializeField] public int currentHealth;

    public Slider slider; //reference to the healthbar

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; //Set the current health to the max health
        UpdateHealthbar();

    }

    //Method to handle taking damage 
    public void HurtPlayer(int damage)
    {
        currentHealth -= damage; //Subtract the damage from the current health
        if(currentHealth <= 0)
        {
            gameObject.SetActive(false); //Deactivate the player
            Debug.Log("Player died!");
        }
        UpdateHealthbar();
    }

        void UpdateHealthbar()
    {
        slider.value = currentHealth; //Update the value of the healthbar
    }
}
