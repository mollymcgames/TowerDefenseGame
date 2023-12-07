using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemySpawnKnight : MonoBehaviour
{

    public GameObject knightPrefab;
    public GameObject redKnightPrefab;

    public Button knightButton;
    public Button redKnightButton;

    // Start is called before the first frame update
    void Start()
    {

        //Add click listeners to the buttons
        knightButton.onClick.AddListener(SpawnKnight);
        redKnightButton.onClick.AddListener(SpawnRedKnight);

    }

    void SpawnKnight()
    {
        //Sample a valid position on the Navmesh
        Vector3 spawnPosition = GetRandomNavMeshPosition();

        Instantiate(knightPrefab, spawnPosition, Quaternion.identity);
    }

    void SpawnRedKnight()
    {
        //Sample a valid position on the Navmesh
        Vector3 spawnPosition = GetRandomNavMeshPosition();

        Instantiate(redKnightPrefab, spawnPosition, Quaternion.identity);
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


