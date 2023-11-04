using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public int damageAmount = 1; // The damage the arrow will do to the enemy

    public void DealDamage()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if(enemy != null)
        {
            EnemyHealthManager enemyHealthManager = enemy.GetComponent<EnemyHealthManager>();
            if(enemyHealthManager != null)
            {
                enemyHealthManager.TakeDamage(damageAmount); //Change the value if needed
            }
        }
    }
}
