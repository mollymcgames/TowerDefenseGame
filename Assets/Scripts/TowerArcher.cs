using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerArcher : MonoBehaviour
{

    public GameObject arrowPrefab; //add the arrow prefab here
    public Transform firePoint; //an empty game object as the firepoint to assingn in the inspector
    public float fireRate = 2.0f; //the rate of fire

    private float timeUnitlFire; //Timer to keep track of when to fire
    // Start is called before the first frame update
    void Start()
    {
        timeUnitlFire = 0f; //set the timer to the fire rate //might need to be 0
        
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
        
    }
    private void FireArrow()
    {
        //Instantiate an arrow prefab at the firepoint position and rotation
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);

        //Get the arrow component
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        //Set the initial velocity of the arrow in the -x direction but will need to change later
        if (rb != null)
        {
            rb.velocity = new Vector2(-5f, 0f); // Adjust the speed here
        }
    }
}
