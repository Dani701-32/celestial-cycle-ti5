using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    private TimeControllerManager timeController;
    public GameObject enemiePrefabs;
    public Vector3 size;

    private GameObject go_Enemy;
    [SerializeField]private Enemy enemy;
    private bool spawned;
    [SerializeField] private Transform[] waypoints;

    void Start()
    {
        timeController = TimeControllerManager.InstanceTime;
        enemy = enemiePrefabs.GetComponent<Enemy>();
        waypoints = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeController.isNight)
        {
            Spawn();
        }
        else
        {
            spawned = false;
            EndSpawn();
        }
    }

    public void Spawn()
    {
        if (!spawned && !go_Enemy)
        {
            if (!enemy.CanSpawn(timeController.GetCurrentPhase()))
                return;
            Vector3 pos = new Vector3(
                UnityEngine.Random.Range(-size.x / 2, size.x / 2),
                UnityEngine.Random.Range(-size.y / 2, size.y / 2),
                UnityEngine.Random.Range(-size.z / 2, size.z / 2)
            );
            GameObject obj = Instantiate(
                enemiePrefabs,
                pos + transform.position,
                Quaternion.identity
            );
            go_Enemy = obj;
            enemy = go_Enemy.GetComponent<Enemy>();
            enemy.waypoints = waypoints;
            spawned = true;
        }
    }

    private void EndSpawn()
    {
        if (this.go_Enemy == null)
            return;
        Enemy go_Enemy = this.go_Enemy.GetComponent<Enemy>();
        if (
            go_Enemy.GetEnemyType() == EnemyType.Karakasa
            || go_Enemy.GetEnemyType() == EnemyType.Humanoid
        )
            return;
        Destroy(this.go_Enemy);
        Debug.Log("EndSpawn");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(transform.position, size);
    }
}
