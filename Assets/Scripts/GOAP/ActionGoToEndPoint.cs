using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionGoToEndPoint : GoapAction
{
    // public EnemyController zmc;

    private EnemyController zmc;

    //reference enemy controller
    private void Start()
    {
        zmc = this.GetComponent<EnemyController>();
    }

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
