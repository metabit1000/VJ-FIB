using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitZtransform : MonoBehaviour
{
    void Update()
    {
        if (transform.position.z >= 100000f)
        {
            var newPosition = transform.position;
            newPosition.z = 0;
            transform.position = newPosition;
        }
    }
}
