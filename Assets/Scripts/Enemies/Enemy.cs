using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    protected GameController gameController;
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
    protected float viewRange = 4f, spriteRange = 5f;
    [SerializeField] protected bool canReceiveDamage = false;

    protected GameObject player;

    [SerializeField]
    protected Animator animator;
    protected NavMeshAgent agent;

    [SerializeField]
    protected EnemyType enemyType;
    
    public MoonPhases moonPhase;

    [SerializeField] protected float timePassad;
    [SerializeField] protected float newDestnationCD = 0.5f;

    //Patrulha waypoints
    public Transform[] waypoints;
    [SerializeField] protected int currentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    public abstract void Attack();

    public abstract void TakeDamage(float damage);

    public bool CanSpawn(MoonPhases timeMoonphase)
    {
        return true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spriteRange);
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

    public abstract void ArtifactEffect(MoonPhases artifactMoon);
    public abstract IEnumerator  EndEffect(float timer);
    protected abstract void Die();

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
