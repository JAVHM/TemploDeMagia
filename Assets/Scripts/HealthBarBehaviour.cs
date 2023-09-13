using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    public Slider slider;
    public Color High;
    public Vector3 Offset;

    public void SetHealth(float health, float maxhealth)
    {
        slider.maxValue = maxhealth;
        slider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
        slider.fillRect.GetComponentInChildren<Image>().color = High;
    }
}
