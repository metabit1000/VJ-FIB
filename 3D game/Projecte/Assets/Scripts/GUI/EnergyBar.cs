using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
	public Gradient gradient;

	Slider slider;
	Image image;
	Energy energy;
	Color originalColor;

	void Start()
	{
		slider = GetComponent<Slider>();
		image = GetComponentInChildren<Image>();
		energy = GameObject.Find("PlayerShip").GetComponent<Energy>();
		slider.maxValue = 1f;
		originalColor = image.color;
	}

	void Update()
	{
		slider.value = energy.GetCurrentEnergy() / energy.GetMaxEnergy();
		image.color = energy.IsOverheated() ? Color.red : originalColor;
	}
}


