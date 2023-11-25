using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGoToHealth : GoapAction
{
    public EnemyController zmc;

    public override bool PrePerform()
    {
        zmc.SetTarget(targetTag);
        return true;
    }

    public override bool PostPerform()
    {
        return true;
    }

}
