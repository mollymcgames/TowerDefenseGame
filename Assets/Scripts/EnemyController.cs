using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class EnemyController : MonoBehaviour
{
    private Animator myAnimator; //The animator component
    [SerializeField] private Transform targetWaypoint; //The waypoint the enemy is moving towards

    [SerializeField] private Transform intermediateWaypoint; //The waypoint the enemy is moving towards
    private HealthManagerUI healthManager; //The health manager script

    private WaveController waveController;

    private MoneyCounter moneyCounter; // Reference to the MoneyCounter script

    private bool hasStartedMoving = false; // Check if the enemy has started moving

    [SerializeField] private float speed = 0.5f ; //The speed at which the enemy moves

    EnemyController zmc;

    EnemyHealthManager ehm; 


    NavMeshAgent agent;

    private bool hasReachedIntermediateWaypoint = false; //Check if the enemy has reached the target
    private bool hasReachedWaypoint = false; //Check if the enemy has reached the target

    private bool hasBeenHealed = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //Get the NavMeshAgent component
        agent.updateRotation = false; //Stop the NavMeshAgent component from rotating the enemy
        agent.updateUpAxis = false; //Stop the NavMeshAgent component from rotating the enemy
        agent.speed = speed; //Set the speed of the NavMeshAgent component
        agent.stoppingDistance = 0.5f; //Set the stopping distance of the NavMeshAgent component

        myAnimator = GetComponent<Animator>(); //Get the animator component

        // Find the intermediate waypoint with teh "IntermediateWaypoint" tag
        GameObject intermediateWaypointObject = GameObject.FindGameObjectWithTag("IntermediateWaypoint");
        if (intermediateWaypointObject != null)
        {
            intermediateWaypoint = intermediateWaypointObject.transform;
        }
        else
        {
            Debug.Log("Cannot find 'IntermediateWaypoint' tag");
        }

        //find the first object with the "Target" tag
        GameObject targetWaypointObject = GameObject.FindGameObjectWithTag("Target");
        SetTarget(targetWaypointObject);

        // Get the HealthManagerUI component
        healthManager = FindFirstObjectByType<HealthManagerUI>();       

        moneyCounter = FindFirstObjectByType<MoneyCounter>(); // Find the MoneyCounter component 

        if (targetWaypoint == null || intermediateWaypoint == null)
        {
            Debug.Log("Waypoint is null");
        }

    }

    void Update()
    {

        //Move towards the intermediate waypoint first
        if (intermediateWaypoint != null && !hasReachedIntermediateWaypoint)
        {
            MoveToIntermediateWaypoint();
        }
        else
        {
            //Move towards the target waypoint after reaching the intermediate waypoint
            MoveToTargetWaypoint();
        }

    }

    void MoveToIntermediateWaypoint()
    {
        FollowWaypoint(intermediateWaypoint);
        if (!hasReachedIntermediateWaypoint && agent.remainingDistance <= agent.stoppingDistance) //Check if the enemy has reached the target
        {
            hasReachedIntermediateWaypoint = true;
        }
    }

    void MoveToTargetWaypoint()
    {
        FollowWaypoint(targetWaypoint);

        if ( agent.pathPending == true || agent.remainingDistance <= 0.0009f) 
        {
            Debug.Log("GOB ["+gameObject.GetInstanceID()+"] THINKING about where to go!");
        }
        else if (hasStartedMoving && agent.remainingDistance <= agent.stoppingDistance) //Check if the enemy has reached the targe    
        {

            if (targetWaypoint.tag == "HealthWaypoint" && hasBeenHealed == false)
            {
                ehm = gameObject.GetComponent<EnemyHealthAncientSkeleton>(); //NOT WORKING CANT USE ENEMY HEALTH MANAGER OR ENEMY HEALTH ANCIENT SKELETON
                zmc = gameObject.GetComponent<EnemyController>();
                
                int healAmount = (int)((float)ehm.currentHealth * 0.60f);
                if (healAmount > ehm.maxHealth)
                {
                    healAmount = ehm.maxHealth;
                }
                else if (healAmount <= 0)
                {
                    healAmount = 15;
                }

                // Debug.Log("GOB ["+gameObject.GetInstanceID()+"] HEALED for: "+healAmount);
                ehm.currentHealth += healAmount;
                zmc.UpdateSpeed(2);
                hasBeenHealed = true;
                // Debug.Log("From action, current HEALED health: "+ehm.currentHealth);

                //Update the healthbar for ancient skeleton
                FloatingHealthBarAncientSkeleton floatingHealthBar = GetComponent<FloatingHealthBarAncientSkeleton>();
                if (floatingHealthBar != null)
                {
                    floatingHealthBar.UpdateHealthBar();
                }
            }
            else if (targetWaypoint.tag != "HealthWaypoint")
            {
                hasReachedWaypoint = true;
                healthManager.ReduceHealth();

                waveController = FindFirstObjectByType<WaveController>();
                waveController.RemoveEnemy(gameObject);


                Destroy(gameObject); //Destroy the enemy game object
                moneyCounter.SubtractMoney(1); //Subtract money when the enemy reaches the target
                return;
            }  
        }
        else if (hasReachedWaypoint && agent.remainingDistance > agent.stoppingDistance) //Check if the enemy has reached the target
        {
            // Debug.Log("WAYPOINT REACHED GOB with ID: "+gameObject.GetInstanceID() + " and their remaining distance is: "+agent.remainingDistance);
            hasReachedWaypoint = false;
            hasBeenHealed = false;
        }

        hasStartedMoving = true;
    }

    public void UpdateSpeed(float newSpeed)
    {
        speed = newSpeed;
        agent.speed = speed;
    }    

    void FollowWaypoint(Transform waypoint)
    {
        if (waypoint != null)
        {
            agent.SetDestination(waypoint.position);
            myAnimator.SetBool("isMoving", true);
            myAnimator.SetFloat("moveX", (waypoint.position.x - transform.position.x));
            myAnimator.SetFloat("moveY", (waypoint.position.y - transform.position.y));

            if (agent.remainingDistance <= agent.stoppingDistance) //Check if the enemy has reached the target
            { 
                myAnimator.SetBool("isMoving", false); // Set isMoving to false to stop the animation
            }
        }
        else
        {
            myAnimator.SetBool("isMoving", false);
        }
    }

    public void SetTarget(GameObject inputTargetWaypoint)
    {
        // Set the targetWaypoint to the desired Vector3 position
        targetWaypoint = inputTargetWaypoint.transform;
    }

    public void SetTarget(string inputTargetWaypointTag)
    {
        SetTarget(GameObject.FindGameObjectWithTag(inputTargetWaypointTag));
    }


    public virtual void FollowTarget()
    {
        if (targetWaypoint != null)
        {
            agent.SetDestination(targetWaypoint.position);
            myAnimator.SetBool("isMoving", true);
            myAnimator.SetFloat("moveX", (targetWaypoint.position.x - transform.position.x));
            myAnimator.SetFloat("moveY", (targetWaypoint.position.y - transform.position.y));

            if (agent.remainingDistance <= agent.stoppingDistance) //Check if the enemy has reached the target
            { 
                myAnimator.SetBool("isMoving", false); // Set isMoving to false to stop the animation
            }
        }
        else
        {
            myAnimator.SetBool("isMoving", false);
        }
    }

}
