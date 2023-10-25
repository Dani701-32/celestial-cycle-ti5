using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class DayNightCycle : MonoBehaviour, ISaveable
{
    public static DayNightCycle InstanceTime;
    public bool continueDayNight;
    private DateTime currentTime;
    private MoonPhases currentPhase;
    private bool isSaveNight = false, switchDay = false;
    private float hour;
    private TimeSpan sunriseTime, sunsetTime;


    public float timeMultiplier;

    public Light sunLight;

    [SerializeField] private Material skyboxMat;

    public AnimationCurve lightChangeCurve;

    public float maxSunLightIntensity, maxMoonLightIntensity, sunriseHour, sunsetHour;

    public bool isNight;

    public Color dayColor, nightColor;

    [Header("Post-Processing:")]
    public Volume nightPost;
   
    [Header("Fog:")]
    public float fogDensity = 0.0035f;
    public Color dayfogColor, nightfogColor;

    [Header("UI:")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI textDays, textMoonPhase;

    [Header("Save Variables")]
    public float startHour;
    public int day, phaseController;

    [SerializeField]
    private List<Light> cityLights;

    private void Awake()
    {
        if (InstanceTime == null)
        {
            InstanceTime = this;
        }
    }

    void Start()
    {
        SetMoonPhase();
        continueDayNight = true;
        textDays.text = "Dia " + day.ToString();
       
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        hour = startHour;

        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);

        RenderSettings.sun = sunLight;
        RenderSettings.skybox = skyboxMat;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = fogDensity;  
    }
    void Update()
    {
        if(continueDayNight)
        {
            UpdateTimeOfDay();
            RotateSun();
            UpdateLightSettings();

            if (hour == 0) switchDay = true;
            else switchDay = false;
            hour = currentTime.Hour;
        }
        
    }

    private void ControlLightsCity(float min, float max)
    {
        foreach (Light light in cityLights) light.intensity = Mathf.Lerp(min, max, 0.6f);
    }

    void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        if(timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }

        sunLight.shadows = isNight ? LightShadows.None : LightShadows.Soft;

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
            if (textDays) textDays.text = "Dia " + day.ToString();
            Debug.Log("Chamou o UpdateTimeOfDay");
        }

        if (!isNight && isSaveNight) isSaveNight = false;
    }

    private void RotateSun()
    {
        float sunLightRotation;

        if(currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
            isNight = false;
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
            isNight = true;
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
        skyboxMat.SetVector(name = "_MainLightDirection", sunLight.transform.forward);
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);

        if(!isNight)
        {
            sunLight.color = dayColor;
            sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
            ControlLightsCity(2, 0);
        }
        else
        {
            sunLight.color = nightColor;
            sunLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
            ControlLightsCity(0, 2);
        }

        RenderSettings.fogColor = Color.Lerp(nightfogColor, dayfogColor, lightChangeCurve.Evaluate(dotProduct));
        sunLight.color = Color.Lerp(nightColor, dayColor, lightChangeCurve.Evaluate(dotProduct));
        nightPost.weight = Mathf.Lerp(1, 0, lightChangeCurve.Evaluate(dotProduct));
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if(difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }


    private void SetMoonPhase()
    {
        string currentMoonPhase = "";
        if (phaseController > 3)
        {
            phaseController = 0;
        }
        switch (phaseController)
        {
            case 0:
                currentPhase = MoonPhases.NewMoon;
                currentMoonPhase = "Lua Nova";
                break;
            case 1:
                currentPhase = MoonPhases.FirstQuarter;
                currentMoonPhase = "Lua Crescente";
                break;
            case 2:
                currentPhase = MoonPhases.FullMoon;
                currentMoonPhase = "Lua Cheia";
                break;
            case 3:
                currentPhase = MoonPhases.ThirdQuarter;
                currentMoonPhase = "Lua Minguante";
                break;
        }

        textMoonPhase.text = currentMoonPhase;
    }

    public MoonPhases GetCurrentPhase()
    {
        return this.currentPhase;
    }


    public object CaptureState()
    {
        return new SaveData
        {
            s_day = day,
            s_phaseController = phaseController,
            s_startHour = hour
        };
    }

    public void RestoreState(object state)
    {
        var saveData = (SaveData)state;

        startHour = saveData.s_startHour;
        day = saveData.s_day;
        phaseController = saveData.s_phaseController;   
    }

    [Serializable]
    public struct SaveData
    {
        public float s_startHour;
        public int s_day, s_phaseController;   
    }
}

public enum MoonPhases
{
    NewMoon,
    FirstQuarter,
    FullMoon,
    ThirdQuarter,
}


