using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnKnight : MonoBehaviour
{

    public GameObject knightPrefab;
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy();   
    }

    void SpawnEnemy()
    {
        //Sample a valid position on the Navmesh
        Vector3 spawnPosition = GetRandomNavMeshPosition();

        Instantiate(knightPrefab, spawnPosition, Quaternion.identity);
    }

        Vector3 GetRandomNavMeshPosition()
        {
            NavMeshHit hit;
            Vector3 randomPosition = Vector3.zero;

            //Keep sampling until a valid position is found on the Navmesh
            if (NavMesh.SamplePosition(randomPosition, out hit, 10f, NavMesh.AllAreas))
            {
                randomPosition = hit.position;
            }
            return randomPosition;
        }
    }


