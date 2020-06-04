using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Energy : MonoBehaviour
{
    public float maxEnergy;
    public float energyPerSecond;
    public float rechargeEnergy = 20f;
    [Range(0,1)] public float overheatingStopThreshold;

    public AudioClip overheatSound;
    public AudioClip stopOverheatSound;

    AudioSource audioSource;
    float currentEnergy;
    bool isOverheated;


    public float GetMaxEnergy() => maxEnergy;

    public float GetCurrentEnergy() => currentEnergy;

    public bool IsOverheated() => isOverheated;


    public bool Consume(float value)
    {
        if (isOverheated) return false;
        else
        {
            currentEnergy -= value;
            return true;
        }
    }

    public void Recharge()
    {
    	currentEnergy = Mathf.Min(currentEnergy + rechargeEnergy, maxEnergy);
    }


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentEnergy = maxEnergy;
    }


    void Update()
    {
        currentEnergy += energyPerSecond * Time.deltaTime;
        currentEnergy = Mathf.Min(currentEnergy, maxEnergy);

        if (currentEnergy >= overheatingStopThreshold * maxEnergy && isOverheated) 
        { 
            isOverheated = false;
            if (audioSource != null && overheatSound != null)
            {
                audioSource.PlayOneShot(stopOverheatSound);
            }
            
        }
        else if (currentEnergy <= 0 && !isOverheated)
        {
            isOverheated = true;
            if (audioSource != null && stopOverheatSound != null)
            {
                audioSource.PlayOneShot(overheatSound);
            }
            currentEnergy = 0f;
        }
    }
}
