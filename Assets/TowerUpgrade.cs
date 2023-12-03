using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgrade : MonoBehaviour
{

    public Button upgradeButton;

    public Button sellButton; //Button to sell the tower
    public GameObject upgradedPrefab; // the upgraded tower prefab

    [SerializeField] private int uniqueCost; //Set the unique cost of the tower in the inspector

    [SerializeField] private int sellValue; //Set the sell value of the tower in the inspector
    public TextMeshProUGUI costText;

    private bool buttonVisible = false; 
    private TowerSpawner towerSpawner;
    private string towerType; // Added to store the tower type

    // Start is called before the first frame update
    void Start()
    {

        //Hide the buttons on start 
        upgradeButton.gameObject.SetActive(false);
        sellButton.gameObject.SetActive(false);


        upgradeButton.onClick.AddListener(UpgradeTower);
        sellButton.onClick.AddListener(SellTower);

        //Get the TowerSpawner script
        towerSpawner = FindFirstObjectByType<TowerSpawner>();
        if(towerSpawner == null)
        {
            Debug.LogError("No TowerSpawner script found!");
        }
        towerType = upgradedPrefab.name; // Assuming the upgradedPrefab has a unique name for each type

        
        UpdateCostText();
    }

    void OnMouseOver()
    {
        //show the upgrade button when the mouse is over the tower
        if(!buttonVisible)
        {
            upgradeButton.gameObject.SetActive(true);
            sellButton.gameObject.SetActive(true);
            buttonVisible = true;
        }
    }

    void OnMouseExit()
    {
        //hide the upgrade button when the mouse is no longer over the tower
        if(buttonVisible)
        {
            upgradeButton.gameObject.SetActive(false);
            sellButton.gameObject.SetActive(false);
            buttonVisible = false;
        }
    }

    void UpgradeTower()
    {
        //Subtrct the unique cost when upgrading the tower
        MoneyCounter moneyCounter = FindFirstObjectByType<MoneyCounter>();
        if(moneyCounter != null && moneyCounter.CanAfford(uniqueCost))
        {
            moneyCounter.SubtractMoney(uniqueCost);
            //Instantiate the upgraded tower prefab at the same position as the current tower
            Instantiate(upgradedPrefab, transform.position, transform.rotation);
            //Destroy the current tower
            Destroy(gameObject);
            Debug.Log("Tower upgraded!");            
        }
        else
        {
            Debug.Log("Can't afford to upgrade tower!");
        }
    }

    void SellTower()
    {
        //Add the sell value to the money counter when selling the tower
        MoneyCounter moneyCounter = FindFirstObjectByType<MoneyCounter>();
        if(moneyCounter != null)
        {
            moneyCounter.AddMoney(sellValue);

            //Get the position of the tower 
            Vector3 position = transform.position;

            //Get the tower spawner script and inform it about the tower before destroying it
            if (towerSpawner != null)
            {
                towerSpawner.RemoveTowerPosition(position, towerType);
            }
            //Destroy the current tower
            Destroy(gameObject);
            Debug.Log("Tower sold!");
        }
    }

    void UpdateCostText()
    {
        costText.text = "£" + uniqueCost.ToString();
    }
}
