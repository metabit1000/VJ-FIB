using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public float smooth = 0.01f;
    public float distanceToPlayer = 10f;
    public float maxAngle = 10;
    Transform player;



    // Update camera position after the other objects positions has been updated
    void FixedUpdate()
    {
        if (player == null) FindPlayer();
        if (player == null) return;

        Vector3 targetPosition = new Vector3(0f, 0f, player.position.z - distanceToPlayer);
        //transform.position = targetPosition;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smooth);


        //rotate camera

        transform.rotation = Quaternion.Lerp(transform.rotation, player.rotation, smooth/4f);

        var angles = transform.rotation.eulerAngles;
        angles.x = CorrectedAngle(angles.x);
        angles.y = CorrectedAngle(angles.y);
        angles.z = 0;
        transform.eulerAngles = angles;
    }

    void FindPlayer()
    {
        var playerObject = GameObject.Find("PlayerShip");
        if (playerObject != null)
            player = playerObject.transform;
    }

    float CorrectedAngle(float angle) {

        if (angle > maxAngle && angle < 360 - maxAngle)
        {
            if ((angle - maxAngle) < (360 - maxAngle - angle))
                return maxAngle;
            else
                return (360 - maxAngle);
        }
        return angle;
    }

}
