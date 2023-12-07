using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGoToEndPoint : GoapAction
{
    EnemyController zmc;

    public override bool PrePerform()
    {
        zmc = gameObject.GetComponent<EnemyController>();   // gets component from -this- gameobject
        zmc.SetTarget(targetTag);
        return true;
    }

    public override bool PostPerform()
    {
        return true;
    }

}
