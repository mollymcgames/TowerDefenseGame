using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TowerArcher : MonoBehaviour
{
    public GameObject arrowPrefab; // Add the arrow prefab here
    public Transform firePoint; // An empty game object as the fire point to assign in the inspector
    public Transform targetWaypoint; // The target waypoint the enemy moves towards

    [SerializeField] private float fireRate = 1.0f; // The rate of fire
    [SerializeField] private float arrowSpeed = 5f; // The speed of the arrow

    private float timeUntilFire; // Timer to keep track of when to fire
    private List<GameObject> activeArrows = new List<GameObject>(); // List to hold active arrows
    private bool shouldFire = true; // A flag to determine if the tower should fire

    // Start is called before the first frame update
    void Start()
    {
        timeUntilFire = 0f; // Set the timer to the fire rate
    }

    // Update is called once per frame
    void Update()
    {
        // Countdown the timer until it's time to fire the next arrow
        timeUntilFire -= Time.deltaTime;

        // If the timer reaches zero or below, fire an arrow and reset the timer
        if (timeUntilFire <= 0 && shouldFire)
        {
            FireArrow();
            timeUntilFire = fireRate;
        }

        // Create a separate list to hold arrows that are no longer active
        List<GameObject> inactiveArrows = new List<GameObject>();

        // If an arrow is present, move it towards the enemy's position
        foreach (GameObject arrow in activeArrows)
        {
            if (arrow != null)
            {
                GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
                if (enemy != null)
                {
                    NavMeshAgent enemyAgent = enemy.GetComponent<NavMeshAgent>();
                    if (enemyAgent != null && targetWaypoint != null)
                    {
                        if (Vector3.Distance(enemyAgent.transform.position, targetWaypoint.position) <= 1.2f)
                        {
                            // Stop firing when the enemy reaches the waypoint
                            shouldFire = false;
                            inactiveArrows.Add(arrow);
                            Destroy(arrow);
                            Debug.Log("Enemy is very close to the waypoint. Stopping fire.");
                        }
                        else if (Vector3.Distance(arrow.transform.position, enemyAgent.transform.position) <= 0.5f)
                        {
                            // Handle the arrow hitting the enemy
                            ArrowScript arrowScript = arrow.GetComponent<ArrowScript>();
                            if (arrowScript != null)
                            {
                                arrowScript.DealDamage();
                            }
                            inactiveArrows.Add(arrow);
                            Destroy(arrow);
                            Debug.Log("Enemy is not close to the waypoint and in this loop.");
                            Debug.Log("The arrows should be stopping fire as the enemy should have reached the taget but its not so were checking it has reached the target." + Vector3.Distance(enemyAgent.transform.position, targetWaypoint.position) + " " + enemyAgent.transform.position + " " + targetWaypoint.position);


                        }
                        else if (shouldFire)
                        {
                            arrow.transform.position = Vector3.MoveTowards(arrow.transform.position, enemyAgent.transform.position, arrowSpeed * Time.deltaTime);
                            Debug.Log("Enemy is not close to the waypoint.");                        
                        }
                    }
                }
            }
        }

        // Remove the inactive arrows from the active arrows list
        foreach (GameObject arrow in inactiveArrows)
        {
            activeArrows.Remove(arrow);
        }
    }

    private void FireArrow()
    {
        // Find the closest object with the tag "Enemy"
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null)
        {
            NavMeshAgent enemyAgent = enemy.GetComponent<NavMeshAgent>();

            // Instantiate an arrow prefab at the fire point position and rotation
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);

            // Calculate the direction towards the enemy agent
            Vector3 direction = (enemyAgent.transform.position - firePoint.position).normalized;

            // Get the arrow component
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

            // Set the initial velocity of the arrow towards the enemy
            if (rb != null)
            {
                rb.velocity = direction * arrowSpeed;
                activeArrows.Add(arrow); // Add the arrow to the list of active arrows
            }
        }
    }
}
