using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GoapWorld
{
    // The World model, implemented as a Singleton class.
    private static readonly GoapWorld instance = new GoapWorld();
    
    // And these are the states that can exist in our World.
    private static WorldStates world;


    static GoapWorld()
    {
        world = new WorldStates();

    }

    private GoapWorld()
    {
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
