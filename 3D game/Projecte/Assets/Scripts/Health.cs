using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public float maxHealth = 20.0f;
    public float healHealth = 10.0f;

    public event Action OnDeath;

    float currentHealth;
    bool bulletImmunity;
    bool godMode;
    bool isDead;

    public GameObject explosion;


    void Start()
    {
        currentHealth = maxHealth;
        bulletImmunity = false;
        godMode = false;
        isDead = false;
    }


    public void TakeDamage(float damage)
    {
        if (godMode) return;

        currentHealth -= damage;

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            OnDeath?.Invoke();
            
            if (explosion != null) {
                GameObject aux = Instantiate(explosion);
                aux.transform.position = this.transform.position;
            }
            
            Destroy(this.gameObject);
        }
    }


    public void Heal()
    {
        currentHealth = Mathf.Min(currentHealth + healHealth, maxHealth);
    }

    public float GetCurrentHealth() => currentHealth;

    public float GetMaxHealth() => maxHealth;

    public bool GetBulletImmunity() => bulletImmunity;

    public void SetBulletImmunity(bool value) => bulletImmunity = value;

    public void SetGodMode(bool value) => godMode = value;


    // check for bullet impacts (bullet = object with damage component)
    private void OnTriggerEnter(Collider other)
    {
        var damageComponent = other.gameObject.GetComponent<BulletDamage>();
        if (damageComponent != null && !this.CompareTag(other.tag) && !bulletImmunity && !damageComponent.GetHasDamaged())
        {
            damageComponent.SetHasDamaged(true);
            TakeDamage(damageComponent.GetDamage());
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherObject = collision.gameObject;

        if (godMode && otherObject.CompareTag("Static")) StartCoroutine(ColliderDesactivation());
        else if (CompareTag("Player"))
        {
            if (collision.transform.CompareTag("Static"))
            {
                TakeDamage(100f);
            }
            else if (collision.transform.CompareTag("Enemy"))
            {
                var collisionDamage = otherObject.GetComponent<CollisionDamage>();
                if(collisionDamage != null) TakeDamage(collisionDamage.GetDamage());

                var enemyHealth = otherObject.GetComponent<Health>();
                if (enemyHealth != null) enemyHealth.TakeDamage(200f);
            }
        }
    }


    private IEnumerator ColliderDesactivation()
    {
        float desactivationTime = 1f;
        float t = 0;

        transform.Find("Colliders").gameObject.SetActive(false);
        while (t < desactivationTime)
        {
            yield return null;
            t += Time.deltaTime;
        }
        transform.Find("Colliders").gameObject.SetActive(true);
    }
}
