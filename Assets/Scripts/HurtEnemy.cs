using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D (Collider2D other)
    {
        if(other.tag == "Enemy")
        {

            EnemyHealthManager enemyHealthManager = other.GetComponent<EnemyHealthManager>();

            if (enemyHealthManager != null)
            {
                enemyHealthManager.TakeDamage(2);
                Debug.Log("Damage dealt!");
            }
        }
    }
}
