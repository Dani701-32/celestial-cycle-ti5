using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Kappa : Enemy
{
    // Start is called before the first frame update
    public bool stuned = false;

    [Header("Disintegration Settings:")]
    public float dissolveSpeed = 1;
    private float timeDissolve = 0;
    [SerializeField]
    private Material dissolveMat;

    void Start()
    {
        timeDissolve = 0;
        dissolveMat.SetFloat(name = "_DissolveAmount", 0.0f);

        player = GameObject.FindWithTag("Player");
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        damageDealer = GetComponentInChildren<EnemyDamageDealer>();
    }
    public override bool CanSpawn(MoonPhases timeMoonphase){
        return timeMoonphase == moonPhase; 
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

    IEnumerator DissolveEffect()
    {
        while (timeDissolve <= 1)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            timeDissolve += (Time.deltaTime * dissolveSpeed);
            dissolveMat.SetFloat(name = "_DissolveAmount", timeDissolve);
        }

        timeDissolve = 1;
        Destroy(this.gameObject);
    }
    protected override void Die()
    {
        StartCoroutine(DissolveEffect());
    }
}
