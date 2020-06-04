using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// TODO: improve shit code
public class Formation : MonoBehaviour
{
    public float distanceToPlayer = 20f;
    public float distanceBetweenMembers = 4f;
    public int formationLayers = 4;

    public float rotationSpeed = 5f;
    public float timeBetweenAttacks = 2f; 

    Dictionary<Transform, Transform> memberToFormationTarget;
    Dictionary<Transform, bool> targetOccupied;
    Transform player;

    public event Action OnFormationAttack;
    float timeSinceLastAttack = 0f;


    public void AddMember(Transform member)
    {
        if (!memberToFormationTarget.ContainsKey(member))
            memberToFormationTarget[member] = null;
    }


    public void DeleteMember(Transform member)
    {
        if (memberToFormationTarget.ContainsKey(member))
        {
            var target = memberToFormationTarget[member];
            memberToFormationTarget.Remove(member);
            if (target != null) targetOccupied[target] = false;
        }
    }


    public bool IsOnPosition(Transform member)
    {
        const float offset = 10f;
        if (memberToFormationTarget.ContainsKey(member))
        {
            var target = memberToFormationTarget[member];
            if (target != null) return Vector3.Distance(target.position, member.position) < offset;
        }
        return false;
    }


    public Transform GetTargetPosition(Transform member)
    {
        return memberToFormationTarget[member];
    }


    private void Start()
    {
        memberToFormationTarget = new Dictionary<Transform, Transform>();
        targetOccupied = new Dictionary<Transform, bool>();

        player = GameObject.Find("PlayerShip").transform;

        CreateFormationTransforms();
    }


    private void Update()
    {
        if (player == null) return;
        transform.position = new Vector3(0, 0, player.position.z + distanceToPlayer);

        List<Transform> members = new List<Transform>();

        foreach (var member in memberToFormationTarget.Keys)
            members.Add(member);

        foreach (var member in members)
            UpdateMemberTarget(member);
       
        transform.Rotate(new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);


        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack >= timeBetweenAttacks)
        {
            OnFormationAttack?.Invoke();
            timeSinceLastAttack = 0f;
        }
    }


    private void UpdateMemberTarget(Transform member)
    {
        var currentTarget = memberToFormationTarget.ContainsKey(member) ? memberToFormationTarget[member] : null;

        Transform bestTarget = currentTarget;
        float bestDistanceToCenter = (bestTarget == null) ? float.MaxValue : bestTarget.localPosition.magnitude;

        foreach (var valueKey in targetOccupied)
        {
            Transform target = valueKey.Key;
            bool occupied = valueKey.Value;

            float distance = target.localPosition.magnitude;
            if (distance < bestDistanceToCenter && !occupied)
            {
                bestTarget = target;
            }
        }
        memberToFormationTarget[member] = bestTarget;
        if (currentTarget != null) targetOccupied[currentTarget] = false; //free formation target
        if (bestTarget != null) targetOccupied[bestTarget] = true; //occupy formation target
    }


    private void CreateFormationTransforms()
    {
        //Transform childTransformInit = new GameObject().transform;
        //childTransformInit.parent = transform;
        //childTransformInit.localPosition = new Vector3(0, 0, 0);
        //targetOccupied.Add(childTransformInit, false);

        for (int i = 1; i <= formationLayers; i++)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if ((x != 0 || y != 0))
                    {
                        Transform childTransform = new GameObject().transform;
                        childTransform.parent = transform;
                        childTransform.localPosition = new Vector3(x * distanceBetweenMembers * i, y * distanceBetweenMembers * i, 0);
                        targetOccupied.Add(childTransform, false);
                    }
                }
            }
        }

    }
}
