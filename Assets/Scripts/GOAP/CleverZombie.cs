using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleverZombie : GoapAgent
{
    // Start is called before the first frame update
    public void Start()
    {
        base.Start();
        // Create a new subgoal for our CleverZombie!
        SubGoal s1 = new SubGoal("isAtTargetWaypoint", 1, true);
        // Add the subgoal as a priority 3 to the goals.
        goals.Add(s1, 3);
    }

}
