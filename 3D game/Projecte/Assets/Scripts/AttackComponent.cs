using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    public GameObject weapon1Object; 
    public GameObject weapon2Object;
    public GameObject specialWeaponObject; //bomb, missile...

    IWeapon weapon1;
    IWeapon weapon2;
    IWeapon specialWeapon;

    void Start()
    {
        if(weapon1Object != null) weapon1 = weapon1Object.GetComponent<IWeapon>();
        if(weapon2Object != null) weapon2 = weapon2Object.GetComponent<IWeapon>();

        if(specialWeaponObject != null) specialWeapon = specialWeaponObject.GetComponent<IWeapon>();
    }

    public void BasicAttack()
    {
        if(weapon1 != null) weapon1.Activate();
        if(weapon2 != null) weapon2.Activate();
    } 

    public void SpecialAttack()
    {
        if (specialWeapon != null) specialWeapon.Activate();
    }

}

