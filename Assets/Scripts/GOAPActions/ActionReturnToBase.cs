using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionReturnToBase : GoapAction
{
    EnemyController zmc;

    EnemyHealthManager ehm;


    public override bool PrePerform()
    {
        Debug.Log("ACTION: Returning to base...");
        // zmc = gameObject.GetComponent<EnemyController>();   // gets component from -this- gameobject
        // zmc.SetTarget(targetTag);

        // // Make the enemy go a little slower too, after all, they are injured!
        // zmc.UpdateSpeed(2.0f);
        // // zmc.UpdateStoppingDistance(0.1f);

        // // To be realistic, the enemy needs to loiter at the health base for a second!
        // duration = 0.5f;

        return true;
    }

    public override bool PostPerform()
    {        
        Debug.Log("ACTION: Returned to base...");
        return true;
    }

}
