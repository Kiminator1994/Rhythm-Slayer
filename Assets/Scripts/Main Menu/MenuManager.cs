using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup settingsMenu;
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private float fadeDuration = 0.1f;


    public void ShowSettings()
    {
        StartCoroutine(FadeMenu(mainMenu, 1, 0, fadeDuration));
        StartCoroutine(FadeMenu(settingsMenu, 0, 1, fadeDuration));
    }

    public void CloseSettings()
    {
        StartCoroutine(FadeMenu(settingsMenu, 1, 0, fadeDuration));
        StartCoroutine(FadeMenu(mainMenu, 0, 1, fadeDuration));
    }

    public void StartSong()
    {
        GameManager.Instance.StartSelectedSong();
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

    public void EndGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        Debug.Log("Game is exiting...");
#endif
    }


}
