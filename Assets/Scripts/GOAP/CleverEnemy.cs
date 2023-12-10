using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleverEnemy : GoapAgent
{

    EnemyHealthManager ehm;

    private bool resetPlan = false;
    
    public void Start()
    {
        base.Start();
        // Create a new subgoal for the Clever Enemy
        SubGoal s1 = new SubGoal("isAtTargetWaypoint", 1, true);
        // Add the subgoal as a priority 3 to the goals. Lower numbers are higher priority
        goals.Add(s1, 3); 

        SubGoal s2 = new SubGoal("isGettingHealed", 1, true);
        goals.Add(s2,5);

        // Our enemy starts out thinking it's healthy enough. That will change...
        beliefs.SetState("isHealthyEnough",1);
    }

    public void Update()
    {
        ehm = gameObject.GetComponent<EnemyHealthAncientSkeleton>();
        Debug.Log("From Clever Enemy, current health: "+ehm.currentHealth);

        if ( ehm.currentHealth > 25 )
        {
            beliefs.SetState("isHealthyEnough",1);            
            beliefs.RemoveState("needsHealing");
            resetPlan = false;
        }
        else if ( ehm.currentHealth <= 25) 
        {
            //The enemy now needs to replan and seek out medical aid
            beliefs.SetState("needsHealing",1);
            beliefs.RemoveState("isHealthyEnough");
            if ( resetPlan == false)
            {
                Debug.Log("Unhealthy, resetting plan!");
                ResetPlan(); 
                resetPlan = true;
            }
        }
    }
}
