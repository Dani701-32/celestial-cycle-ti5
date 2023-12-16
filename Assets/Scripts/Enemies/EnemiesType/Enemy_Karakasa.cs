using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Karakasa : Enemy
{
   [SerializeField]
    private bool isFoward = true;


    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.gameController;
        player = GameObject.FindWithTag("Player");
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        isFoward = true;
        currentWaypointIndex = 0;
        damageDealer = GetComponentInChildren<EnemyDamageDealer>();
    }
    public override bool CanSpawn(MoonPhases timeMoonphase){
        return true; 
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameController.dayNightController.isNight){
            Die();
        }
        Attack();
        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);
        if (
            newDestnationCD <= 0
            || Vector3.Distance(player.transform.position, transform.position) <= viewRange
        )
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= viewRange)
            {
                agent.stoppingDistance = 1.5f;

                newDestnationCD = 0.5f;
                agent.SetDestination(player.transform.position);
                transform.LookAt(player.transform);
            }
            else
            {
                timePassad = 0f;
                newDestnationCD = 4f;
                agent.stoppingDistance = .3f;
                if (agent.remainingDistance < 0.5f)
                {
                    if (isFoward)
                    {
                        currentWaypointIndex++;
                        if (currentWaypointIndex >= waypoints.Length)
                        {
                            currentWaypointIndex = waypoints.Length - 2;
                            isFoward = false;
                        }
                    }
                    else
                    {
                        currentWaypointIndex--;
                        if (currentWaypointIndex < 0)
                        {
                            currentWaypointIndex = 1;
                            isFoward = true;
                        }
                    }
                    agent.SetDestination(waypoints[currentWaypointIndex].position);
                }
            }
        }
        newDestnationCD -= Time.deltaTime;
    }

    public override void Attack()
    {
        if (!player.GetComponent<Player>().IsDead())
        {
            if (timePassad >= attackCoulDown)
            {
                if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
                {
                    animator.SetTrigger("attack");
                    timePassad = 0;
                }
            }
            timePassad += Time.deltaTime;
        }
    }

    public override void ArtifactEffect(MoonPhases artifactMoon)
    {
        if (moonPhase == artifactMoon)
        {
            animator.SetTrigger("damage");
            // canReceiveDamage = true;
        }
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        animator.SetTrigger("damage");

        if (health <= 0)
        {
            Die();
        }
    }

    public override IEnumerator EndEffect(float timer)
    {
        Debug.Log("Iniciando corrotina");
        yield return new WaitForSeconds(timer);
        // canReceiveDamage = false;
    }

    protected override void Die()
    {
        Destroy(this.gameObject);
    }
}
