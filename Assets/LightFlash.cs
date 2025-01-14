using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;
using UnityEngine.Rendering.PostProcessing;

public class LightFlash : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer _renderer;
    public GameObject dashWarning;
    public GameObject reactionTimeTracker;
    public PostProcessVolume postProcessVolume; // To store the Post-process Volume component
    private bool isPostProcessEnabled = false;
    private SwitchFromAItoMDV switchToMDVScript;
    public GameObject carAI;

    private Bloom bloomLayer; // Bloom effect reference
    private ColorGrading colorGradingLayer; // Color Grading effect reference

    public float amplitude = 5.0f; // Amplitude of the breathing effect
    public float frequency = 1.5f; // Frequency of the breathing effect

    private float baseIntensity = 0.0f; // Base intensity for the bloom

    // Start is called before the first frame update
    void Start()
    {
        carAI = GameObject.Find("DrivableSmartCommon-no_driver(Clone)");
        dashWarning = GameObject.Find("Dash_Warning");
        
        // Attempt to find the PostProcessVolume in the scene if it's not manually assigned
        if (postProcessVolume == null)
        {
            postProcessVolume = FindObjectOfType<PostProcessVolume>();
        }

        // Get the bloom layer from the post-processing profile
        if (postProcessVolume.profile.TryGetSettings(out bloomLayer))
        {
            Debug.Log("Bloom found in PostProcessVolume");
            baseIntensity = bloomLayer.intensity.value; // Store the initial intensity value
        }
        else
        {
            Debug.LogError("Bloom not found in PostProcessVolume. Make sure it's added to the profile.");
        }

        // Get the Color Grading layer from the post-processing profile
        if (postProcessVolume.profile.TryGetSettings(out colorGradingLayer))
        {
            Debug.Log("Color Grading found in PostProcessVolume");
            colorGradingLayer.colorFilter.value = Color.blue; // Set color filter to blue
        }
        else
        {
            Debug.LogError("Color Grading not found in PostProcessVolume. Make sure it's added to the profile.");
        }
    }

    public void flash()
    {
        // Start the breathing effect
        StartCoroutine(BreathingBloomCoroutine(2.5f, 50)); // 4 second duration, repeat 5 times
    }

    private IEnumerator BreathingBloomCoroutine(float duration, int repetitions)
{
    for (int i = 0; i < repetitions; i++)
    {
        float elapsedTime = 0.0f;

        // Peak intensity phase: increase to peak and then decrease smoothly
        while (elapsedTime < duration)
        {
            // Calculate time factor for breathing effect
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Calculate bloom intensity based on sine wave for smooth breathing
            float intensity = baseIntensity + Mathf.Sin(t * Mathf.PI * frequency) * amplitude;

            // Update the bloom intensity
            if (bloomLayer != null)
            {
                bloomLayer.intensity.value = intensity / 40000;
            }

            // Increment elapsed time
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Fade out phase: slowly reduce intensity to zero
        elapsedTime = 0.0f;
        float fadeDuration = 1.5f; // Duration of the fade out phase

        while (elapsedTime < fadeDuration)
        {
            // Calculate how far we are into the fade
            float t = elapsedTime / fadeDuration;

            // Smoothly interpolate from the current intensity to zero
            if (bloomLayer != null)
            {
                bloomLayer.intensity.value = Mathf.Lerp(bloomLayer.intensity.value, 0.0f, t);
            }

            // Increment elapsed time
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // After the fade-out phase, set bloom intensity to exactly zero (optional safety measure)
        if (bloomLayer != null)
        {
            bloomLayer.intensity.value = 0.0f;
        }

        // Optional: Add a delay between repetitions if you want to pause between cycles
        yield return new WaitForSeconds(0.0f); // 0.5 second pause between cycles (adjust as needed)
    }
}
}