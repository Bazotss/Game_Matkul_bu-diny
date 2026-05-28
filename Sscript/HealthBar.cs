using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = Mathf.Lerp(slider.value, health, Time.deltaTime * 10);
    }

    public void SetHealth(int health)
    {
        slider.value = Mathf.Lerp(slider.value, health, Time.deltaTime * 10);
    }
}
