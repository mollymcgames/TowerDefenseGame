using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform targetWaypoint; //The waypoint the enemy is moving towards
    public float speed = 4f; //The speed at which the enemy moves
    // Start is called before the first frame update
    void Start()
    {
        if(targetWaypoint == null)
        {
            Debug.LogError("Waypoint is null");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(targetWaypoint != null)
        {
            //Move towards the waypoint
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
        }
        
    }
}
