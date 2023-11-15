using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Diagnostics;



//have more than one tower with text for this tower updating individually
[System.Serializable]
public class TowerInfo
{
    public GameObject towerPrefab;
    public TextMeshProUGUI towerCountText;


    //Individual counts for each tower prefab 
    public int maxTowersPerTower;
    public int currentTowers;
}

public class TowerSpawner : MonoBehaviour
{

    // public List<GameObject> towerPrefabs; // List of all the towers in the scene
    public List<TowerInfo> towerInfos; // List of all the towers in the scene
    public int maxTowers = 3; // The maximum number of towers the player can place
    private int currentTowers = 0; // The current number of towers the player has placed

    public TextMeshProUGUI towerArcherCountText; // Reference to the tower archer count text


    // public Transform[] allowedPositions;   //Define the specific positions where the towers can be added

    public Vector3[] allowedPositions; //The positions at which the enemy can be spawned

    public int currentTowerIndex = 0; //The index of the currently selected tower in the towerPrefabs list


    // // Start is called before the first frame update
    void Start()
    {
        // Set the maximum towers for each tower type
        towerInfos[0].maxTowersPerTower = 5;   // Assuming element 0 is for "towerarcher"
        towerInfos[1].maxTowersPerTower = 3;   // Assuming element 1 is for "redmoon tower"
        UpdateTowerCountText();
    }

    // Update is called once per frame
    void Update()
    {
        //Only allow the player to add towers if the number of towers is less than the max number of towers
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0) && (towerInfos[currentTowerIndex].currentTowers < towerInfos[currentTowerIndex].maxTowersPerTower))
        {
            // Debug.Log("current towers is" + currentTowers);
            // Debug.Log("max towers is" + maxTowers);
            Vector3 mousePosition = Input.mousePosition; // Get the mouse position
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition); // Convert the mouse position from screen space to world space
            worldPosition.z = 0; // Set the z position to 0

            //Check if the clicked position is within the allowed positions
            foreach (Vector3 position in allowedPositions)
            {
                //calculate the distance between the clicked position and the allowed position
                if (Vector3.Distance(worldPosition, position) < 0.5f)
                {
                    //if the distance is less than 0.5f then the clicked position is within the allowed position
                    //so we can place the tower

                    //Instantiate the tower prefab at the mouse position
                    Instantiate(towerInfos[currentTowerIndex].towerPrefab, worldPosition, Quaternion.identity);
                    towerInfos[currentTowerIndex].currentTowers++;

                    //Increment the current number of towers
                    // currentTowers++;

                    //Update the tower count text in the canvas UI
                    UpdateTowerCountText();
                    break;

                }
            }
        }

        // Check if the player wants to switch to the next tower type
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchTower();
        }
        
    }

    private void UpdateTowerCountText()
    {
        // //Update the tower count text in the canvas UI
        // int remainingTowers = maxTowers - currentTowers;
        // towerArcherCountText.text = remainingTowers.ToString();

        foreach (var towerInfo in towerInfos)
        {
            int remainingTowers = towerInfo.maxTowersPerTower - towerInfo.currentTowers;
            towerInfo.towerCountText.text = remainingTowers.ToString();
        }
    }

    private void SwitchTower()
    {
        //Switch to the next tower type in the list
        currentTowerIndex = (currentTowerIndex + 1) % towerInfos.Count;
        // currentTowerIndex++;
        // if (currentTowerIndex >= towerPrefabs.Count)
        // {
        //     currentTowerIndex = 0;
        // }
    }

}



//TO DO - you can jsut add many towers by clicking on the screen but this isnt ideal we should have a cap 
//on the number of towers we can have and we should have a way to select the tower we want to place