using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBarNecromancer : MonoBehaviour
{
    // Start is called before the first frame update    

    [SerializeField] private Slider slider;
    public EnemyHealthNecromancer enemyHealthNecromancer;


    void Start()
    {
        slider.maxValue = enemyHealthNecromancer.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = enemyHealthNecromancer.currentHealth;

    }
}
