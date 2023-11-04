using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerArcher : MonoBehaviour
{
    public GameObject arrowPrefab; // Add the arrow prefab here
    public Transform firePoint; // An empty game object as the fire point to assign in the inspector
    public Transform targetWaypoint; // The target waypoint the enemy moves towards

    [SerializeField] private float fireRate = 0f; // The rate of fire
    [SerializeField] private float arrowSpeed = 5f; // The speed of the arrow

    private float timeUnitlFire; // Timer to keep track of when to fire
    private GameObject currentArrow; // The current arrow that is fired
    private bool ShouldFire = true; // A flag to determine if the tower should fire

    // Start is called before the first frame update
    void Start()
    {
        timeUnitlFire = 0f; // Set the timer to the fire rate
    }

    // Update is called once per frame
    void Update()
    {
        // Countdown the timer until it's time to fire the next arrow
        timeUnitlFire -= Time.deltaTime;

        // If the timer reaches zero or below, fire an arrow and reset the timer
        if (timeUnitlFire <= 0 && ShouldFire)
        {
            FireArrow();
            timeUnitlFire = fireRate;
        }

        // If an arrow is present, move it towards the enemy's position
        if (currentArrow != null)
        {
            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (enemy != null && Vector3.Distance(enemy.transform.position, targetWaypoint.position) <= 0.5f)
            {
                // Stop firing or handle this condition as needed
                ShouldFire = false;
                Debug.Log("Enemy is very close to the waypoint. Stopping fire.");
            }
            else if (enemy != null && ShouldFire)
            {
                currentArrow.transform.position = Vector3.MoveTowards(currentArrow.transform.position, enemy.transform.position, arrowSpeed * Time.deltaTime);
            }
        }
    }

    private void FireArrow()
    {
        // Find the closest object with the tag "Enemy"
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null)
        {
            // Instantiate an arrow prefab at the firepoint position and rotation
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);

            // Calculate the direction towards the enemy
            Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

            // Get the arrow component
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

            // Set the initial velocity of the arrow towards the enemy
            if (rb != null)
            {
                rb.velocity = direction * arrowSpeed;
                currentArrow = arrow; // Set the current arrow
            }
        }
    }
}
