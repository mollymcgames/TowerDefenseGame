using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    public int maxHealth;
    [SerializeField] private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; //Set the current health to the max health
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
    }
}
