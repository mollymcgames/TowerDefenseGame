using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GoblinRiderController : MonoBehaviour
{

    private Animator myAnimator; //The animator component

    private Vector3 targetPosition; //The waypoint the enemy is moving towards

    [SerializeField] private float speed = 3.0f ; //The speed at which the enemy moves



    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>(); //Get the animator component

        // Set the target position
        targetPosition = new Vector3(9.67f, -3.92f, 0.503175318f);        
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }

    public void FollowTarget()
    {
        myAnimator.SetBool("isMoving", true);
        // myAnimator.SetFloat("moveX", (targetPosition.x - transform.position.x));
        // myAnimator.SetFloat("moveY", (targetPosition.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

}
