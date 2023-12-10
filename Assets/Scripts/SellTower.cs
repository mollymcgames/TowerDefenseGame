using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SellTower : MonoBehaviour
{

    public Button sellButton; //Button to sell the tower

    [SerializeField] private int sellValue; //Set the sell value of the tower in the inspector

    private bool buttonVisible = false; 

    [SerializeField] private AudioSource sellSoundEffect;

    private TowerSpawner towerSpawner;


    // Start is called before the first frame update
    void Start()
    {

        //Hide the buttons on start 
        sellButton.gameObject.SetActive(false);


        sellButton.onClick.AddListener(SellThisTower);

        //Get the TowerSpawner script
        towerSpawner = FindFirstObjectByType<TowerSpawner>();
        if (towerSpawner == null)
        {
            Debug.LogError("No TowerSpawner script found!");
        }        
        
    }

    void OnMouseOver()
    {
        //show the upgrade button when the mouse is over the tower
        if (!buttonVisible)
        {
            sellButton.gameObject.SetActive(true);
            buttonVisible = true;
        }
    }

    void OnMouseExit()
    {
        //hide the upgrade button when the mouse is no longer over the tower
        if (buttonVisible)
        {
            sellButton.gameObject.SetActive(false);
            buttonVisible = false;
        }
    }


    void SellThisTower()
    {
        StartCoroutine(PlaySellSoundAndDestroy());
    }


    IEnumerator PlaySellSoundAndDestroy()
    {
        sellSoundEffect.Play(); //Play the sell sound effect
        // yield return new WaitForSeconds(sellSoundEffect.clip.length);  // Wait for the sound to finish playing
        yield return new WaitForSeconds(0.5f);  // Wait for the sound to finish playing not the whole clip length
        //Add the sell value to the money counter when selling the tower    
        MoneyCounter moneyCounter = FindFirstObjectByType<MoneyCounter>();
        if (moneyCounter != null)
        {
            moneyCounter.AddMoney(sellValue);
            //Get the position of the tower         
            Vector3 position = transform.position;
            //Get the tower spawner script and inform it about the tower before destroying it
            if (towerSpawner != null)
            {
                towerSpawner.RemoveTowerPosition(towerSpawner.DeriveTowerPosition(position));
            }
            //Destroy the current tower
            Destroy(gameObject);
            Debug.Log("Tower sold!");
        }
    }
}

