using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private GameObject[] enemies; 
    // private GameObject enemyIBelongTo;

    private void Start() {
    }

    public void setHealthOnBar(int health) {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
