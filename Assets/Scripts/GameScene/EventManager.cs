using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private LineRenderer[] redLasers = new LineRenderer[5];
    [SerializeField] private LineRenderer[] blueLasers = new LineRenderer[5];

    [SerializeField] private Renderer[] redPlatforms;
    [SerializeField] private Renderer[] bluePlatforms;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Renderer[] trackNeonLightBars; // All 30 manually positioned light bars
    [SerializeField] private float lightFadeDuration = 0.5f;

    private static readonly string EmissionKeyword = "_EMISSION";
    private static readonly string EmissionColorProperty = "_EmissionColor";

    [SerializeField] private MusicManager musicManager;
    private SongData songData;

    private void Start()
    {
        songData = GameManager.Instance.GetSongData();
        if (songData != null && songData.eventList != null)
        {
            StartCoroutine(HandleEvents());
        }
        else
        {
            Debug.LogError("SongData or EventList is missing!");
        }
    }

    private IEnumerator HandleEvents()
    {
        float secondsPerBeat = 60f / songData.bpm; // Duration of one beat in seconds

        foreach (EventData eventData in songData.eventList)
        {
            // Calculate the event time in seconds
            float eventTimeInSeconds = eventData.time * secondsPerBeat;
            float eventTime = eventTimeInSeconds + songData.songTimeOffset + GameManager.Instance.GetPlayerOffset();

            while (musicManager.GetCurrentTime() < eventTime)
            {
                yield return null;
            }

            switch (eventData.type)
            {
                case 1: // Neon Track Lights
                    HandleNeonTrackLights(eventData.value);
                    break;

                case 2: // Red Laser Events
                    foreach (LineRenderer redLaser in redLasers)
                    {
                        HandleLaserEvent(redLaser, eventData.value);
                    }
                    break;

                case 3: // Blue Laser Events
                    foreach (LineRenderer blueLaser in blueLasers)
                    {
                        HandleLaserEvent(blueLaser, eventData.value);
                    }
                    break;

                case 4: // Primary Light Events
                    HandlePrimaryLightEvent(eventData.value);
                    break;
            }
        }
    }

    private void HandleNeonTrackLights(int value)
    {
        foreach (Renderer lightBar in trackNeonLightBars)
        {
            Material lightBarMaterial = lightBar.material;

            switch (value)
            {
                case 0: // Off
                    StartCoroutine(FadeEmission(lightBarMaterial, lightBarMaterial.GetColor(EmissionColorProperty), Color.black, lightFadeDuration));
                    break;

                case 1: // Blue On
                    AssignMaterial(lightBar, blueMaterial);
                    EnableEmission(lightBarMaterial, blueMaterial.GetColor(EmissionColorProperty));
                    break;

                case 2: // Blue Fade In
                    AssignMaterial(lightBar, blueMaterial);
                    StartCoroutine(FadeEmission(lightBarMaterial, Color.black, blueMaterial.GetColor(EmissionColorProperty), lightFadeDuration));
                    break;

                case 5: // Red On
                    AssignMaterial(lightBar, redMaterial);
                    EnableEmission(lightBarMaterial, redMaterial.GetColor(EmissionColorProperty));
                    break;

                case 6: // Red Fade In
                    AssignMaterial(lightBar, redMaterial);
                    StartCoroutine(FadeEmission(lightBarMaterial, Color.black, redMaterial.GetColor(EmissionColorProperty), lightFadeDuration));
                    break;

                case 3: // Fade Out
                case 7: // Fade Out
                    StartCoroutine(FadeEmission(lightBarMaterial, lightBarMaterial.GetColor(EmissionColorProperty), Color.black, lightFadeDuration));
                    break;
            }
        }
    }

    private void HandleLaserEvent(LineRenderer laser, int value)
    {
        switch (value)
        {
            case 0: // Laser Off
                laser.enabled = false;
                break;
            case 1: // Blue Laser On
            case 5: // Red Laser On
                laser.enabled = true;
                break;
            case 2: // Fade In Blue 
            case 6: // Fade In Red
                laser.enabled = true;
                StartCoroutine(FadeLaser(laser, 0f, 1f, 0.5f)); // 0.5 seconds fade in
                break;
            case 3: // Fade Out Blue
            case 7: // Fade Out Red
                StartCoroutine(FadeLaser(laser, 1f, 0f, 0.5f)); // 0.5 seconds fade out
                break;
        }
    }

    private void HandlePrimaryLightEvent(int value)
    {
        // Handle red platforms
        foreach (Renderer platform in redPlatforms)
        {
            HandlePlatformEmission(platform, redMaterial, value);
        }

        // Handle blue platforms
        foreach (Renderer platform in bluePlatforms)
        {
            HandlePlatformEmission(platform, blueMaterial, value);
        }
    }

    private void HandlePlatformEmission(Renderer platform, Material material, int value)
    {
        Material platformMaterial = platform.material;
        Color materialEmissionColor = material.GetColor(EmissionColorProperty);

        switch (value)
        {
            case 0: // Light Off
                StartCoroutine(FadeEmission(platformMaterial, materialEmissionColor, Color.black, lightFadeDuration));
                break;
            case 1: // Light On
            case 5: // Light On
                EnableEmission(platformMaterial, materialEmissionColor);
                break;
            case 2: // Fade In
            case 6: // Fade In
                StartCoroutine(FadeEmission(platformMaterial, Color.black, materialEmissionColor, lightFadeDuration));
                break;
            case 3: // Fade Out
            case 7: // Fade Out
                StartCoroutine(FadeEmission(platformMaterial, materialEmissionColor, Color.black, lightFadeDuration));
                break;
        }
    }

    private void AssignMaterial(Renderer renderer, Material material)
    {
        renderer.material = material;
    }

    private void EnableEmission(Material material, Color emissionColor)
    {
        material.EnableKeyword(EmissionKeyword);
        material.SetColor(EmissionColorProperty, emissionColor);
    }

    private IEnumerator FadeEmission(Material material, Color startColor, Color endColor, float duration)
    {
        material.EnableKeyword(EmissionKeyword);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Color currentColor = Color.Lerp(startColor, endColor, elapsed / duration);
            material.SetColor(EmissionColorProperty, currentColor);
            elapsed += Time.deltaTime;
            yield return null;
        }

        material.SetColor(EmissionColorProperty, endColor);

        if (endColor == Color.black)
        {
            material.DisableKeyword(EmissionKeyword);
        }
    }

    private IEnumerator FadeLaser(LineRenderer laser, float startWidth, float endWidth, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float currentWidth = Mathf.Lerp(startWidth, endWidth, elapsed / duration);
            laser.startWidth = currentWidth;
            laser.endWidth = currentWidth;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // make sure Laser is completely turned off, otherwise laser sometimes slightly visible
        laser.startWidth = endWidth;
        laser.endWidth = endWidth;

        if (endWidth == 0f)
            laser.enabled = false;
    }
}
