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

    private void Start()
    {
        UpdateOffsetTime(actualOffsetTime);
    }

    public void ShowSettings()
    {
        StartCoroutine(FadeMenu(graphicsMenu, 1, 0, fadeDuration));
        StartCoroutine(FadeMenu(playerSettingsMenu, 0, 1, fadeDuration));
    }

    public void CloseSettings()
    {
        StartCoroutine(FadeMenu(playerSettingsMenu, 1, 0, fadeDuration));
        StartCoroutine(FadeMenu(graphicsMenu, 0, 1, fadeDuration));
    }

    public void ShowGraphics()
    {
        StartCoroutine(FadeMenu(playerSettingsMenu, 1, 0, fadeDuration));
        StartCoroutine(FadeMenu(graphicsMenu, 0, 1, fadeDuration));
    }

    public void CloseGraphics()
    {
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
    }
}
