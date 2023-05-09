using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool dieInStun = false;
    public bool isAgressive = false;

    [SerializeField]
    EnemyType enemyType;

    public UnityEngine.AI.NavMeshAgent navMeshAgent; //  Nav mesh agent component
    public float startWaitTime = 4; //  Wait time of every action
    public float timeToRotate = 2; //  Wait time when the enemy detect near the player without seeing
    public float speedWalk = 6; //  Walking speed, speed in the nav mesh agent
    public float speedRun = 9; //  Running speed

    public float viewRadius = 15; //  Radius of the enemy view
    public float viewAngle = 90; //  Angle of the enemy view
    public LayerMask playerMask; //  To detect the player with the raycast
    public LayerMask obstacleMask; //  To detect the obstacules with the raycast
    public float meshResolution = 1.0f; //  How many rays will cast per degree
    public int edgeIterations = 4; //  Number of iterations to get a better performance of the mesh filter when the raycast hit an obstacule
    public float edgeDistance = 0.5f; //  Max distance to calcule the a minumun and a maximum raycast when hits something

    public Transform[] waypoints; 
    int m_CurrentWaypointIndex; 

    Vector3 playerLastPosition = Vector3.zero; 
    Vector3 m_PlayerPosition; 

    float m_WaitTime; 
    float m_TimeToRotate; 
    bool m_playerInRange; 
    bool m_PlayerNear; 
    bool m_IsPatrol;
    bool m_CaughtPlayer; 

    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_playerInRange = false;
        m_PlayerNear = false;
        m_WaitTime = startWaitTime; 
        m_TimeToRotate = timeToRotate;

        m_CurrentWaypointIndex = 0; 
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk; 
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    private void Update()
    {
        EnviromentView(); 

        if (!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    private void Chasing()
    {
        m_PlayerNear = false; 
        playerLastPosition = Vector3.zero; 

        if (!m_CaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition);
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (
                m_WaitTime <= 0
                && !m_CaughtPlayer
                && Vector3.Distance(
                    transform.position,
                    GameObject.FindGameObjectWithTag("Player").transform.position
                ) >= 6f
            )
            {
                
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else
            {
                if (
                    Vector3.Distance(
                        transform.position,
                        GameObject.FindGameObjectWithTag("Player").transform.position
                    ) >= 2.5f
                )
                    
                    Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    private void Patroling()
    {
        if (m_PlayerNear)
        {
            
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
               
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false; 
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position); 
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    private void OnAnimatorMove() { }

    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(
            transform.position,
            viewRadius,
            playerMask
        ); 

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position); 
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_playerInRange = true; 
                    m_IsPatrol = false;
                }
                else
                {

                    m_playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {

                m_playerInRange = false;
            }
            if (m_playerInRange)
            {
                m_PlayerPosition = player.transform.position;
            }
        }
    }
    void OnDrawGizmos()
{
    if (m_playerInRange)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, m_PlayerPosition);
    }
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
