using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubGoal {

    // Dictionary to store our goals
    public Dictionary<string, int> subGoals;
    // Bool to store if goal should be removed after the goal is achieved
    public bool remove;

    // Constructor
    public SubGoal(string s, int subgoalImportanceFactor, bool r) {

        subGoals = new Dictionary<string, int>();
        subGoals.Add(s, subgoalImportanceFactor);
        remove = r;
    }
}

public class GoapAgent : MonoBehaviour {

    // This contains ALL the Actions associated with our enemy
    public List<GoapAction> actions = new List<GoapAction>();
    
    // And these are the things our enemy wants to do, their Goals!
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    // These are the things our enemy belives to be true in the world around them
    public WorldStates beliefs = new WorldStates();

    // This is a reference to the Planner that works out what Actions will be performed
    GoapPlanner planner;
    
    // This is the current Queue of Action's our enemy is going to be performing...
    Queue<GoapAction> actionQueue;
    
    // And this is the CURRENT Action our enemy is performing...
    public GoapAction currentAction;
    
    SubGoal currentGoal;

    // Start is called before the first frame update
    public void Start() 
    {
        GoapAction[] configuredActions = this.GetComponents<GoapAction>();
        foreach (GoapAction a in configuredActions) {
            actions.Add(a);
        }
    }

    bool invoked = false;
    void CompleteAction()
    {
        Debug.Log("Completing action: "+currentAction.actionName);
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;        
    }


    public void ResetPlan()
    {
        Debug.Log("Resetting entire plan!");
        CompleteAction();
        planner = null;
        actionQueue = null;
    }

    // LateUpdate is called once per frame
    // IMPORTANT! This is where the plan can be made to be recalculated from time to time based on if enough changes have happened to the world!
    void LateUpdate() 
    {
        /* This method does X things...
        * 1. Check the current action and run it when the enemy is at the target waypoint
        * 2. Check that a planner has been instantiated and has been used to generate a new plan using 
        *    a sorted list of goals.
        * 3. Maintain the Queue of actions, removing the current action when it's done and flagged for removal.
        * 4. 
        */


        // Check if there's a current action that is running....
        if ( currentAction != null && currentAction.running)
        {
            Debug.Log("Checking running action: "+currentAction.actionName);

            // This is important so that agent MOVES TOWARDS THE GOAL
            float distanceToTarget = Vector3.Distance(currentAction.target.transform.position, this.transform.position);
            
            // Is the enemy at it's destination yet?
            if ( currentAction.agent.hasPath && distanceToTarget < 1.5f)
            {
                Debug.Log("This action has now completed! ["+currentAction.actionName+"]");
                // Stops an action being invoked twice!
                if ( !invoked)
                {
                    // This runs the method that wraps up the action AFTER the action's duration has expired.
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            else {
                Debug.Log("Running action too far away from target still: "+currentAction.actionName + " , distance="+distanceToTarget);
            }
            return;
        }

        // Check and generate the plan.
        if ( planner == null || actionQueue == null)
        {
            planner = new GoapPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach(KeyValuePair<SubGoal,int> sg in sortedGoals)
            {
                // No agent beliefs, just use the world goals
                actionQueue = planner.plan(actions, sg.Key.subGoals, beliefs);
                if ( actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        // Check the action and remove it from the queue if flagged for removal
        if ( actionQueue != null && actionQueue.Count == 0)
        {
            if ( currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            // This is IMPORTANT!  It's here we set the planner to NULL!  This means when there are no goals left the planner recalculates!
            planner = null;
        }

        // If the action queue is not empty however...
        if ( actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            if ( currentAction.PrePerform())
            {
                if ( currentAction.target == null && currentAction.targetTag != "")
                {
                    Debug.Log("Doing action. Going to target tag: "+currentAction.targetTag);
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                }

                // Give the agent somewhere to go!
                if ( currentAction.target != null)
                {
                    currentAction.running = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
            }
            else
            {
                // This forces a new plan to be generated!
                actionQueue = null;
            }
        }
    }
}


