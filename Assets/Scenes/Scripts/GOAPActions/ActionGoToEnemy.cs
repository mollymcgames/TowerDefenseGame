using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ActionGoToEnemy : GoapAction
{
    RedKnightController zmc;

    EnemyHealthManager ehm;


    public override bool PrePerform()
    {
        Debug.Log("ACTION: Finding an enemy...");

        // Set the target to the nearest enemy
        List<GameObject> badGuys = new List<GameObject>(); //Get a list of all the enemies
        GameObject.FindGameObjectsWithTag("Enemy", badGuys);

        // Transform[] badGuysLocations = new Transform[badGuys.Count];
        // int i=0;
        // foreach (GameObject nextBadGuy in badGuys)
        // {
        //     badGuysLocations[i++] = nextBadGuy.transform;
        // }

        GameObject closestBadGuy = GetClosestEnemy(badGuys); //Get the closest enemy 
        target = closestBadGuy;

        zmc = gameObject.GetComponent<RedKnightController>();   // gets component from -this- gameobject
        zmc.SetTarget(target);

        // // Make the enemy go a little slower too, after all, they are injured!
        // zmc.UpdateSpeed(2.0f);
        // // zmc.UpdateStoppingDistance(0.1f);

        // // To be realistic, the enemy needs to loiter at the health base for a second!
        // duration = 0.5f;

        return true;
    }

    public override bool PostPerform()
    {        
        Debug.Log("ACTION: Found an enemy...");
        return true;
    }

    GameObject GetClosestEnemy (List<GameObject> enemies)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        // Loop around all the enemies and find the closest one
        foreach(GameObject potentialTarget in enemies)        
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
     
        return bestTarget;
    }

    // Transform GetClosestEnemy (Transform[] enemies)
    // {
    //     Transform bestTarget = null;
    //     float closestDistanceSqr = Mathf.Infinity;
    //     Vector3 currentPosition = transform.position;
    //     foreach(Transform potentialTarget in enemies)
    //     {
    //         Vector3 directionToTarget = potentialTarget.position - currentPosition;
    //         float dSqrToTarget = directionToTarget.sqrMagnitude;
    //         if(dSqrToTarget < closestDistanceSqr)
    //         {
    //             closestDistanceSqr = dSqrToTarget;
    //             bestTarget = potentialTarget;
    //         }
    //     }
     
    //     return bestTarget;
    // }


}
