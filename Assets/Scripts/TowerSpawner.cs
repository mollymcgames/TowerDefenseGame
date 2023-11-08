using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerSpawner : MonoBehaviour
{
    public GameObject towerPrefab; // Reference to the tower prefab
    public int maxTowers = 3; // The maximum number of towers the player can place
    private int currentTowers = 0; // The current number of towers the player has placed

    public TextMeshProUGUI towerArcherCountText; // Reference to the tower archer count text

    // // Start is called before the first frame update
    void Start()
    {
        UpdateTowerCountText();
    }

    // Update is called once per frame
    void Update()
    {
        //Only allow the player to add towers if the number of towers is less than the max number of towers
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0) && (currentTowers < maxTowers))
        {
            // Debug.Log("current towers is" + currentTowers);
            // Debug.Log("max towers is" + maxTowers);
            // Get the mouse position
            Vector3 mousePosition = Input.mousePosition;
            // Convert the mouse position from screen space to world space
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            // Set the z position to 0
            worldPosition.z = 0;
            // Instantiate the tower prefab at the mouse position
            Instantiate(towerPrefab, worldPosition, Quaternion.identity);

            //Increment the current number of towers
            currentTowers++;

            //Update the tower count text in the canvas UI
            UpdateTowerCountText();
        }
        
    }

    private void UpdateTowerCountText()
    {
        //Update the tower count text in the canvas UI
        int remainingTowers = maxTowers - currentTowers;
        towerArcherCountText.text = remainingTowers.ToString();
    }
}



//TO DO - you can jsut add many towers by clicking on the screen but this isnt ideal we should have a cap 
//on the number of towers we can have and we should have a way to select the tower we want to place