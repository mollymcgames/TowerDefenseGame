using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ActionType
{
    doSomething,
    goSomewhere
}

public abstract class GoapAction : MonoBehaviour
{
    public ActionType actionType = ActionType.goSomewhere;

    public string actionName = "Action";
    
    public float cost = 1.0f;
    
    // The target upon which this Action takes effect
    public GameObject target;    
    
    // This defines how far away from the target we want the action's object to be from the target before the action is "done".
    public float targetDistance = 1.5f;

    [SerializeField] public string targetTag;
    public float duration = 0.0f;  // How long the action should run for
    
    // This is an array of mandatory world state preconditions that if there, 
    // will make this Action become active/valid.
    public WorldState[] preConditionsWorldState;
    
    // This is an array of world states that the world will be in once this Action has completed.
    public WorldState[] afterEffectsWorldState;
    
    public NavMeshAgent agent;

    // This is a list of pre conditions that make this Action valid.
    public Dictionary<string, int> preConditions;

    // And this is list of post conditions once the Action has run.
    public Dictionary<string, int> afterEffects;

    // This is what the "NPC" this Action is attached too believes. Could be 
    // things like health, how much ammo he has, etc.
    public WorldStates agentBeliefs;

    public WorldStates worldBeliefs;

    // A flag that indicates if this particular Action is running.
    public bool running = false;

    // A flag that indicates if this particular Action is saying, enough's enough, time to replan.
    public bool replan = false;


    public GoapAction()
    {
        preConditions = new Dictionary<string, int>();
        afterEffects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        // Put any pre conditions configured in the inspector into a list of pre conditions.
        if (preConditionsWorldState != null)
        {
            foreach (WorldState w in preConditionsWorldState)
            {
                preConditions.Add(w.key, w.value);
            }
        }

        // Put any post conditions configured in the inspector into a list of pre conditions.
        if (afterEffectsWorldState != null)
        {
            foreach (WorldState w in afterEffectsWorldState)
            {
                afterEffects.Add(w.key, w.value);
            }
        }

        // Get our agents beliefs
        worldBeliefs = this.GetComponent<GoapAgent>().beliefs;
    }

    public bool IsAchievable()
    {
        return true;
    }

    // This method checks if the action is achievable given what the world conditions compared/matched with the 
    // pre conditions configured in this Action.
    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach (KeyValuePair<string, int> p in preConditions)
        {
            if (!conditions.ContainsKey(p.Key))
            {
                return false;
            }
        }
        return true;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();

    public virtual void DoAction() {}
}
