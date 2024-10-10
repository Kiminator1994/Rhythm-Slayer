using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreScreenManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI maxCombo;
    [SerializeField] private TextMeshProUGUI missCount;

    private void Start()
    {
        if(GameManager.Instance != null)
        {
            string title = GameManager.Instance.GetTitle();
            int playerPoints = GameManager.Instance.GetPlayerPoints();
            int maxCombo = GameManager.Instance.GetMaxCombo();
            int missCount = GameManager.Instance.GetPlayerMissCount();
            Debug.Log("MissCount: " + missCount);
            SetTitle(title);
            SetScore(playerPoints);
            SetMaxCombo(maxCombo);
            SetMissCount(missCount);

            Debug.Log(title + " " + playerPoints + " " + maxCombo + " " + missCount);
        }
    }

    private void SetTitle(string title)
    {
        this.titleText.text = title;
    }

    private void SetScore(int score)
    {
        this.score.text = score.ToString();
    }

    private void SetMaxCombo(int maxCombo)
    {
        this.maxCombo.text = maxCombo.ToString();
    }

    private void SetMissCount(int missCount)
    {
        this.missCount.text = missCount.ToString();
    }

    public void SelectedMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
