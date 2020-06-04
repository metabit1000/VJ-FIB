using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Sounds")]
    public AudioSource audio;
    public AudioClip supplySound;
    public AudioClip winSound;

    PlayerMovement moveComponent;
    Crosshair aimComponent;
    AttackComponent attackComponent;
    Health health;
    Energy energy;

    float doubleTapTime;
    float timeSinceLastTap_W;
    float timeSinceLastTap_S;
    float timeSinceLastTap_A;
    float timeSinceLastTap_D;


    GameMenuManager gameMenuManager;


    void Start()
    {
        moveComponent = this.GetComponent<PlayerMovement>();
        aimComponent = this.GetComponent<Crosshair>();
        attackComponent = this.GetComponent<AttackComponent>();
        health = GetComponent<Health>();
        energy = GetComponent<Energy>();

        doubleTapTime = 1f;
        timeSinceLastTap_W = doubleTapTime;
        timeSinceLastTap_S = doubleTapTime;
        timeSinceLastTap_A = doubleTapTime;
        timeSinceLastTap_D = doubleTapTime;

        if (health != null) health.OnDeath += OnDeath;

        GameObject managerObject = GameObject.FindGameObjectWithTag("Manager");
        if (managerObject != null) gameMenuManager = managerObject.GetComponent<GameMenuManager>();
    }


    void Update()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 1);
        Vector3 objective = Vector3.zero;


        if (aimComponent != null && aimComponent.enabled)
        {
            objective = aimComponent.GetAimPosition();
            moveDirection = (objective - transform.position).normalized;
            moveDirection.x *= 2f;
            moveDirection.y *= 2f;
        }

        if (moveComponent != null && moveComponent.enabled)
        {
            moveComponent.Move(moveDirection);
            moveComponent.LookAt(objective);

            if (Input.GetButton("Boost"))
            {
                moveComponent.Boost();
            }
            else if (Input.GetButtonUp("Boost"))
            {
                moveComponent.StopBoosting();
            }

            if (Input.GetButton("SlowDown"))
            {
                moveComponent.SlowDown();
            }
            else if (Input.GetButtonUp("SlowDown"))
            {
                moveComponent.StopSlowingDown();
            }

            if (Input.GetKeyDown("w"))
            {
                if (timeSinceLastTap_W < doubleTapTime)
                    moveComponent.Roll(Vector2.up);
                timeSinceLastTap_W = 0;
            }
            if (Input.GetKeyDown("s"))
            {
                if (timeSinceLastTap_S < doubleTapTime)
                    moveComponent.Roll(Vector2.down);
                timeSinceLastTap_S = 0;
            }

            if (Input.GetKeyDown("a"))
            {
                if (timeSinceLastTap_A < doubleTapTime)
                    moveComponent.Roll(Vector2.left);

                moveComponent.StartTilting(-1);
                timeSinceLastTap_A = 0;
            }
            else if (Input.GetKeyUp("a"))
            {
                moveComponent.StopTilting();
            }

            if (Input.GetKeyDown("d"))
            {
                if (timeSinceLastTap_D < doubleTapTime)
                    moveComponent.Roll(Vector2.right);
                
                moveComponent.StartTilting(1);
                timeSinceLastTap_D = 0;
            }
            else if (Input.GetKeyUp("d"))
            {
                moveComponent.StopTilting();
            }

            timeSinceLastTap_W += Time.deltaTime;
            timeSinceLastTap_S += Time.deltaTime;
            timeSinceLastTap_A += Time.deltaTime;
            timeSinceLastTap_D += Time.deltaTime;
        }

        if (attackComponent != null && attackComponent.enabled)
        {
            if (Input.GetButton("Fire1"))
            {
                attackComponent.BasicAttack();
            }
            if (Input.GetButton("Fire2"))
            {
                attackComponent.SpecialAttack();
            }
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("HealthSupply")) {
            health.Heal();
            audio.PlayOneShot(supplySound,0.7f);

            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
        
        else if (other.gameObject.CompareTag("EnergySupply")) {
            energy.Recharge();
            other.GetComponent<ScorePoints>()?.AddScore();
            audio.PlayOneShot(supplySound,0.7f);

            other.gameObject.SetActive(false);  //onTrigger is called more than one time before objects is destroyed?? desactivation needed
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("Win"))
        {
            if (gameMenuManager != null) gameMenuManager.ViewWinMenu();
            audio.PlayOneShot(winSound,0.7f);
        }
    }


    private void OnDeath()
    {
        gameMenuManager.ViewLoseMenu();
    }


    private void OnDestroy()
    {
        if (health != null) health.OnDeath -= OnDeath;

    }
}
