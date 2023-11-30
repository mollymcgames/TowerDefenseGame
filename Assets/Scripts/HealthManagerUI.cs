using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
// using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthManagerUI : MonoBehaviour
{
    public TextMeshProUGUI healthText; //The text that displays the health
    private int currentHearts; //How many hearts you have currently
    private int maxHearts = 5; //How many hearts you have in total

    private MoneyCounter moneyCounter;
    private AudioManager audioManager; //reference to the audio manager that plays the bg music
    void Start()
    {
        currentHearts = maxHearts; //Set the current health to the max health
        UpdateHealthText(); //Update the health text on start
        moneyCounter = FindFirstObjectByType<MoneyCounter>();
        audioManager = FindFirstObjectByType<AudioManager>();
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
            Debug.Log("Game Over");
            moneyCounter.GameOver(); //reset the money counter by clearing the player prefs
            audioManager.StopBackgroundMusic(); //stop the background music
            SceneManager.LoadScene("GameOver"); //Load the game over scene
        }
    }

    //Update the health text
    void UpdateHealthText()
    {
        healthText.text = currentHearts.ToString(); //Update the health text
    }
}
