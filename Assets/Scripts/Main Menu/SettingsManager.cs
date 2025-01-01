using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SettingsManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup playerSettingsMenu;
    [SerializeField] private TextMeshProUGUI settingsSavedText;
    [SerializeField] private TextMeshProUGUI offsetTime;
    [SerializeField] private Slider slider;
    [SerializeField] private CanvasGroup graphicsMenu;
    [SerializeField] private Toggle fullscreen;
    [SerializeField] private Toggle vsync;
    [SerializeField] private TextMeshProUGUI resolution;
    [SerializeField] private List<ResolutionItem> resolutions = new List<ResolutionItem>();
    [SerializeField] private float fadeDuration = 0.1f;

    private int selectedResolution;
    private float actualOffsetTime = 0f;
    private bool settingsPageActive = true;

    private void Start()
    {
        LoadSettings();
        LoadGraphics();
        settingsSavedText.gameObject.SetActive(false);
    }

    private void LoadSettings()
    {
        SaveData saveData = GameSaveManager.LoadGame();

        if (saveData.settingsSaveData != null)
        {
            actualOffsetTime = saveData.settingsSaveData.playerOffset;
            slider.value = actualOffsetTime;
            UpdateOffsetTimeText(actualOffsetTime, false);
        }
    }

    private void LoadGraphics()
    {
        fullscreen.isOn = Screen.fullScreen;
        if (QualitySettings.vSyncCount == 0)
        {
            vsync.isOn = false;
        }
        else
        {
            vsync.isOn = true;
        }

        bool foundResolution = false;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundResolution = true;
                selectedResolution = i;
                UpdateResolutionText();
            }
        }
        if (!foundResolution)
        {
            ResolutionItem resolutionItem = new ResolutionItem();
            resolutionItem.vertical = Screen.height;
            resolutionItem.horizontal = Screen.width;
            resolutions.Add(resolutionItem);
            selectedResolution = resolutions.Count - 1;
        }
    }

    // Switch between Settings

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

    // Player Settings

    // Slider can only use methods with 1 float parameter
    public void UpdateOffsetTime(float currentOffset)
    {
        actualOffsetTime = currentOffset;
        offsetTime.text = currentOffset.ToString("F3") + " Sec.";
        settingsSavedText.text = "Unsaved Settings!";
        settingsSavedText.gameObject.SetActive(true);
    }


    public void UpdateOffsetTimeText(float currentOffset, bool offsetChanged = false)
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
        UpdateOffsetTimeText(actualOffsetTime, true);
    }

    public void SubToPlayerOffset()
    {
        actualOffsetTime -= 0.01f;
        slider.value = actualOffsetTime;
        UpdateOffsetTimeText(actualOffsetTime, true);
    }

    public void ResetPlayerOffset()
    {
        actualOffsetTime = 0f;
        slider.value = 0f;
        UpdateOffsetTimeText(actualOffsetTime, true);
    }

    public void SaveSettings()
    {
        GameManager.Instance.SetPlayerOffset(actualOffsetTime);

        SettingsSaveData settingsData = new SettingsSaveData
        {
            playerOffset = actualOffsetTime,
            selectedResolutionIndex = selectedResolution,
            isFullscreen = fullscreen.isOn,
            vsync = vsync.isOn
        };

        // Load existing save data and save it in the json file
        SaveData saveData = GameSaveManager.LoadGame();
        saveData.settingsSaveData = settingsData;
        GameSaveManager.SaveGame(saveData);

        settingsSavedText.text = "Settings saved.";
        settingsSavedText.gameObject.SetActive(true);
        StartCoroutine(DeactivateSavedtext());
    }

    private IEnumerator DeactivateSavedtext()
    {
        yield return new WaitForSeconds(3f); // show Text for 3 seconds
        settingsSavedText.gameObject.SetActive(false);
    }


    // Graphic Settings

    public void SwitchResolutionLeft()
    {
        selectedResolution--;
        if (selectedResolution < 0)
        {
            selectedResolution = 0;
        }
        UpdateResolutionText();
    }

    public void SwitchResolutionRight()
    {
        selectedResolution++;
        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1;
        }
        UpdateResolutionText();
    }

    private void UpdateResolutionText()
    {
        resolution.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplyGraphics()
    {
        if (vsync.isOn) 
        { 
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreen.isOn);
    }
}

[System.Serializable] // Show in Inspector
public class ResolutionItem
{
    public int horizontal;
    public int vertical;
}