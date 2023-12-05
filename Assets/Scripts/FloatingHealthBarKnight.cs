using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBarKnight : MonoBehaviour
{
    // Start is called before the first frame update    

    [SerializeField] private Slider slider;
    public HealthManager healthManager;


    void Start()
    {
        slider.maxValue = healthManager.maxHealth;
    }

    // Update is called once per frame
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
