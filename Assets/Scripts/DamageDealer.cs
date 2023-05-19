using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    bool canDamage = false;
    List<GameObject> hasDealtDamage;

    [SerializeField]
    float weapoLength;

    [SerializeField]
    float weaponDamage;

    // Start is called before the first frame update
    void Start()
    {
        canDamage = false;
        hasDealtDamage = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canDamage)
        {
            RaycastHit hit;
            int layerMask = 1 << 8;
            if (Physics.Raycast(transform.position, -transform.up, out hit, weapoLength, layerMask))
            {
                if (!hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    Debug.Log("damage");
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
            }
        }
    }

    public void StartDealDamage()
    {
        canDamage = true;
        hasDealtDamage.Clear();
    }

    public void EndDamage()
    {
        canDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weapoLength);
    }
}
