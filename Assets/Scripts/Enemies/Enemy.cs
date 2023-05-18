using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool dieInStun = false;
    public bool isAgressive = false;
    [SerializeField]
    EnemyType enemyType;

    //Patrulha waypoints
    public Transform[] waypoints; 
    int m_CurrentWaypointIndex; 

    Vector3 playerLastPosition = Vector3.zero; 
    Vector3 m_PlayerPosition; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

