﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

[RequireComponent(typeof(Light))]
public class TimeControllerManager : MonoBehaviour, ISaveable
{
    public static TimeControllerManager InstanceTime;
    public TimeControllerData timeControllerData;

    public SavingLoading savingLoading;

    private Material skyMaterial;
    private AnimationCurve lightAngleCurve;
    private Light sunLight;

    private int phaseController, day;
    private float hour, longitude, nightDuration;
    private float intensity, sunSize, dayMinutesDuration, lastNightDuration = -1;
    private float dt = 0, hourEval, gray, lastLongitude = 1000, lastHour = -1;

    private MoonPhases currentPhase;
    [HideInInspector]
    public bool isNight;
    private bool isSaveNight = false, switchDay = false;
    
    private Color dayColor, dawnColor, nightColor, lightColor;
    private Gradient gradientColor;

    private Transform mainCamera;
    private DateTime currentTime;

    [Header("UI:")]
    public TextMeshProUGUI textHours;
    public TextMeshProUGUI textDays, textMoonPhase;

    [SerializeField]
    private List<Light> cityLights;

    private void Awake()
    {
        if (InstanceTime == null)
        {
            InstanceTime = this;
        }

        sunLight = this.GetComponent<Light>();
        mainCamera = Camera.main.transform;
    }

    void Start()
    {
        foreach (Light light in cityLights)
        {
            light.intensity = 0;
        }

        //if(!savingLoading.StatusFile())
        //{
        //    InitializeVariables();
        //}

        InitializeVariables();

        SetMoonPhase();

        textDays.text = "Day " + day.ToString();

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.sun = sunLight;
        RenderSettings.skybox = skyMaterial;
        RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Skybox;
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(hour);
        lastNightDuration = -1;
    }

    private void ControlLightsCity(float min, float max)
    {
        foreach (Light light in cityLights) light.intensity = Mathf.Lerp(min, max, lightAngleCurve.Evaluate(hour));
    }

    public void Update()
    {
        lastHour = hour;
        lastLongitude = longitude;
        dt = Time.deltaTime;
        hour += dt * 24.0f / (60 * dayMinutesDuration);

        UpdateTimeOfDay();

        if (lastNightDuration != nightDuration)
        {
            lastNightDuration = nightDuration;
            UpdateGradients();
        }

        if (isNight)
        {
            ControlLightsCity(0, 2);
            skyMaterial.SetFloat("_SunSize", 0.0f);
        }
        else
        {
            ControlLightsCity(2, 0);
            skyMaterial.SetFloat("_SunSize", sunSize);
        }

        if (hour > 24)
        {
            hour -= 24;
            switchDay = true;
        }
        else switchDay = false;

        isNight = hour < nightDuration / 2 || hour > 24 - (nightDuration / 2);

        hourEval = lightAngleCurve.Evaluate(hour);

        sunLight.transform.position = mainCamera.position;
        sunLight.transform.localEulerAngles = new Vector3(hourEval, 0, 0);
        sunLight.transform.Rotate(Vector3.up, longitude);

        if (hour > 12)
        {
            sunLight.transform.localEulerAngles = new Vector3(sunLight.transform.localEulerAngles.x, sunLight.transform.localEulerAngles.y + 180, 0);

            sunLight.transform.localEulerAngles = new Vector3(sunLight.transform.localEulerAngles.x, -sunLight.transform.localEulerAngles.y, 0);
        }

        lightColor = gradientColor.Evaluate(hour / 24.0f);
        sunLight.color = lightColor;

        gray = lightColor.grayscale;

        RenderSettings.ambientIntensity = gray;
        skyMaterial.SetFloat("_Exposure", gray * 1.3f);
        skyMaterial.SetColor("_SkyTint", lightColor);
        skyMaterial.SetColor("_GroundColor", lightColor * 0.5f);

        sunLight.shadows = isNight ? LightShadows.None : LightShadows.Soft;
        sunLight.shadowStrength = gray;

        RenderSettings.ambientSkyColor = lightColor * 0.85f;
        RenderSettings.ambientEquatorColor = lightColor * 0.5f;
        RenderSettings.ambientGroundColor = lightColor * 0.015f;

        intensity = isNight ? 0 : gray;
    }

    public void UpdateGradients()
    {
        //light color --------
        GradientColorKey[] gradientColorKeys = new GradientColorKey[6];
        gradientColorKeys[0].color = nightColor;
        gradientColorKeys[1].color = dawnColor;
        gradientColorKeys[2].color = dayColor;
        gradientColorKeys[3].color = dayColor;
        gradientColorKeys[4].color = dawnColor;
        gradientColorKeys[5].color = nightColor;

        float startHour = nightDuration / 48.0f;

        float dawnTime = 1.5f / 24.0f / 2.0f; //1 hour

        gradientColorKeys[0].time = startHour;
        gradientColorKeys[1].time = startHour + dawnTime;
        gradientColorKeys[2].time = startHour + (dawnTime * 2);
        gradientColorKeys[3].time = (1 - startHour) - (dawnTime * 2);
        gradientColorKeys[4].time = (1 - startHour) - dawnTime;
        gradientColorKeys[5].time = (1 - startHour);

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1;
        alphaKeys[0].time = 0;
        alphaKeys[1].alpha = 1;
        alphaKeys[1].time = 1;

        gradientColor = new Gradient { colorKeys = gradientColorKeys, alphaKeys = alphaKeys };

        //light angle --------
        Keyframe[] keys = lightAngleCurve.keys;
        keys[1].time = nightDuration / 2;
        keys[3].time = 24 - nightDuration / 2;
        lightAngleCurve.keys = keys;

        for (int i = 0; i < lightAngleCurve.keys.Length; i++)
        {
            AnimationUtility.SetKeyBroken(lightAngleCurve, i, true);
            AnimationUtility.SetKeyLeftTangentMode(lightAngleCurve, i, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(lightAngleCurve, i, AnimationUtility.TangentMode.Linear);
        }
    }

    public void UpdateTimeOfDay()
    {
        DateTime preciousTime = currentTime;
        currentTime = currentTime.AddSeconds(Time.deltaTime * 24 * (60 / dayMinutesDuration));
        if (textHours) textHours.text = currentTime.ToString("HH:mm");

        if (isNight && !isSaveNight && switchDay)
        {
            isSaveNight = true;
            day++;
            if (day == 8)
            {
                day = 1;
                phaseController++;
                SetMoonPhase();
            }
            if (textDays) textDays.text = "Day " + day.ToString();
        }

        if (!isNight && isSaveNight) isSaveNight = false;
    }

    private void SetMoonPhase()
    {
        if (phaseController > 3)
        {
            phaseController = 0;
        }
        switch (phaseController)
        {
            case 0:
                currentPhase = MoonPhases.NewMoon;
                break;
            case 1:
                currentPhase = MoonPhases.FirstQuarter;
                break;
            case 2:
                currentPhase = MoonPhases.FullMoon;
                break;
            case 3:
                currentPhase = MoonPhases.ThirdQuarter;
                break;
        }

        textMoonPhase.text = currentPhase.ToString();
    }

    public MoonPhases GetCurrentPhase()
    {
        return this.currentPhase;
    }

    public void InitializeVariables()
    {
        skyMaterial = timeControllerData.skyMaterial;
        lightAngleCurve = timeControllerData.lightAngleCurve;
        hour = timeControllerData.hour;
        longitude = timeControllerData.longitude;
        nightDuration = timeControllerData.nightDuration;
        dayMinutesDuration = timeControllerData.dayMinutesDuration;
        sunSize = timeControllerData.sunSize;
        phaseController = timeControllerData.phaseController;
        currentPhase = timeControllerData.currentPhase;
        day = timeControllerData.day;
        intensity = timeControllerData.intensity;
        nightColor = timeControllerData.nightColor;
        dayColor = timeControllerData.dayColor;
        dawnColor = timeControllerData.dawnColor;
        gradientColor = timeControllerData.gradientColor;
    }

    public object CaptureState()
    {
        return new SaveData
        {
            s_hour = hour,
            s_currentPhase = currentPhase,
            s_phaseController = phaseController,
            s_day = day
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        hour = saveData.s_hour;
        currentPhase = saveData.s_currentPhase;
        phaseController = saveData.s_phaseController;
        day = saveData.s_day;
    }

    [Serializable]
    private struct SaveData
    {
        public float s_hour;
        public MoonPhases s_currentPhase;
        public int s_phaseController;
        public int s_day;
    }
}

public enum MoonPhases
{
    NewMoon,
    FirstQuarter,
    FullMoon,
    ThirdQuarter,
}





