using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    public bool dieInStun = false;
    public bool isAgressive = false;

    [Header("Status")]
    [SerializeField]
    protected float health = 100;

    [SerializeField]
    protected float attackCoulDown = 3f;

    [SerializeField]
    protected float attackRange = 1f;

    [SerializeField]
    protected float viewRange = 4f;

    protected GameObject player;

    [SerializeField]
    protected Animator animator;
    protected NavMeshAgent agent;

    [SerializeField]
    protected EnemyType enemyType;

    protected float timePassad;
    protected float newDestnationCD = 0.5f;

    //Patrulha waypoints
    public Transform[] waypoints;
     [SerializeField] protected int currentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    public abstract void Attack();

    public abstract void TakeDamage(float damage);

    protected void Die()
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

    public void StarDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().StarDealDamage();
    }

    public void EndDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().EndDealDamage();
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
