using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    // Start is called before the first frame update    

    [SerializeField] private Slider slider;
    public EnemyHealthZombie enemyHealthManager;


    void Start()
    {
        slider.maxValue = enemyHealthManager.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = enemyHealthManager.currentHealth;

    }
}
