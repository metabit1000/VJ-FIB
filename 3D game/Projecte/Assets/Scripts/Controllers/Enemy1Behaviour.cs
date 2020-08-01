using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Behaviour : MonoBehaviour
{
	AttackComponent attackComponent;
	BasicMovement moveComponent;
	
	public Transform target;
    public float distance;

    Transform player;

    Vector3 initDirection;


    void Start()
    {
        attackComponent = this.GetComponent<AttackComponent>();
        moveComponent = this.GetComponent<BasicMovement>();

        var playerObject = GameObject.Find("PlayerShip");
        if(playerObject != null) player = playerObject.transform;

        initDirection = transform.forward.normalized;
    }


    void Update()
    {
        if (player == null || !IsInRange(player.position) || !IsInFront(player.position)) return;
    	
    	if (moveComponent != null)
        {
            moveComponent.LookAt(player.position);
            if (target != null) moveComponent.MoveTo(target.position);
            else moveComponent.MoveTo(transform.position + initDirection*100f);
        }

        if (attackComponent != null)
        {
            attackComponent.BasicAttack();
        }
    }


    bool IsInRange(Vector3 position)
    {
        return Mathf.Abs(transform.position.z - position.z) <= distance;
    }


    bool IsInFront(Vector3 position)
    {
        return transform.position.z > position.z;
    }
}
