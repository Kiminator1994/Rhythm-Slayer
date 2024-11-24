using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SettingsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI settingsSavedText;
    [SerializeField] private TextMeshProUGUI offsetTime;
    [SerializeField] private Slider slider;
    [SerializeField] private CanvasGroup playerSettingsMenu;
    [SerializeField] private CanvasGroup graphicsMenu;
    [SerializeField] private float fadeDuration = 0.1f;
    private float actualOffsetTime = 0f;
    private bool settingsPageActive = true;

    private void Start()
    {
        UpdateOffsetTime(actualOffsetTime, false);
    }

    public void ShowSettings()
    {
        if (settingsPageActive) 
            return;
        StartCoroutine(FadeMenu(graphicsMenu, 1, 0, fadeDuration));
        StartCoroutine(FadeMenu(playerSettingsMenu, 0, 1, fadeDuration));
        settingsPageActive = true;
    }

    public void CloseSettings()
    {
        if (!settingsPageActive)
            return;
        StartCoroutine(FadeMenu(playerSettingsMenu, 1, 0, fadeDuration));
        StartCoroutine(FadeMenu(graphicsMenu, 0, 1, fadeDuration));
    }

    public void ShowGraphics()
    {
        if (!settingsPageActive)
            return;
        StartCoroutine(FadeMenu(playerSettingsMenu, 1, 0, fadeDuration));
        StartCoroutine(FadeMenu(graphicsMenu, 0, 1, fadeDuration));
        settingsPageActive = false;
    }

    public void CloseGraphics()
    {
        if (settingsPageActive)
            return;
        StartCoroutine(FadeMenu(graphicsMenu, 1, 0, fadeDuration));
        StartCoroutine(FadeMenu(playerSettingsMenu, 0, 1, fadeDuration));
    }

    private IEnumerator FadeMenu(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        if (endAlpha > 0)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            canvasGroup.alpha = newAlpha;
            yield return null;
        }

        if (endAlpha == 0)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    // Slider can only use methods with 1 float parameter
    public void UpdateOffsetTime(float currentOffset)
    {
        actualOffsetTime = currentOffset;
        offsetTime.text = currentOffset.ToString("F3") + " Sec.";
        settingsSavedText.text = "Unsaved Settings!";
        settingsSavedText.gameObject.SetActive(true);
    }


    public void UpdateOffsetTime(float currentOffset, bool offsetChanged = false)
    {
        if (offsetChanged)
        {
            settingsSavedText.text = "Unsaved Settings!";
            settingsSavedText.gameObject.SetActive(true);
        }
        actualOffsetTime = currentOffset;
        offsetTime.text = currentOffset.ToString("F3") + " Sec.";
    }

    public float GetPlayerOffset()
    {
        return actualOffsetTime;
    }

    public void AddToPlayerOffset()
    {
        actualOffsetTime += 0.01f;
        slider.value = actualOffsetTime;
        UpdateOffsetTime(actualOffsetTime, true);
    }

    public void SubToPlayerOffset()
    {
        actualOffsetTime -= 0.01f;
        slider.value = actualOffsetTime;
        UpdateOffsetTime(actualOffsetTime, true);
    }

    public void ResetPlayerOffset()
    {
        actualOffsetTime = 0f;
        slider.value = 0f;
        UpdateOffsetTime(actualOffsetTime, true);
    }

    // TO DO
    public void SaveSettings()
    {
        GameManager.Instance.SetPlayerOffset(actualOffsetTime);
        settingsSavedText.text = "Settings saved.";
        settingsSavedText.gameObject.SetActive(true);
        StartCoroutine(DeactivateSavedtext());
    }

    private IEnumerator DeactivateSavedtext()
    {
        yield return new WaitForSeconds(3f);
        settingsSavedText.gameObject.SetActive(false);
    }
}
