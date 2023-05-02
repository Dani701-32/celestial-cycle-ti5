using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    private TimeController timeController;
    public GameObject enemiesPrefabs;
    public Vector3 size;

    [SerializeField] private GameObject enemy;

    [SerializeField] private bool spawned;

    void Start()
    {
        timeController = TimeController.InstanceTime;
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
        if (!spawned)
        {
            Vector3 pos = new Vector3(
                UnityEngine.Random.Range(-size.x / 2, size.x / 2),
                UnityEngine.Random.Range(-size.y / 2, size.y / 2),
                UnityEngine.Random.Range(-size.z / 2, size.z / 2)
            );
            GameObject obj = Instantiate(
                enemiesPrefabs,
                pos + transform.position,
                Quaternion.identity
            );
            enemy = obj;
            Debug.Log("Spawn");
            spawned = true;
        }
    }

    private void EndSpawn()
    {
        if (enemy == null) return;
        Destroy(enemy);
        Debug.Log("EndSpawn");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawCube(transform.position, size);
    }
}
