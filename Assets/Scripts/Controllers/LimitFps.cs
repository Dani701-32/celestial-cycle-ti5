using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFps : MonoBehaviour
{
    public enum limits
    {
        noLimit = -1,
        limit30 = 30,
        limit60 = 60,
        limit120 = 120,
        limit240 = 240,
    }

    public bool limiterFps;
    public limits limit;
    
    private void Awake()
    {
        if (limiterFps) Application.targetFrameRate = (int)limit;
        else Application.targetFrameRate = Screen.currentResolution.refreshRate;        
    }

    //void Start()
    //{
    //    //Application.targetFrameRate = 60;
    //    Application.targetFrameRate = Screen.currentResolution.refreshRate;
    //}
}
