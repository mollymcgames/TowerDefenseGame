using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgrade : MonoBehaviour
{

    public Button upgradeButton;
    public GameObject upgradedPrefab; // the upgraded tower prefab

    [SerializeField] private int uniqueCost; //Set the unique cost of the tower in the inspector
    public TextMeshProUGUI costText;
    // Start is called before the first frame update
    void Start()
    {
        
        upgradeButton.onClick.AddListener(UpgradeTower);

        UpdateCostText();
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

    void UpdateCostText()
    {
        costText.text = uniqueCost.ToString();
    }
}
