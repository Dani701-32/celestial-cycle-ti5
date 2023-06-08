using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    public List<Slash> slashes;

    public void Slash()
    {
        StartCoroutine(SlashAttackSystem());
    }

    IEnumerator SlashAttackSystem()
    {
        for (int i = 0; i < slashes.Count; i++)
        {
            yield return new WaitForSeconds(slashes[i].delay);
            GameObject obj = Instantiate(slashes[i].slashVFX, slashes[i].spawn.position, slashes[i].spawn.rotation);
        }
    }
}

[System.Serializable]
public class Slash
{
    public GameObject slashVFX;
    public Transform spawn;
    public float delay;
}
