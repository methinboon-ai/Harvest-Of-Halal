using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class DayCycle : MonoBehaviour
{
    // Adjust the duration of a full day cycle in seconds
    public float secondsInFullDay = 120f;

    [Range(0, 1)]
    public float currentTimeOfDay = 0f; // Current time (0=midnight, 0.5=noon, 1=midnight)

    private float timeMultiplier = 25f;
    private float sunInitialIntensity;
    private PlantHandler plantHandler;
    private bool grew = false;
    [SerializeField] float PlantGrowPoint;
    void Start()
    {
        // Get the initial intensity of the directional light
        sunInitialIntensity = GetComponent<Light>().intensity;
        plantHandler = GameObject.Find("Plants").GetComponent<PlantHandler>();
        PlantGrowPoint = Mathf.Clamp(PlantGrowPoint, 0.01f, 1);
    }

    void Update()
    {
        // Update the time of day
        currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

        // Loop the time back to 0 when it reaches 1 (a full cycle)
        if (currentTimeOfDay >= 1f)
        {
            currentTimeOfDay = 0f;
        }

        // Rotate the light around the X-axis
        // The rotation is based on the current time of day (360 degrees for a full cycle)
        transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360f) - 90f, 170f, 0f));
        // The -90f offset is used to make the light start at a typical 'sunrise' angle.

        // You can also adjust the light intensity and color based on the time of day
        UpdateLighting(currentTimeOfDay);
        DayPassedCheck();
    }

    void DayPassedCheck()
    {
        if (grew == false && currentTimeOfDay >= PlantGrowPoint)
        {
            //Debug.Log("Grow");
            grew = true;
            plantHandler.GrowAll();
        }
        if (currentTimeOfDay >= 0 && currentTimeOfDay < PlantGrowPoint)
        {
            grew = false;
        }
    }
    void UpdateLighting(float timePercent)
    {
        // Simple intensity adjustment: brighter during the day, dimmer at night
        if (timePercent < 0.25f || timePercent > 0.75f)
        {
            // Night time (dim or off)
            GetComponent<Light>().intensity = Mathf.Lerp(sunInitialIntensity, 0, timePercent > 0.75f ? (timePercent - 0.75f) * 4 : (0.25f - timePercent) * 4);
        }
        else
        {
            // Day time (full brightness)
            GetComponent<Light>().intensity = sunInitialIntensity;
        }

        // For a more advanced day/night cycle, consider using animation curves or gradients for color and intensity.
    }
}
