using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieManController : MonoBehaviour
{
    private Animator myAnimator; //The animator component
    public Transform targetWaypoint; //The waypoint the enemy is moving towards
    public HealthManagerUI healthManager; //The health manager script
    private bool hasStartedMoving = false; // Check if the enemy has started moving


    NavMeshAgent agent;
    private bool hasReachedWaypoint = false; //Check if the enemy has reached the target
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //Get the NavMeshAgent component
        agent.updateRotation = false; //Stop the NavMeshAgent component from rotating the enemy
        agent.updateUpAxis = false; //Stop the NavMeshAgent component from rotating the enemy

        myAnimator = GetComponent<Animator>(); //Get the animator component

        if (targetWaypoint == null)
        {
            Debug.LogError("Waypoint is null");
        }

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update: agent.remainingDistance: " + agent.remainingDistance);
        Debug.Log("Update: agent.stoppingDistance: " + agent.stoppingDistance);
        FollowTarget();
        if (hasStartedMoving && !hasReachedWaypoint && agent.remainingDistance <= agent.stoppingDistance) //Check if the enemy has reached the targe    
        {
            hasReachedWaypoint = true;
            healthManager.ReduceHealth(); //Reduce the health by 1
        }
        else if (hasReachedWaypoint && agent.remainingDistance > agent.stoppingDistance) //Check if the enemy has reached the target
        {
            hasReachedWaypoint = false;
        }

        hasStartedMoving = true;
    }

    public void FollowTarget()
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
