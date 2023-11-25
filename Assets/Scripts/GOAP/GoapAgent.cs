using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubGoal {

    // Dictionary to store our goals
    public Dictionary<string, int> subGoals;
    // Bool to store if goal should be removed
    public bool remove;

    // Constructor
    public SubGoal(string s, int subgoalImportanceFactor, bool r) {

        subGoals = new Dictionary<string, int>();
        subGoals.Add(s, subgoalImportanceFactor);
        remove = r;
    }
}

public class GoapAgent : MonoBehaviour {

    public List<GoapAction> actions = new List<GoapAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public WorldStates beliefs = new WorldStates();

    GoapPlanner planner;
    Queue<GoapAction> actionQueue;
    public GoapAction currentAction;
    SubGoal currentGoal;

    // Start is called before the first frame update
    public void Start() 
    {
        GoapAction[] acts = this.GetComponents<GoapAction>();
        foreach (GoapAction a in acts) {
            actions.Add(a);
        }
    }

    bool invoked = false;
    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;        
    }

    // Update is called once per frame
    void LateUpdate() 
    {
        if ( currentAction != null && currentAction.running)
        {
            // This is important so that agent MOVES TOWARDS THE GOAL
            float distanceToTarget = Vector3.Distance(currentAction.target.transform.position, this.transform.position);
            if ( currentAction.agent.hasPath && distanceToTarget < 1f) //works out if the agent has a path and if the distance to the target is less than 1
            {
                // Stops an action being invoked twice!
                if ( !invoked)
                {
                    // @TODO read up on Invoke!
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }

        if ( planner == null || actionQueue == null)
        {
            planner = new GoapPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach(KeyValuePair<SubGoal,int> sg in sortedGoals)
            {
                // No agent beliefs, just use the world goals
                actionQueue = planner.plan(actions, sg.Key.subGoals, null);
                if ( actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        // Once we have an empty action queue...
        if ( actionQueue != null && actionQueue.Count == 0)
        {
            if ( currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
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


