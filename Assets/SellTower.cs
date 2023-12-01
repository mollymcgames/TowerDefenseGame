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
    // Start is called before the first frame update
    void Start()
    {

        //Hide the buttons on start 
        sellButton.gameObject.SetActive(false);


        sellButton.onClick.AddListener(SellThisTower);
        
    }

    void OnMouseOver()
    {
        //show the upgrade button when the mouse is over the tower
        if(!buttonVisible)
        {
            sellButton.gameObject.SetActive(true);
            buttonVisible = true;
        }
    }

    void OnMouseExit()
    {
        //hide the upgrade button when the mouse is no longer over the tower
        if(buttonVisible)
        {
            sellButton.gameObject.SetActive(false);
            buttonVisible = false;
        }
    }


    void SellThisTower()
    {
        //Add the sell value to the money counter when selling the tower
        MoneyCounter moneyCounter = FindFirstObjectByType<MoneyCounter>();
        if(moneyCounter != null)
        {
            moneyCounter.AddMoney(sellValue);
            //Destroy the current tower
            Destroy(gameObject);
            Debug.Log("Tower sold!");
        }
    }

}
