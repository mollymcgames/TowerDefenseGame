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

    public Button towerButton; // Add a reference to the tower button in the canvas

    [SerializeField] private int maxTowersPerTower; // The maximum number of towers the player can place
    public int MaxTowerPerTower {get { return maxTowersPerTower; }}
    
    // //Individual counts for each tower prefab 
    public int currentTowers;
}

public class TowerSpawner : MonoBehaviour
{
    public List<TowerInfo> towerInfos; // List of all the towers in the scene
    public int maxTowers = 3; // The maximum number of towers the player can place
    private int currentTowers = 0; // The current number of towers the player has placed

    public TextMeshProUGUI towerArcherCountText; // Reference to the tower archer count text


    public Vector3[] allowedPositions; //The positions at which the enemy can be spawned

    public int currentTowerIndex = 0; //The index of the currently selected tower in the towerPrefabs list

    private Dictionary<Vector3,string> towerPositions = new Dictionary<Vector3,string>();

    public MoneyCounter moneyCounter; // Reference to the money counter script

    void Start()
    {
        UpdateTowerCountText();

        //Add onClick listeners to the tower buttons
        foreach (var towerInfo in towerInfos)
        {
            towerInfo.towerButton.onClick.AddListener(() => SwitchTower(towerInfo)); // Add a listener to the tower button
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Only allow the player to add towers if the number of towers is less than the max number of towers
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0) && (towerInfos[currentTowerIndex].currentTowers < towerInfos[currentTowerIndex].MaxTowerPerTower))
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
                if (Vector3.Distance(worldPosition, position) < 0.5f && !towerPositions.ContainsKey(position))
                {
                    //If the distance is less than 0.5f then the clicked position is within the allowed position so we can place the tower
                    Instantiate(towerInfos[currentTowerIndex].towerPrefab, worldPosition, Quaternion.identity); //Instantiate the tower prefab at the mouse position
                    towerInfos[currentTowerIndex].currentTowers++;
                    towerPositions.Add(position, "occupied");

                    UpdateTowerCountText(); //Update the tower count text in the canvas UI

                    moneyCounter.SubtractMoney(1); //subtract one coin when the tower is placed
                    break;

                }
            }
        }
    }

    private void UpdateTowerCountText()
    {
        // //Update the tower count text in the canvas UI

        foreach (var towerInfo in towerInfos)
        {
            int remainingTowers = towerInfo.MaxTowerPerTower - towerInfo.currentTowers;
            towerInfo.towerCountText.text = remainingTowers.ToString();
        }
    }

    private void SwitchTower(TowerInfo selectedTower)
    {
        int newIndex = towerInfos.IndexOf(selectedTower);   //find the index of the selected tower in the towerInfos list
        //Switch to the selected tower type
        if (newIndex != -1)
        {
            currentTowerIndex = newIndex;
        }
    }

}



//TO DO - you can jsut add many towers by clicking on the screen but this isnt ideal we should have a cap 
//on the number of towers we can have and we should have a way to select the tower we want to place