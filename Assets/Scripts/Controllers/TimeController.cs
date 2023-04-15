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
    [SerializeField] private float timeMultiplier, startHour;
    public int day = 0;
    private DateTime currentTime;

    [Header("Sun Controlls")]
    [SerializeField] private Light sunLight;
    [SerializeField] private float sunriseHour, sunsetHour, maxSunLightIntensity;
    private TimeSpan sunriseTime, sunsetTime;

    [Header("Moon Controlls")]
    [SerializeField] private Light moonLight;
    [SerializeField] private float maxMoonLightIntensity;

    [Header("Ambient Controlls")]
    [SerializeField] private AnimationCurve lightCurve;
    [SerializeField] private Color ambientDayLight, ambientNightLight;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);

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
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
        if (textHours) textHours.text = currentTime.ToString("HH:mm");
        TimeSpan midnight = new TimeSpan(0);
        if (currentTime.TimeOfDay == midnight)
        {
            day++;
            if (textDays) textDays.text = $"Dia: {day}";
        }
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(ambientNightLight, ambientDayLight, lightCurve.Evaluate(dotProduct));
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
}
