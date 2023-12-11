using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

// This class represents a "node" in the graphs of Actions that the Planner will build up
public class Node
{
    public Node parent;
    
    // A node has to have a cost - as these costs get added together with other Actions in a 
    // candidate plan.  Assuming there could be multiple candidate plan's - the plan with the
    // cheapest total cost is then selected.
    public float cost;

    // This is for storing the possible FUTURE state of the world when the Action this 
    // node represents if it were running.
    public Dictionary<string, int> state;

    // And every Node is associated to a single Action
    //@TODO Possibly a Node and an Action could well be merged together in the future...
    public GoapAction action;

    // A basic constructor
    public Node(Node parent, float cost, Dictionary<string, int> allstates, GoapAction action) //dont think this is used
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allstates);
        this.action = action;
    }

    // This constructor takes in the states of the world the Action and loads them into the states in the Node.
    public Node(Node parent, float cost, Dictionary<string, int> allStates, Dictionary<string, int> beliefStates, GoapAction action) {

        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates);

        // As well as the world states add the enemy's beliefs as states that can be
        // used to match preconditions
        foreach (KeyValuePair<string, int> b in beliefStates)
        {
            if (!this.state.ContainsKey(b.Key))
            {
                this.state.Add(b.Key, b.Value);
            }
        }
        this.action = action;
    }

}

// This class is the planner itself, and works out what Actions should be in Queue for the Enemy to work through.
public class GoapPlanner
{
    public string planName = "default";

    public Queue<GoapAction> plan(List<GoapAction> actions, Dictionary<string, int> enemyGoal, WorldStates worldBeliefStates, string planName)
    {
        this.planName = planName;
        return plan(actions, enemyGoal, worldBeliefStates);
    }

    public Queue<GoapAction> plan(List<GoapAction> actions, Dictionary<string, int> enemyGoal, WorldStates worldBeliefStates)
    {
        List<GoapAction> usableActions = new List<GoapAction>();
        foreach (GoapAction a in actions)
        {
            if (a.IsAchievable())
            {
                usableActions.Add(a);
            }
        }

        // First node, so it has null parent, zero cost, etc
        List<Node> leaves = new List<Node>();
        // Node start = new Node(null, 0.0f, GoapWorld.Instance.GetWorld().GetStates(), null);
        // Dictionary<string,int> beliefState = new Dictionary<string,int>();
        // beliefState.Add("isHealthyEnough",1);
        Node start = new Node(null, 0.0f, GoapWorld.Instance.GetWorld().GetStates(), worldBeliefStates.GetStates(), null);


        // This will recurse along other Nodes using the first node, which remember, has no parent!
        // It might look like one line - but a LOT is going inside!!!
        bool success = BuildGraph(start, leaves, usableActions, enemyGoal);

        if ( !success)
        {
            // Debug.Log("["+planName+"] NO PLAN FOUND!");
            return null;
        }


        // Iterate through each leaf to identify the cheapest leaf.
        // Then starting from that cheapest leaf, back up the chain of parent leaves and sum up the total cost.

        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if (leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }
        
        // Now we have to work out our cheapest leaf so that we can make a Queue out of the Actions represented by the nodes in 
        // the plan for the Enemy to work its way through
        List<GoapAction> result = new List<GoapAction>();
        Node n = cheapest;
        while (n != null)
        {
            // Only the start node will have a null action
            if ( n.action != null )
            {
                result.Insert(0, n.action);
            }
            //Proceed to the next parent to obtain the next action
            n = n.parent;
        }


        Queue<GoapAction> queue = new Queue<GoapAction>();  //Queue that has the actions for the enemy to perform

        foreach (GoapAction a in result)
        {
            queue.Enqueue(a);            
        }

        // Debug.Log("We have a plan ["+planName+"] These are the queue steps: ");
        // int step = 1;
        // foreach (GoapAction a in queue) 
        // {
        //     Debug.Log("["+planName+"] Queue step ["+ step++ +"]: "+a.actionName);
        // }

        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<GoapAction> usableActions, Dictionary<string,int> enemyGoal)
    {
        bool foundPath = false;
        foreach (GoapAction nextPossibleAction in usableActions)
        {
            // The parent Action(node) state is known and that's what our GOAP is trying to find an action to match
            // Examine the next Action and see if it's achievable based on the parent state.
            // Disregard the next possible action if it's unattainable given the parent state.
            if ( nextPossibleAction.IsAchievableGiven(parent.state))
            {
                // Make a Dictionary of states held within the parent state. 
                // It is advisable to take a copy to prevent unintentional manipulation to the parent state.
                // Theese copies serve as representations of the potential future state of the world, should the Action be performed.
                Dictionary<string,int> projectedStates = new Dictionary<string, int>(parent.state);

                // The current state is known from the parent state, but now we have to "pretend" our next possible action has been performed.
                // Load in to the view of the current state the "after effects" of the next possible action pretending that the action took place

                foreach (KeyValuePair<string, int> stateEffect in nextPossibleAction.afterEffects)
                {
                    if ( !projectedStates.ContainsKey(stateEffect.Key))
                    {
                        projectedStates.Add(stateEffect.Key, stateEffect.Value);
                    }
                }

                //Add up the cost of the parent and the next possible action
                Node node = new Node(parent, parent.cost + nextPossibleAction.cost, projectedStates, nextPossibleAction);

                // For each of the possible actions, determine if the goal of the enemy has been achieved yet.
                // Look through our current state "possible future model" and if in that we find the name
                // of one or more of our enemy goals (remember the goal is set in the enemy "Agent" class, e.g. CleverEnemy.cs) 
                // then we have a winner
                if ( GoalAchieved(enemyGoal, projectedStates))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else // No matches found in our projected states for the goal so recurse down the graph and look again on other remaining Actions/Nodes.
                {
                    List<GoapAction> subsetOfRemainingActions = ActionSubset(usableActions, nextPossibleAction);
                    bool found = BuildGraph(node, leaves, subsetOfRemainingActions, enemyGoal);
                    if ( found )                    
                    {
                        foundPath = true;
                    }
                }
            }
        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> enemyGoal, Dictionary<string, int> desiredStates)
    {
        foreach (KeyValuePair<string,int> g in enemyGoal)
        {
            if ( !desiredStates.ContainsKey(g.Key))
            {
                return false;
            }
        }
        return true;
    }

    //This returns a copy of the action list, minus the action that has just been tested and failed for trying to achieve a goal.
    private List<GoapAction> ActionSubset(List<GoapAction> actions, GoapAction removeThisActionAsItHasNotAchievedTheGoal)
    {
        List<GoapAction> reducedListOfPossibleActions = new List<GoapAction>();
        foreach(GoapAction a in actions )
        {
            if ( !a.Equals(removeThisActionAsItHasNotAchievedTheGoal))
            {
                reducedListOfPossibleActions.Add(a);
            }
        }
        return reducedListOfPossibleActions;
    }
}
