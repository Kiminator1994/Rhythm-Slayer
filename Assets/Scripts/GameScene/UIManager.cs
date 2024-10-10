using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Playtime
    [SerializeField] private TextMeshProUGUI playtime;

    // Points & combo
    [SerializeField] private TextMeshProUGUI points;
    [SerializeField] private TextMeshProUGUI combo;

    // Health
    [SerializeField] private TextMeshProUGUI health;


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
}
