using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public int damageAmount = 1; // The damage the arrow will do to the enemy

    private Rigidbody2D arrowRigidbody; // The arrow's rigidbody component


    private void Start()
    {
        arrowRigidbody = GetComponent<Rigidbody2D>(); // Get the arrow's rigidbody component

    }

    public void ShootArrow(Vector3 direction, float speed)
    {
        arrowRigidbody.velocity = direction * speed; // Set the arrow's velocity to the direction and speed
    }

    public void ArrowBehaviour(Transform targetWaypoint, bool shouldFire, List<GameObject> inactiveArrows, float arrowSpeed)
    {

        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy != null)
        {
            UnityEngine.AI.NavMeshAgent enemyAgent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (enemyAgent != null && targetWaypoint != null)
            {
                if (Vector3.Distance(enemyAgent.transform.position, targetWaypoint.position) <= 1.2f)
                {
                    // Stop firing when the enemy reaches the waypoint
                    shouldFire = false;
                    inactiveArrows.Add(gameObject);
                    Destroy(gameObject);
                }
                else if (Vector3.Distance(transform.position, enemyAgent.transform.position) <= 0.5f)
                {
                    // Handle the arrow hitting the enemy
                    DealDamage();
                    inactiveArrows.Add(gameObject);
                    Destroy(gameObject);
                }
                else if (shouldFire)
                {
                    transform.position = Vector3.MoveTowards(transform.position, enemyAgent.transform.position, arrowSpeed * Time.deltaTime);
                }
            }
        }
    }

    public void DealDamage()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if(enemy != null)
        {
            EnemyHealthManager enemyHealthManager = enemy.GetComponent<EnemyHealthManager>();
            if(enemyHealthManager != null)
            {
                enemyHealthManager.TakeDamage(damageAmount); // Change the value if needed
            }
        }
    }
}
