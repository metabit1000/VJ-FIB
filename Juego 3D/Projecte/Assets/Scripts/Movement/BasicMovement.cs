using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public float speed = 15f;
    Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    
    public void MoveTo(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void LookAt(Vector3 position)
    {
        var rotation = Quaternion.LookRotation(position - transform.position).eulerAngles;
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, transform.eulerAngles.z);
    }
}
