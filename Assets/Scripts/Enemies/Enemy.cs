using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public bool dieInStun = false;
    public bool isAgressive = false;

    [Header("Status")]
    [SerializeField]
    private float health = 100;

    [SerializeField]
    private float attackCoulDown = 3f;

    [SerializeField]
    private float attackRange = 1f;

    [SerializeField]
    private float viewRange = 4f;

    private GameObject player;
    private Animator animator;
    private NavMeshAgent agent;

    [SerializeField]
    private EnemyType enemyType;

    private float timePassad;
    private float newDestnationCD = 0.5f;

    //Patrulha waypoints
    public Transform[] waypoints;
    int currentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
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


    private void Attack()
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

    public void TakeDamage(float damage)
    {
        health -= damage;
        animator.SetTrigger("damage");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    public bool CanSpawn(MoonPhases moonphase)
    {
        switch (enemyType)
        {
            case EnemyType.Kitsune:
                return (
                    moonphase == MoonPhases.FirstQuarter || moonphase == MoonPhases.ThirdQuarter
                )
                    ? true
                    : false;
            case EnemyType.Tengu:
                return (moonphase == MoonPhases.FullMoon) ? true : false;
            case EnemyType.Kappa:
                return (moonphase == MoonPhases.NewMoon) ? true : false;

            default:
                return true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRange);
    }

    public EnemyType GetEnemyType()
    {
        return enemyType;
    }
}

public enum AIState
{
    Idle,
    Patrol,
    Attack,
    Stunned
}

public enum EnemyType
{
    Karakasa,
    Humanoid,
    Kitsune,
    Tengu,
    Kappa,
}
