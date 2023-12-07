using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GoapWorld
{
    // The World model, implemented as a Singleton class.
    private static readonly GoapWorld instance = new GoapWorld();
    
    // And these are the states that can exist in our World.
    private static WorldStates world;

    // @TODO Do we need this?  Some queue....
    private static Queue<GameObject> patients;

    static GoapWorld()
    {
        world = new WorldStates();

        // @TODO Still not sure if this is needed....
        patients = new Queue<GameObject>();
    }

    private GoapWorld()
    {
    }

    public void AddPatient(GameObject p)
    {
        patients.Enqueue(p);
    }

    public GameObject RemovePatient()
    {
        if (patients.Count == 0) return null;
        return patients.Dequeue();
        
    }
    public static GoapWorld Instance
    {
        get { return instance; }
    }

    public WorldStates GetWorld()
    {
        return world;
    }
}
