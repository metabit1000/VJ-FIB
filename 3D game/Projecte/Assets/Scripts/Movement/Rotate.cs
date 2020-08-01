using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(20,20,20);


    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
