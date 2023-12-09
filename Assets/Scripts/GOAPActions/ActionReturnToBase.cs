using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionReturnToBase : GoapAction
{
    RedKnightController rnc;

    EnemyHealthManager ehm;


    public override bool PrePerform()
    {
        Debug.Log("ACTION: Returning to base...");

        rnc = gameObject.GetComponent<RedKnightController>();   // gets component from -this- gameobject
        rnc.SetTarget(rnc.GetOriginalSpawnLocation());       

        return true;
    }

    public override bool PostPerform()
    {        
        Debug.Log("ACTION: Returned to base...");
        replan = true;  
        return true;
    }

}
