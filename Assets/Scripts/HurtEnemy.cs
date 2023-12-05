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
            // Destroy(other.gameObject);

            EnemyHealthManager enemyHealthManager = other.GetComponent<EnemyHealthManager>();

            // EnemyHealthZombie enemyHealthManager = other.GetComponent<EnemyHealthZombie>();

            if (enemyHealthManager != null)
            {
                enemyHealthManager.TakeDamage(5);
                Debug.Log("Damage dealt!");
            }
            // else
            // {
            //     Debug.LogWarning("EnemyHealthZombie component not found!");
            // }
        }
    }
}
