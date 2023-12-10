using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBarKnight : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public HealthManager healthManager;


    void Start()
    {
        slider.maxValue = healthManager.maxHealth;
    }

    void Update()
    {
        slider.value = healthManager.currentHealth;
        UpdateHealthBar();

    }

    public void UpdateHealthBar()
    {
        slider.value = healthManager.currentHealth;
    }
}
