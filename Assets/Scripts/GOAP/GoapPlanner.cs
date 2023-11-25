using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public GoapAction action;

    public Node(Node parent, float cost, Dictionary<string, int> allstates, GoapAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allstates);
        this.action = action;
    }
}

public class GoapPlanner
{
    public Queue<GoapAction> plan(List<GoapAction> actions, Dictionary<string, int> goal, WorldStates states)
    {
        List<GoapAction> usableActions = new List<GoapAction>();
        foreach(GoapAction a in actions)
        {
            if (a.IsAchievable())
            {
                usableActions.Add(a);
            }
        }

        List<Node> leaves = new List<Node>();
        // First node, so it has null parent, zero cost, etc
        Node start = new Node(null, 0, GoapWorld.Instance.GetWorld().GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if ( !success)
        {
            Debug.Log("NO PLAN FOUND!");
            return null;
        }

        // Now we need to work through each of the leaves and find the cheapest leaf
        // Why? Because....then starting from that cheapest leaf we can then work
        // back up the chain of parent leaves and sum up the total cost.
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

        List<GoapAction> result = new List<GoapAction>();
        Node n = cheapest;
        while (n != null)
        {
            // Only the start node will have a NULL action!
            if ( n.action != null )
            {
                result.Insert(0, n.action);
            }
            // Move on to the next parent so that we get the next (well previous!) action!
            n = n.parent;
        }

        // Finally we can now create our queue that has the actions our agent can go and perform.
        Queue<GoapAction> queue = new Queue<GoapAction>();
        foreach (GoapAction a in result)
        {
            queue.Enqueue(a);            
        }

        Debug.Log("We have a plan.  These are the queue steps: ");
        int step = 1;
        foreach (GoapAction a in queue) 
        {
            Debug.Log("Queue step ["+ step++ +"]: "+a.actionName);
        }

        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<GoapAction> usableActions, Dictionary<string,int> goal)
    {
        bool foundPath = false;
        foreach (GoapAction action in usableActions)
        {
            if ( action.IsAchievableGiven(parent.state))
            {
                Dictionary<string,int> currentState = new Dictionary<string, int>(parent.state);
                foreach (KeyValuePair<string, int> effect in action.afterEffects)
                {
                    if ( !currentState.ContainsKey(effect.Key))
                    {
                        currentState.Add(effect.Key, effect.Value);
                    }
                }
                // Add up the costs of the nodes as we go - which is how the "cheapest" node works above.
                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if ( GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GoapAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if ( found )                    
                    {
                        foundPath = true;
                    }
                }
            }
        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach (KeyValuePair<string,int> g in goal)
        {
            if ( !state.ContainsKey(g.Key))
            {
                return false;
            }
        }
        return true;
    }

    private List<GoapAction> ActionSubset(List<GoapAction> actions, GoapAction removeMe)
    {
        List<GoapAction> subset = new List<GoapAction>();
        foreach(GoapAction a in actions )
        {
            if ( !a.Equals(removeMe))
            {
                subset.Add(a);
            }
        }
        return subset;
    }
}
