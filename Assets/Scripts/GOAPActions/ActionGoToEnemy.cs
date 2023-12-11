using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ActionGoToEnemy : GoapAction
{
    RedKnightController rnc;

    public override bool PrePerform()
    {
        Debug.Log("ACTION: Finding an enemy...");

        // Set the target to the nearest enemy
        List<GameObject> badGuys = new List<GameObject>(); //Get a list of all the enemies
        GameObject.FindGameObjectsWithTag("Enemy", badGuys);

        GameObject closestBadGuy = GetClosestEnemy(badGuys); //Get the closest enemy 
        if ( closestBadGuy == null) 
        {
            // No enemies found - so bail, replan and hope more enemies arrive
            replan = true;
            return false;
        }
        target = closestBadGuy;

        rnc = GetRedKnightController();   // gets component from -this- gameobject
        rnc.StopAttack();
        rnc.SetTarget(target);

        // Heading towards an enemy? We need to be nice and close...
        targetDistance = 1.09f;

        // Once the target is found, we want to move on quickly!
        duration = 0f;
        
        return true;
    }

    public override bool PostPerform()
    {        
        // Debug.Log("ACTION: Found an enemy...");
        rnc = GetRedKnightController();   // gets component from -this- gameobject
        rnc.StopAttack();
        return true;
    }

    private RedKnightController GetRedKnightController()
    {
        return gameObject.GetComponent<RedKnightController>();   // gets component from -this- gameobject
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
}
