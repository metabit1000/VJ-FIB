using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float time = 2f;

    float countDown;

    // Start is called before the first frame update
    void Start()
    {
        countDown = time;
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
        
        if (countDown <= 0f)
        	Destroy(this.gameObject);
    }
}
