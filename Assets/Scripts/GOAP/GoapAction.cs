using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GoapAction : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1.0f;
    public GameObject target;
    [SerializeField] public string targetTag;
    public float duration = 0;
    public WorldState[] preConditionsWorldState;
    public WorldState[] afterEffectsWorldState;
    public NavMeshAgent agent;

    public Dictionary<string, int> preConditions;
    public Dictionary<string, int> afterEffects;

    public WorldStates agentBeliefs;

    public bool running = false;

    public GoapAction()
    {
        preConditions = new Dictionary<string, int>();
        afterEffects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        if (preConditionsWorldState != null)
        {
            foreach (WorldState w in preConditionsWorldState)
            {
                preConditions.Add(w.key, w.value);
            }
        }

        if (afterEffectsWorldState != null)
        {
            foreach (WorldState w in afterEffectsWorldState)
            {
                afterEffects.Add(w.key, w.value);
            }
        }
    }

    public bool IsAchievable()
    {
        return true;
    }

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
