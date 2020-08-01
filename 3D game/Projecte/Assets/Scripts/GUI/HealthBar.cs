using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Gradient gradient;

	Slider slider;
	Image image;
	Health health;

	void Start()
	{
		slider = GetComponent<Slider>();
		image = GetComponentInChildren<Image>();
		health = GameObject.Find("PlayerShip").GetComponent<Health>(); ;
		slider.maxValue = 1f;
	}

	void Update() 
	{
		slider.value = health.GetCurrentHealth() / health.GetMaxHealth();
		image.color = gradient.Evaluate(slider.normalizedValue);
	}
}
