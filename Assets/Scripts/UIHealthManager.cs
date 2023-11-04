using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthManager : MonoBehaviour
{
    public EnemyHealthManager enemyHealthManager;
    public Slider slider; //reference to the healthbar
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = enemyHealthManager.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = enemyHealthManager.currentHealth;

        //update the postion of the healthbar to match the position of the enemy
        transform.position = Camera.main.WorldToScreenPoint(enemyHealthManager.transform.position + new Vector3(0, 2, 0));

    }
}
