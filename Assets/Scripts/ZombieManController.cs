using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManController : MonoBehaviour
{
    private Animator myAnimator; //The animator component
    public Transform targetWaypoint; //The waypoint the enemy is moving towards
    public float speed = 4f; //The speed at which the enemy moves
    [SerializeField] 
    // private float range;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>(); //Get the animator component

        if(targetWaypoint == null)
        {
            Debug.LogError("Waypoint is null");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();          
    }
        
    public void FollowTarget()
    {
        if (targetWaypoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
            myAnimator.SetBool("isMoving", true);
            myAnimator.SetFloat("moveX", targetWaypoint.position.x - transform.position.x);
            myAnimator.SetFloat("moveY", targetWaypoint.position.y - transform.position.y);

            if (transform.position == targetWaypoint.position) // Check if the enemy has reached the target
            {
                myAnimator.SetBool("isMoving", false); // Set isMoving to false to stop the animation
            } else 
            {        
                myAnimator.SetBool("isMoving", true);
            }
        }
        else
        {
            myAnimator.SetBool("isMoving", false);
        }
    }
}
