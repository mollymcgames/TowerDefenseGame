using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class EnemySpawnKnight : MonoBehaviour
{

    public GameObject knightPrefab;
    public GameObject redKnightPrefab;

    public Button knightButton;
    public Button redKnightButton;

    public TextMeshProUGUI knightCountText;
    public TextMeshProUGUI redKnightCountText;

    private int knightCount = 1; //Set the amount of knights 
    private int redKnightCount = 1; //Set the amount of red knights

    private bool buttonVisible = false;

    public LauncherWeapon launcherWeapon;

    private bool attackAnimationPlayed = false;

    // Start is called before the first frame update
    void Start()
    {

        //Add click listeners to the buttons
        knightButton.onClick.AddListener(SpawnKnight);
        redKnightButton.onClick.AddListener(SpawnRedKnight);

        UpdateCountText();
        UpdateButtonInteractable();

        // Manually assign the launcherWeapon reference
        launcherWeapon = GetComponentInChildren<LauncherWeapon>();
        if (launcherWeapon == null)
        {
            Debug.LogError("LauncherWeapon script not found on the same GameObject!");
        }

    }

    IEnumerator PlayAttackAnimation(float duration)
    {
        launcherWeapon.PlayAttackAnimation();
        yield return new WaitForSeconds(duration);
        attackAnimationPlayed = false;
        launcherWeapon.PlayIdleAnimation();

    }

    void SpawnKnight()
    {
        if(knightCount > 0)
        {
            //Sample a valid position on the Navmesh
            Vector3 spawnPosition = GetRandomNavMeshPosition();

            Instantiate(knightPrefab, spawnPosition, Quaternion.identity);

            //Decrement the knight count and update the text 
            knightCount--;
            UpdateCountText();
            UpdateButtonInteractable();

            StartCoroutine(PlayAttackAnimation(1f));
            
        }

    }

    void SpawnRedKnight()
    {
        if(redKnightCount > 0)
        {
            //Sample a valid position on the Navmesh
            Vector3 spawnPosition = GetRandomNavMeshPosition();

            Instantiate(redKnightPrefab, spawnPosition, Quaternion.identity);

            //Decrement the red knight count and update the text
            redKnightCount--;
            UpdateCountText();
            UpdateButtonInteractable();
            
            launcherWeapon.PlayAttackAnimation();

            StartCoroutine(PlayAttackAnimation(1f));
        }
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

    void OnMouseOver()
    {
        //show the upgrade button when the mouse is over the tower
        if (!buttonVisible)
        {
            knightButton.gameObject.SetActive(true);
            redKnightButton.gameObject.SetActive(true);
            buttonVisible = true;
        }
    }

    void UpdateCountText()
    {
        //Update the text for the knight count
        knightCountText.text = knightCount.ToString();
        redKnightCountText.text = redKnightCount.ToString();
    }

    void UpdateButtonInteractable()
    {
        //Disable the buttons if the count is 0
        knightButton.interactable = knightCount > 0;
        redKnightButton.interactable = redKnightCount > 0;
    }

    void OnMouseExit()
    {
        //hide the upgrade button when the mouse is no longer over the tower
        if (buttonVisible)
        {
            knightButton.gameObject.SetActive(false);
            redKnightButton.gameObject.SetActive(false);
            buttonVisible = false;
        }
    }

}


