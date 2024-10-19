using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class UIManager : MonoBehaviour
{
    // Playtime
    [SerializeField] private TextMeshProUGUI playtime;

    // Points & combo
    [SerializeField] private TextMeshProUGUI points;
    [SerializeField] private TextMeshProUGUI combo;

    // Health
    [SerializeField] private TextMeshProUGUI health;

    // Countdown
    [SerializeField] private TextMeshProUGUI countdown;
    [SerializeField] private int countdownDuration = 3;

    // PauseGame
    [SerializeField] private Canvas Canvas;
    [SerializeField] private XRRayInteractor leftHandRay;
    [SerializeField] private XRRayInteractor rightHandRay;

    // UIStats

    public void UpdatePlaytime(float currentPlaytime)
    {
        // Convert playtime from seconds to format (minutes:seconds)
        int minutes = Mathf.FloorToInt(currentPlaytime / 60f);
        int seconds = Mathf.FloorToInt(currentPlaytime - minutes * 60);
        playtime.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void UpdatePoints(int actualPoints)
    {
        points.text = actualPoints.ToString();
    }

    public void UpdateCombo(int actualCombo)
    {
        combo.text = actualCombo.ToString();
    }

    public void UpdateHealth(int actualHealth)
    { 
        health.text = actualHealth.ToString();        
    }

    public void StartCountdown(System.Action onCountdownComplete)
    {
        StartCoroutine(CountdownCoroutine(onCountdownComplete));
    }

    private IEnumerator CountdownCoroutine(System.Action onCountdownComplete)
    {
        for (int seconds = countdownDuration; seconds > 0; seconds--)
        {
            countdown.text = seconds.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdown.text = "Start!";
        yield return new WaitForSeconds(1f);
        countdown.text = ""; // Hide the countdown

        onCountdownComplete.Invoke();
    }


    // UIPause

    public void ShowPauseMenu()
    {
        Canvas.gameObject.SetActive(true);
        rightHandRay.gameObject.SetActive(true);
        leftHandRay.gameObject.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        Canvas.gameObject.SetActive(false);
        rightHandRay.gameObject.SetActive(false);
        leftHandRay.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    public void ResumeGame()
    {
        ClosePauseMenu();
        GameManager.Instance.ResumeGame();
    }

    public void BackToMenu()
    {
        GameManager.Instance.UnsubcscribeGameSceneEvents();
        SceneManager.LoadScene("MainMenu");
    }
}
