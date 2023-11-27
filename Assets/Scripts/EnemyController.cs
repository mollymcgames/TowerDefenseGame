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

    private bool hasStartedMoving = false; // Check if the enemy has started moving

    [SerializeField] private float speed = 0.5f ; //The speed at which the enemy moves

    //ADD+
    EnemyController zmc;

    EnemyHealthManager ehm; //ADD


    NavMeshAgent agent;

    private bool hasReachedIntermediateWaypoint = false; //Check if the enemy has reached the target
    private bool hasReachedWaypoint = false; //Check if the enemy has reached the target

    private bool hasBeenHealed = false; //ADD+
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //Get the NavMeshAgent component
        agent.updateRotation = false; //Stop the NavMeshAgent component from rotating the enemy
        agent.updateUpAxis = false; //Stop the NavMeshAgent component from rotating the enemy
        agent.speed = speed; //Set the speed of the NavMeshAgent component
        agent.stoppingDistance = 0.5f; //Set the stopping distance of the NavMeshAgent component we might need to change this later but for now it works

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
        // if (targetWaypointObject != null)
        // {
        //     targetWaypoint = targetWaypointObject.transform;
        // }
        // else
        // {
        //     Debug.Log("Cannot find 'Target' tag");
        // }

        // Get the HealthManagerUI component
        healthManager = FindFirstObjectByType<HealthManagerUI>();        

        if (targetWaypoint == null || intermediateWaypoint == null)
        {
            Debug.Log("Waypoint is null"); //can change to debug log otherwise
        }

    }

    // Update is called once per frame
    void Update()
    {

        //Move towards the intermediate waypoint first
        if(intermediateWaypoint != null && !hasReachedIntermediateWaypoint)
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

            //ADD+
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

                Debug.Log("GOB ["+gameObject.GetInstanceID()+"] HEALED for: "+healAmount);
                ehm.currentHealth += healAmount;
                zmc.UpdateSpeed(2);
                hasBeenHealed = true;
                Debug.Log("From action, current HEALED health: "+ehm.currentHealth);

                //Update the healthbar for ancient skeleton
                FloatingHealthBarAncientSkeleton floatingHealthBar = GetComponent<FloatingHealthBarAncientSkeleton>();
                if (floatingHealthBar != null)
                {
                    floatingHealthBar.UpdateHealthBar();
                }
            }
            else if(targetWaypoint.tag != "HealthWaypoint")
            {
                hasReachedWaypoint = true;
                healthManager.ReduceHealth(); //Reduce the health by 1

                Debug.Log("Bad guy beat ya!");
                waveController = FindFirstObjectByType<WaveController>();
                // List<GameObject> activeEnemies = waveController.GetActiveEnemies();
                Debug.Log("A GOB ["+gameObject.GetInstanceID()+"] WON but they're no longer active so...active enemies BEFORE processing:"+waveController.GetActiveEnemies().Count);
                waveController.RemoveEnemy(gameObject);
                Debug.Log("A GOB ["+gameObject.GetInstanceID()+"] WON but they're no longer active so...active enemies AFTER processing:"+waveController.GetActiveEnemies().Count);


                Destroy(gameObject); //Destroy the enemy game object
                return;
            }  
        }
        else if (hasReachedWaypoint && agent.remainingDistance > agent.stoppingDistance) //Check if the enemy has reached the target
        {
            Debug.Log("WAYPOINT REACHED GOB with ID: "+gameObject.GetInstanceID() + " and their remaining distance is: "+agent.remainingDistance);
            hasReachedWaypoint = false;
            hasBeenHealed = false;
        }

        hasStartedMoving = true;
    }

    //ADD+
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
