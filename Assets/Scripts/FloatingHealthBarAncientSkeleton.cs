using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBarAncientSkeleton : MonoBehaviour
{
    // Start is called before the first frame update    

    [SerializeField] private Slider slider;
    public EnemyHealthAncientSkeleton enemyHealthAncientSkeleton;


    void Start()
    {
        slider.maxValue = enemyHealthAncientSkeleton.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = enemyHealthAncientSkeleton.currentHealth;

    }

    public void UpdateHealthBar()
    {
        slider.value = enemyHealthAncientSkeleton.currentHealth;
    }
}
