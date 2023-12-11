using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionAttackEnemy : GoapAction
{
    RedKnightController rnc;

    EnemyHealthManager ehm;

    public override bool PrePerform()
    {
        Debug.Log("ACTION: About to attack an enemy...");
        actionType = ActionType.doSomething;

        // Once the target is found, attack quickly
        duration = 0f;

        return true;
    }

    public override bool PostPerform()
    {        
        Debug.Log("ACTION: Enemy dead...");
        rnc = GetRedKnightController();
        rnc.StopAttack();
        return true;
    }

    private RedKnightController GetRedKnightController()
    {
        return gameObject.GetComponent<RedKnightController>();   // gets component from -this- gameobject
    }

    public override void DoAction()
    {
        Debug.Log("ACTION: Attacking an enemy...");        
        base.DoAction();
        rnc = GetRedKnightController();  // gets component from -this- gameobject
        try 
        {
            ehm = rnc.targetWaypoint.gameObject.GetComponent<EnemyHealthManager>(); // we need to know our enemy's health...
        }
        catch ( Exception e) 
        {
            return;
        }
       
        // Keep on attacking until enemy is dead
        if ( ehm.currentHealth >= 0 )
        {
            // Debug.Log("ACTION: Enemy health before: "+ehm.currentHealth);
            rnc.InitiateAttack();            
            // Debug.Log("ACTION: Enemy health after: "+ehm.currentHealth);
            Invoke("DoAction",0.75f);
        }
        else
        {
            rnc.StopAttack();
            replan = true;
        }
    }

}
