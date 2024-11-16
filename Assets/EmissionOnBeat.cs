using System;
using System.Collections;
using UnityEngine;

public class EmissionOnBeat : MonoBehaviour
{
    public MusicManager musicManager;
    [SerializeField] private SongData songData;
    public float playerOffsetTime = 0f; // Adjustable time offset for the player in settings TO DO!!!!

    public void Start()
    {
        StartCoroutine(StartLightning());
    }

    private IEnumerator StartLightning()
    {
        float secondsPerBeat = 60f / songData.bpm; // Time interval for each beat
        float nextFlashTime = 0f; // Tracks when the next flash should occur
        float musicStartTime = musicManager.GetCurrentTime(); // Initial music time

        while (!GameManager.Instance.GameIsOver)
        {
            float currentTime = musicManager.GetCurrentTime();

            // Check if it's time to flash based on the nextFlashTime
            if (currentTime >= nextFlashTime)
            {
                FlashLight();
                nextFlashTime += secondsPerBeat; // Schedule the next flash
            }

            yield return null; // Wait for the next frame
        }
    }



    private Light beatLight;
    private Renderer objectRenderer;
    private Material objectMaterial;
    private Color originalColor;
    private Color beatColor = Color.red;
    private float flashDuration = 0.1f; // Duration of the flash

    private void Awake()
    {
        // Get Light Component
        beatLight = GetComponent<Light>();
        if (beatLight == null)
        {
            Debug.LogWarning("No Light component found on the GameObject.");
        }

        // Get Renderer and Material for Emission (Optional)
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectMaterial = objectRenderer.material;
            if (objectMaterial.HasProperty("_EmissionColor"))
            {
                originalColor = objectMaterial.GetColor("_EmissionColor");
            }
        }
    }

    private void FlashLight()
    {
        // Flash the Light
        if (beatLight != null)
        {
            beatLight.enabled = true; // Turn on the light
            StartCoroutine(ResetLight()); // Reset after duration
        }

        // Flash Material Emission (Optional)
        if (objectMaterial != null)
        {
            objectMaterial.SetColor("_EmissionColor", beatColor * 2.0f); // Set bright color
            objectMaterial.EnableKeyword("_EMISSION"); // Enable emission
            StartCoroutine(ResetEmission()); // Reset after duration
        }
    }

    private IEnumerator ResetLight()
    {
        yield return new WaitForSeconds(flashDuration);
        if (beatLight != null)
        {
            beatLight.enabled = false; // Turn off the light
        }
    }

    private IEnumerator ResetEmission()
    {
        yield return new WaitForSeconds(flashDuration);
        if (objectMaterial != null)
        {
            objectMaterial.SetColor("_EmissionColor", originalColor); // Reset to original color
            objectMaterial.EnableKeyword("_EMISSION"); // Ensure emission is still active
        }
    }




}
