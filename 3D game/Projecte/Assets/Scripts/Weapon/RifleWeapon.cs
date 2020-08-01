using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RifleWeapon : MonoBehaviour, IWeapon
{
    public float cooldown = 0.5f;
    public float shotSpeed = 20f;
    public int ammunition = -1; // -1 == infinite
    public GameObject prefabBullet;

    public Transform releasePoint;
    public Transform endPoint;


    float timeWithoutActivating = 0f;
    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }


    void Update()
    {
        timeWithoutActivating += Time.deltaTime;
    }


    public void Activate()
    {
        if (timeWithoutActivating < cooldown) return;
        timeWithoutActivating = 0f;

        Transform bulletTransform = Instantiate(prefabBullet).transform;
        bulletTransform.tag = transform.parent.gameObject.tag;

        bulletTransform.position = releasePoint.position;
        bulletTransform.LookAt(endPoint);
        bulletTransform.Rotate(new Vector3(90, 0, 0));

        Vector3 shotDirection = (endPoint.position - releasePoint.position).normalized;
        bulletTransform.GetComponent<Rigidbody>().velocity = shotDirection * shotSpeed;

        Destroy(bulletTransform.gameObject, 2f);    //destroy bullet after N seconds


        if (source != null)
        {
            source.pitch = Random.Range(0.85f, 1f);
            source.Play();
        }
    }

}
