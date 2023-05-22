using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Human : Enemy
{
    private bool isFoward = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        isFoward = true;
        currentWaypointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);
        Attack();
        if (newDestnationCD <= 0)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= viewRange)
            {
                newDestnationCD = 0.5f;
                agent.SetDestination(player.transform.position);
                transform.LookAt(player.transform);
            }
            else
            {
                newDestnationCD = 4f;
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
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

    public override void TakeDamage(float damage)
    {
        health -= damage;
        animator.SetTrigger("damage");

        if (health <= 0)
        {
            Die();
        }
    }
}