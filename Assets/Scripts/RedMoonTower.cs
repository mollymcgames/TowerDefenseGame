using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RedMoonTower : MonoBehaviour
{
    public GameObject energyBallPrefab; // Add the energy ball prefab here
    public Transform firePoint; // An empty game object as the fire point to assign in the inspector
    public Transform targetWaypoint; // The target waypoint the enemy moves towards

    [SerializeField] private float fireRate = 1.0f; // The rate of fire
    [SerializeField] public float energyBallSpeed = 2f; // The speed of the arrow
    [SerializeField] private float firingRange = 5f; // The desired firing range

    private float timeUntilFire; // Timer to keep track of when to fire
    public List<GameObject> activeEnergyBalls = new List<GameObject>(); // List to hold active arrows
    private bool shouldFire = true; // A flag to determine if the tower should fire

    // Start is called before the first frame update
    void Start()
    {
        timeUntilFire = 0f; // Set the timer to the fire rate  
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Countdown the timer until it's time to fire the next arrow
        timeUntilFire -= Time.deltaTime;

        // If the timer reaches zero or below, fire an arrow and reset the timer
        if (timeUntilFire <= 0 && shouldFire)
        {
            FireEnergyBall();
            timeUntilFire = fireRate;
        }

        // Create a separate list to hold arrows that are no longer active
        List<GameObject> inactiveEnergyBalls = new List<GameObject>();

        // If an arrow is present, move it towards the enemy's position
        foreach (GameObject energyBall in activeEnergyBalls)
        {
            if (energyBall != null)
            {
                EnergyBallScript energyBallScript = energyBall.GetComponent<EnergyBallScript>();
                if (energyBallScript != null)
                {
                    energyBallScript.EnergyBallBehaviour(targetWaypoint, shouldFire, inactiveEnergyBalls, energyBallSpeed);
                }
            }
        }

        // Remove the inactive arrows from the active arrows list
        foreach (GameObject energyBall in inactiveEnergyBalls)
        {
            inactiveEnergyBalls.Remove(energyBall);
        }
    }

    private void FireEnergyBall()
    {
        // The Tower should find the closest enemy before firing an arrow. If there are multiple enemies on the scene it will prioritize the nearest one.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Get all the enemies on the scene
        GameObject closestEnemy = null; // The closest enemy
        float closestDistance = Mathf.Infinity; // The distance to the closest enemy
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position); // Calculate the distance between the tower and the enemy
                //we will also use this for the ifcheck 
                if (distance < closestDistance && distance <= firingRange) // If the distance is less than the closest distance
                {
                    closestDistance = distance; // Set the closest distance to the distance
                    closestEnemy = enemy; // Set the closest enemy to the enemy
                }
            }
        }
        if (closestEnemy != null)
        {
            NavMeshAgent enemyAgent = closestEnemy.GetComponent<NavMeshAgent>();
            ShootEnergyBall(enemyAgent);
        }
    }


    protected virtual void ShootEnergyBall(NavMeshAgent enemyAgent)
    {
            // Instantiate an arrow prefab at the fire point position and rotation
            GameObject energyBall = Instantiate(energyBallPrefab, firePoint.position, firePoint.rotation);

            // Calculate the direction towards the enemy agent
            Vector3 direction = (enemyAgent.transform.position - firePoint.position).normalized;

            energyBall.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction) * Quaternion.Euler(0, 0, 270);

            // Get the arrow component
            Rigidbody2D rb = energyBall.GetComponent<Rigidbody2D>();

            // Set the initial velocity of the arrow towards the enemy
            if (rb != null)
            {
                rb.velocity = direction * energyBallSpeed;
                activeEnergyBalls.Add(energyBall); // Add the arrow to the list of active arrows
            }
    }
}
