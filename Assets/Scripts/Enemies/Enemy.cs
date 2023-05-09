using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isAgressive = false;

    //GPT
    public Transform[] waypoints;
    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 5.0f;
    public float stunDuration = 3.0f;
    public float attackRange = 2.0f;
    public float attackDelay = 2.0f;
    public float idleTime = 2.0f;
    public float sightRange = 10.0f;

    [SerializeField]
    EnemyType enemyType;

    [SerializeField]
    AIState currentState = AIState.Idle;

    [SerializeField]
    private int currentWaypoint = 0;
    private UnityEngine.AI.NavMeshAgent agent;
    private float stunTimer = 0.0f;
    private float attackTimer = 0.0f;
    private float idleTimer = 0.0f;

    [SerializeField]
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Enemy started");
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            default:
            case AIState.Idle:
                Idle();
                break;
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Attack:
                Attack();
                break;
            case AIState.Stunned:
                Stunned();
                break;
        }
    }

    private void Idle()
    {
        if (CanSeeTarget())
        {
            currentState = AIState.Attack;
            return;
        }
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTime)
        {
            idleTimer = 0.0F;
            currentState = AIState.Patrol;
        }
    }

    private void Patrol()
    {
        if (CanSeeTarget())
        {
            currentState = AIState.Attack;
            return;
        }
        if (waypoints == null){

            return;
        }
        agent.speed = patrolSpeed;
        agent.SetDestination(waypoints[currentWaypoint].position);

        // Check if we've reached the current waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) <= 1.0f)
        {
            // Move to the next waypoint
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    private void Attack() { }

    private void Stunned() { }

    private bool CanSeeTarget()
    {
        // Check if the player is within sight range
        if (Vector3.Distance(transform.position, target.transform.position) <= sightRange)
        {
            // Check if there's line of sight to the player
            RaycastHit hit;
            if (
                Physics.Raycast(
                    transform.position,
                    (target.transform.position - transform.position).normalized,
                    out hit
                )
            )
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, sightRange);
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
