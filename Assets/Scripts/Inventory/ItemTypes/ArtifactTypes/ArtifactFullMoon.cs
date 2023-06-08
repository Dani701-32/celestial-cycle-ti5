using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactFullMoon : Artifact
{
    [SerializeField]
    private float range = 5f;

    [SerializeField]
    private float cooldown = 5f;
    private float remaningCooldown = 0;

    private void Update()
    {
        if (useArtifact)
        {
            int layerMask = 1 << 8;
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                range,
                transform.forward,
                layerMask
            );
            if (hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    if (
                        hit.transform.TryGetComponent(out Enemy enemy)
                        && !hasAffacted.Contains(hit.transform.gameObject)
                    )
                    {
                        enemy.ArtifactEffect(artifactMoon);
                        hasAffacted.Add(hit.transform.gameObject);
                    }
                }
            }

            useArtifact = false;
        }
        remaningCooldown = (remaningCooldown <= 0) ? 0 : remaningCooldown;
    }

    public override void Use()
    {
        if (charge <= 0 && remaningCooldown != 0)
            return;
        Debug.Log("Usando artefato");
        hasAffacted.Clear();
        remaningCooldown = cooldown;
        useArtifact = true;
        StartCoroutine(CountdownCoroutine());
    }

    public override void Recharge()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator CountdownCoroutine()
    {
        while (remaningCooldown > 0f)
        {
            remaningCooldown -= Time.deltaTime;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (gameController == null)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gameController.player.transform.position, range);
    }
}
