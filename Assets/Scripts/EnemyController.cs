using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class EnemyController : MonoBehaviour
{
    private Animator myAnimator; //The animator component
    [SerializeField] private Transform targetWaypoint; //The waypoint the enemy is moving towards
    private HealthManagerUI healthManager; //The health manager script

    private WaveController waveController;

    private bool hasStartedMoving = false; // Check if the enemy has started moving

    [SerializeField] private float speed = 0.5f ; //The speed at which the enemy moves


    NavMeshAgent agent;
    private bool hasReachedWaypoint = false; //Check if the enemy has reached the target
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //Get the NavMeshAgent component
        agent.updateRotation = false; //Stop the NavMeshAgent component from rotating the enemy
        agent.updateUpAxis = false; //Stop the NavMeshAgent component from rotating the enemy
        agent.speed = speed; //Set the speed of the NavMeshAgent component
        agent.stoppingDistance = 0.5f; //Set the stopping distance of the NavMeshAgent component we might need to change this later but for now it works

        myAnimator = GetComponent<Animator>(); //Get the animator component

        // Set the targetWaypoint to the desired Vector3 position
        GameObject targetWaypointObject = new GameObject("TargetWaypoint");
        targetWaypoint = targetWaypointObject.transform;
        // targetWaypoint.position = new Vector3(6.07f, -2.58f, 0.5f); //hardcoded position of the target waypoint
        targetWaypoint.position = new Vector3(9.67f, -3.92f, 0.5f); //hardcoded position of the target waypoint

        // Get the HealthManagerUI component
        healthManager = FindFirstObjectByType<HealthManagerUI>();        

        if (targetWaypoint == null)
        {
            Debug.LogError("Waypoint is null");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Tracking GOB bad guy with ID: "+gameObject.GetInstanceID() + " and their remaining distance is: "+agent.remainingDistance);

        FollowTarget();
        if ( agent.pathPending == true || agent.remainingDistance <= 0.0009f) 
        {
            Debug.Log("GOB ["+gameObject.GetInstanceID()+"] THINKING about where to go!");
        }
        else if (hasStartedMoving && agent.remainingDistance <= agent.stoppingDistance) //Check if the enemy has reached the targe    
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
        else if (hasReachedWaypoint && agent.remainingDistance > agent.stoppingDistance) //Check if the enemy has reached the target
        {
            Debug.Log("WAYPOINT REACHED GOB with ID: "+gameObject.GetInstanceID() + " and their remaining distance is: "+agent.remainingDistance);
            hasReachedWaypoint = false;
        }

        hasStartedMoving = true;
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

    //we can delete this probably 
    // void DestroyEnemy()
    // {
    //     if(gameObject != null && gameObject.activeSelf)
    //     { 
    //         Debug.Log("Destroying GOB: "+gameObject.GetInstanceID());            
    //         Destroy(gameObject); //Destroy the enemy game object
    //     }
    // }

}
