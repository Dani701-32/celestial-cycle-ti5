using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController_Old : MonoBehaviour
{
    public static TimeController_Old InstanceTime;

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

    public bool isNight = false;

    [Header("Sky Material Controlls:")]
    [SerializeField] private AnimationCurve lightCurve;
    [SerializeField] private Material matSky;
    public float sunSize = 0.065f;
    [SerializeField] private float atmosphereThicknessDay, atmosphereThicknessNight;

    private float atmosphere;


    [Header("Day Color Controlls:")]
    [SerializeField] private Color daySkyLight;
    [SerializeField] private Color dayEquatorLight, dayGroundLight;

    [Header("Night Color Controlls:")]
    [SerializeField] private Color nightSkyLight;
    [SerializeField] private Color nightEquatorLight, nightGroundLight;


    [SerializeField] private List<Light> cityLights;

    private void Awake()
    {
        if (InstanceTime == null)
        {
            InstanceTime = this;
        }
        foreach (Light light in cityLights)
        {
            light.intensity = 0;
        }
    }


    // Start is called before the first frame update
    void Start()
    {

        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
        SetMoonPhase();

        atmosphere = atmosphereThicknessDay;

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

        atmosphere = Mathf.Lerp(atmosphereThicknessNight, atmosphereThicknessDay, lightCurve.Evaluate(dotProduct));
        matSky.SetFloat("_AtmosphereThickness", atmosphere);
        
  
        RenderSettings.ambientSkyColor = Color.Lerp(nightSkyLight, daySkyLight, lightCurve.Evaluate(dotProduct));
        RenderSettings.ambientEquatorColor = Color.Lerp(nightEquatorLight, dayEquatorLight, lightCurve.Evaluate(dotProduct));
        RenderSettings.ambientGroundColor = Color.Lerp(nightGroundLight, dayGroundLight, lightCurve.Evaluate(dotProduct));

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
            isNight = false;
            TimeSpan sunriseToSunset = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            matSky.SetFloat("_SunSize", sunSize);

            double porcent = timeSinceSunrise.TotalMinutes / sunriseToSunset.TotalMinutes;
            angle = Mathf.Lerp(0, 180, (float)porcent);
            // Rotate the moonLight to the opposite side of the sunLight
            moonLight.transform.rotation = Quaternion.AngleAxis(angle + 180f, Vector3.right);
        }
        else
        {
            isNight = true;
            TimeSpan sunsetToSunrise = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            matSky.SetFloat("_SunSize", 0);

            double porcent = timeSinceSunset.TotalMinutes / timeSinceSunset.TotalMinutes;
            angle = Mathf.Lerp(180, 360, (float)porcent);
            // Rotate the moonLight to the opposite side of the sunLight
            moonLight.transform.rotation = Quaternion.AngleAxis(angle - 180f, Vector3.right);
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
