using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleverRedKnight : GoapAgent
{
    private bool resetPlan = false;
    
    // Start is called before the first frame update
    public void Start()
    {
        base.Start();
        // Create a new subgoal for our Clever Knight!
        SubGoal s1 = new SubGoal("enemyIsDead", 1, true);
        // Add the subgoal as a priority 3 to the goals. Lower numbers are higher priority!
        goals.Add(s1, 5); 

        SubGoal s2 = new SubGoal("isBackAtBase", 1, true);
        goals.Add(s2,3);
    }

    public void Update()
    {
        
    }
}