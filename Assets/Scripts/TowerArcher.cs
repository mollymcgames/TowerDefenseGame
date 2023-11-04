using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerArcher : MonoBehaviour
{

    public GameObject arrowPrefab; //add the arrow prefab here
    public Transform firePoint; //an empty game object as the firepoint to assingn in the inspector
    [SerializeField]
    private float fireRate = 2.0f; //the rate of fire
    [SerializeField]
    private float arrowSpeed = 5f; //the speed of the arrow

    private float timeUnitlFire; //Timer to keep track of when to fire
    private GameObject currentArrow; //the current arrow that is fired
    // Start is called before the first frame update
    void Start()
    {
        timeUnitlFire = 12f; //set the timer to the fire rate //might need to be 0
        
    }

    // Update is called once per frame
    void Update()
    {
        //countdown the timer until it's time to fire the next arrow
        timeUnitlFire -= Time.deltaTime;

        //If the timer reaches zero or below fire an arrow and reset the timer
        if(timeUnitlFire <= 0)
        {
            FireArrow();
            timeUnitlFire = fireRate; 
        }

        //If an arrow is present move it towards the enemy's positon 
        if(currentArrow != null)
        {
            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (enemy != null)
            {
                currentArrow.transform.position = Vector3.MoveTowards(currentArrow.transform.position, enemy.transform.position, arrowSpeed * Time.deltaTime);
            }
        }
        
    }
    private void FireArrow()
    {
        //Find the closest object with the tag "Enemy"
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null)
        {
            //Instantiate an arrow prefab at the firepoint position and rotation
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);

            //calculate the direction towards the enemy
            Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

            //Get the arrow component
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

            //Set the initial velocity of the arrow in the -x direction but will need to change later
            if (rb != null)
            {
                // rb.velocity = new Vector2(-5f, 0f); // Adjust the speed here
                rb.velocity = direction * arrowSpeed;
                currentArrow = arrow; // Set the current arrow                
            }
        }
    }
}
