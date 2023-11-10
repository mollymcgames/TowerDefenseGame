using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBarBat : MonoBehaviour
{
    // Start is called before the first frame update    

    [SerializeField] private Slider slider;
    public EnemyHealthBat enemyHealthBat;


    void Start()
    {
        slider.maxValue = enemyHealthBat.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = enemyHealthBat.currentHealth;

    }
}
