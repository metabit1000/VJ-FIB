using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class LaserWeapon : MonoBehaviour, IWeapon
{
    public float dps;
    public float range;
    public float energyCostPerSecond;
    public Transform startPoint;
    public Transform endPoint;

    LineRenderer lineRenderer;
    bool isActivated;
    string ownerTag;
    Energy energy;

    AudioSource source;

    public void Activate()
    {
        isActivated = true;
    }


    void Start()
    {
        ownerTag = this.transform.parent.tag;
        energy = GetComponentInParent<Energy>();

        isActivated = false;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;

        // laser no visible
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, startPoint.position);

        source = GetComponent<AudioSource>();
    }


    void LateUpdate()
    {
        if (isActivated && (energy == null || energy.Consume(energyCostPerSecond * Time.deltaTime) ) )
        {
            EmitLaser();
            if (source != null && !source.isPlaying) source.Play();
        }
            
        else
        {
            lineRenderer.SetPosition(0, startPoint.position);
            lineRenderer.SetPosition(1, startPoint.position);
            if (source != null) source.Stop();
        }
        isActivated = false;

    }


    //TODO: Ignore owner of the laser, desactive beam when not calling to Activate
    void EmitLaser()
    {
        lineRenderer.SetPosition(0, startPoint.position);
        Vector3 laserDirection = (endPoint.position - startPoint.position).normalized;
        RaycastHit hit;


        if (Physics.Raycast(startPoint.position, laserDirection, out hit, range))
        {
            Health health = hit.transform.GetComponent<Health>();
            if (health != null && !hit.transform.CompareTag(ownerTag) && (ownerTag != "Enemy" || hit.transform.CompareTag("Player")))
            {
                health.TakeDamage(dps * Time.deltaTime);
            }
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, startPoint.position + range * laserDirection);
        }
    }
}
