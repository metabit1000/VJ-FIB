using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public GameObject scope1Prefab;
    public GameObject scope2Prefab;

    public float crosshairDistance = 20;

    public Vector2 positionLimits; 

    private Transform crosshairNear;
    private Transform crosshairFar;
    private Transform mainCamera;

    void Start()
    {
        crosshairNear = Instantiate(scope1Prefab).transform;
        crosshairFar = Instantiate(scope2Prefab).transform;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    //TODO: Draw crosshair on top 
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = crosshairDistance;

        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePos);


        targetPosition.x = Mathf.Clamp(targetPosition.x, -positionLimits.x, positionLimits.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -positionLimits.y, positionLimits.y);

        crosshairFar.position = targetPosition;

        Vector3 dir = this.transform.position - crosshairFar.position;
        crosshairNear.position = crosshairFar.position + (dir.normalized * dir.magnitude* 0.4f);

        crosshairNear.transform.rotation = mainCamera.rotation;
        crosshairFar.transform.rotation = mainCamera.rotation;
    }

    public Vector3 GetAimPosition()
    {
        //si el mouse se fue a tomar por culo...
        if (crosshairFar.position.z <= transform.position.z) 
            return new Vector3(0, 0, transform.position.z + crosshairDistance);

        return crosshairFar.position;
    }


}
