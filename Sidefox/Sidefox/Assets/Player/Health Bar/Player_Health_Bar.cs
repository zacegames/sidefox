using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Health_Bar : MonoBehaviour

{
    public Slider pHealthSlider;

    public void SetMaxHealth(int health)
    {
        pHealthSlider.maxValue = health;
        pHealthSlider.value = health;
    }

    public void SetHealth(float Health_Value)
    {
        pHealthSlider.value = Health_Value;
    }
}
