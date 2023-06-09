using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySlash : MonoBehaviour
{
    public float delay = 0.6f;

    void Start()
    {
        Destroy(this.gameObject, delay);  
    }
}
