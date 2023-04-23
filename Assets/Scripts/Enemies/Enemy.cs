using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyType type;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetType(int type)
    {
        switch (type)
        {
            default:
            case 0:
                this.type = EnemyType.Grunt;
                break;
            case 1:
                this.type = EnemyType.Agility;
                break;
            case 2:
                this.type = EnemyType.Medium;
                break;
            case 3:
                this.type = EnemyType.Tank;
                break;
        }
    }
}

public enum EnemyType
{
    Grunt,
    Medium,
    Agility,
    Tank,
}