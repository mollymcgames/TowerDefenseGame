using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgrade : MonoBehaviour
{

    public Button upgradeButton;
    public GameObject upgradedPrefab; // the upgraded tower prefab
    // Start is called before the first frame update
    void Start()
    {
        
        upgradeButton.onClick.AddListener(UpgradeTower);
    }

    void UpgradeTower()
    {
        //Instantiate the upgraded tower prefab at the same position as the current tower
        Instantiate(upgradedPrefab, transform.position, transform.rotation);
        //Destroy the current tower
        Destroy(gameObject);
        Debug.Log("Tower upgraded!");
    }
}
