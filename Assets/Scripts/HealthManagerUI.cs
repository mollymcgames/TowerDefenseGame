using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
// using UnityEngine.UI;
using TMPro;

public class HealthManagerUI : MonoBehaviour
{
    // Start is called before the first frame update

    public TextMeshProUGUI healthText; //The text that displays the health
    private int currentHearts; //How many hearts you have currently
    private int maxHearts = 5; //How many hearts you have in total
    void Start()
    {
        currentHearts = maxHearts; //Set the current health to the max health
        UpdateHealthText(); //Update the health text on start
    }

    //reduce the health by 1
    public void ReduceHealth()
    {
        if (currentHearts > 0) //Check if the current health is greater than 0
        {
            currentHearts--; //Reduce the current health by 1
            UpdateHealthText(); //Update the health text
        }
        if (currentHearts <= 0) //Check if the current health is less than or equal to 0
        {
            //Do something when the player dies
            Debug.Log("Game Over");
        }
    }

    //Update the health text
    void UpdateHealthText()
    {
        healthText.text = currentHearts.ToString(); //Update the health text
    }
}
