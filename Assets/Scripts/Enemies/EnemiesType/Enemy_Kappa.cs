using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Kappa : Enemy
{
    // Start is called before the first frame update
    public bool stuned = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        damageDealer = GetComponentInChildren<EnemyDamageDealer>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);
        Attack();
        if (
            newDestnationCD <= 0
            && Vector3.Distance(player.transform.position, transform.position) <= viewRange
        )
        {
            newDestnationCD = 0.5f;
            agent.SetDestination(player.transform.position);
            transform.LookAt(player.transform);
        }
        newDestnationCD -= Time.deltaTime;
    }

    public override void Attack()
    {
        if (player.GetComponent<Player>().IsDead())
            return;
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

    public override void TakeDamage(float damage)
    {
        if (!canReceiveDamage)
            return;
        health -= damage;
        animator.SetTrigger("damage");

        if (health <= 0)
        {
            Die();
        }
    }

    public override void ArtifactEffect(MoonPhases artifactMoon)
    {
        if (moonPhase == artifactMoon)
        {
            animator.SetTrigger("damage");
            canReceiveDamage = true;
        }
    }

    public override IEnumerator EndEffect(float timer)
    {
        Debug.Log("Iniciando corrotina");
        yield return new WaitForSeconds(timer);
        canReceiveDamage = false;
    }

    protected override void Die()
    {
        Destroy(this.gameObject);
    }
}
