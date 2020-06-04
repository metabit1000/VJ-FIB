using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AlwaysShooting : MonoBehaviour
{

    private IWeapon[] weapons; 


    void Start()
    {
        weapons = GetComponentsInChildren<IWeapon>();
    }


    void Update()
    {
        foreach (var weapon in weapons)
            weapon.Activate();
    }
}
