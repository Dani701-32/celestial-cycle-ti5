using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactNewMoon : Artifact
{
    [SerializeField]
    private float enemyCooldown;

    [SerializeField]
    private float range = 8f;

    private void Update()
    {
        if (useArtifact)
        {
            int layerMask = 1 << 8;
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                range,
                transform.up,
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
                        StartCoroutine(enemy.EndEffect(enemyCooldown));
                        hasAffacted.Add(hit.transform.gameObject);
                    }
                }
            }
        }
        remaningCooldown = (remaningCooldown <= 0) ? 0 : remaningCooldown;
        if (remaningCooldown <= 0)
        {
            useArtifact = false;
        }
    }

    public override void Use()
    {
        if (remaningCooldown != 0)
            return;
        if (gameController.player.currentNewMoon <= 0)
            return;
        Instantiate(particle, gameController.player.transform);

        hasAffacted.Clear();
        remaningCooldown = cooldown;
        gameController.player.currentNewMoon -= cost;
        useArtifact = true;
        StartCoroutine(CountdownCoroutine());
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
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
