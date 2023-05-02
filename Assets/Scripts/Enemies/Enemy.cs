using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyType enemyType;
    public bool isAgressive = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Enemy started");
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isAgressive = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        isAgressive = false;
    }

    public EnemyType GetEnemyType() { return enemyType; }

}

public enum EnemyType
{
    Karakasa,
    Humanoid,
    Kitsune,
    Tengu,
    Kappa,
}