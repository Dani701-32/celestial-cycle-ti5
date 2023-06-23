using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeController", menuName = "DayNightSystem/TimeController")]
public class TimeControllerData : ScriptableObject
{
    [Header("Components:")]
    public Material skyMaterial;
    public AnimationCurve lightAngleCurve;

    [Header("Generic Hours:")]
    [Range(0, 24)]
    public float hour;

    [Range(-90, 90)]
    public float longitude;

    [Range(2, 22)]
    public float nightDuration = 12;

    [Range(0, 60)]
    public float dayMinutesDuration = 10;

    [Range(0.02f, 0.2f)]
    public float sunSize = 0.06f;

    [Header("Moon Controlls:")]
    [Range(0, 3)] public int phaseController = 0;
    public int day = 1;
    public float intensity;

    [Header("Color Controlls:")]
    public Color nightColor;
    public Color dayColor;
    public Color dawnColor;
    public Gradient gradientColor;
}
