using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvas : MonoBehaviour
{
    public Transform cam;

    void Start()
    {
        cam = GameController.gameController.cam;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
