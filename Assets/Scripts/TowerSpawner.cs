using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    public GameObject towerPrefab; // Reference to the tower prefab

    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {

        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Get the mouse position
            Vector3 mousePosition = Input.mousePosition;
            // Convert the mouse position from screen space to world space
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            // Set the z position to 0
            worldPosition.z = 0;
            // Instantiate the tower prefab at the mouse position
            Instantiate(towerPrefab, worldPosition, Quaternion.identity);
        }
        
    }
}
