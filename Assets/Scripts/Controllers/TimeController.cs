using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    [Header("Generic Hours")]
    [SerializeField] private TextMeshProUGUI textHours;
    [SerializeField] private TextMeshProUGUI textDays;
    [SerializeField] private TextMeshProUGUI textMoonPhase;
    [SerializeField] private float timeMultiplier, startHour;
    public int day = 1;
    private DateTime currentTime;

    [Header("Sun Controlls")]
    [SerializeField] private Light sunLight;
    [SerializeField] private float sunriseHour, sunsetHour, maxSunLightIntensity;
    private TimeSpan sunriseTime, sunsetTime;

    [Header("Moon Controlls")]
    [SerializeField] private Light moonLight;
    [SerializeField] private float maxMoonLightIntensity;
    [SerializeField] private MoonPhases currentPhase;
    private int phaseController = 0;

    [Header("Ambient Controlls")]
    [SerializeField] private AnimationCurve lightCurve;
    [SerializeField] private Color ambientDayLight, ambientNightLight;

    [SerializeField] private List<Light> cityLights;

    // Start is called before the first frame update

    void Awake()
    {
        foreach (Light light in cityLights)
        {
            light.intensity = 0;
        }
    }
    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
        SetMoonPhase();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();

    }

    private void UpdateTimeOfDay()
    {
        DateTime preciousTime = currentTime;
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
        if (textHours) textHours.text = currentTime.ToString("HH:mm");

        if (currentTime.Date != preciousTime.Date)
        {
            day++;
            if (day == 8)
            {
                day = 1;
                phaseController++;
                SetMoonPhase();
            }
            if (textDays) textDays.text = "Day " + day.ToString();
        }
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(ambientNightLight, ambientDayLight, lightCurve.Evaluate(dotProduct));

        foreach (Light light in cityLights)
        {
            light.intensity = Mathf.Lerp(2, 0, lightCurve.Evaluate(dotProduct));
        }
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan diff = toTime - fromTime;
        if (diff.TotalSeconds < 0)
        {
            diff += TimeSpan.FromHours(24);
        }
        return diff;
    }
    private void RotateSun()
    {
        float angle;
        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunset = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double porcent = timeSinceSunrise.TotalMinutes / sunriseToSunset.TotalMinutes;
            angle = Mathf.Lerp(0, 180, (float)porcent);
        }
        else
        {
            TimeSpan sunsetToSunrise = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double porcent = timeSinceSunset.TotalMinutes / timeSinceSunset.TotalMinutes;
            angle = Mathf.Lerp(180, 360, (float)porcent);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(angle, Vector3.right);
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
}



public enum MoonPhases
{
    NewMoon,
    FirstQuarter,
    FullMoon,
    ThirdQuarter,
}