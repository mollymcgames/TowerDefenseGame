using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GoapAction : MonoBehaviour
{
    public string actionName = "Action";
    
    public float cost = 1.0f;
    
    // The target upon which this Action take's affect
    public GameObject target;    
    
    [SerializeField] public string targetTag;
    // How long the action should run for
    public float duration = 0.0f;
    
    // This is an array of mandatory WORLD STATE preconditions that if there, 
    // will make this Action become active/valid.
    // NOTE: This is here so that they can be configured in the INSPECTOR
    public WorldState[] preConditionsWorldState;
    
    // And this is an array of WORLD STATES that the world will be in
    // once this Action has completed.
    // NOTE: This is here so that they can be configured in the INSPECTOR
    public WorldState[] afterEffectsWorldState;
    
    public NavMeshAgent agent;

    // This is a list of pre conditions that make this Action valid.
    // NOTE: These remain hidden from the INSPECTOR
    public Dictionary<string, int> preConditions;

    // And this is list of post conditions once the Action has run.
    // NOTE: These remain hidden from the INSPECTOR
    public Dictionary<string, int> afterEffects;

    // This is what the "NPC" this Action is attached too believes. Could be 
    // things like health, how much ammo he has, etc.
    public WorldStates agentBeliefs;

    public WorldStates worldBeliefs;

    // A little flag that indicates if this particular Action is running.
    public bool running = false;

    public GoapAction()
    {
        preConditions = new Dictionary<string, int>();
        afterEffects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        // Pop any pre conditions configured in the INSPECTOR into our list of pre conditions.
        if (preConditionsWorldState != null)
        {
            foreach (WorldState w in preConditionsWorldState)
            {
                preConditions.Add(w.key, w.value);
            }
        }

        // Pop any post conditions configured in the INSPECTOR into our list of pre conditions.
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
}
