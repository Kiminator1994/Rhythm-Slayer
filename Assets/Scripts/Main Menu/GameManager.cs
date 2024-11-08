using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    // GameScene UI
    private UIManager uiManager;

    // Points & combo
    private SwordInteraction swordLeft;
    private SwordInteraction swordRight;
    private NoteEndManager noteEndManager;
    private MusicManager musicManager;
    private int playerPoints = 0;
    private int playerCombo = 0;
    private int maxCombo = 0;
    private byte basicPoints = 100;
    private float multiplier = 1f;
    private int playerMissCount = 0;

    // Health
    private byte missDamage = 10;
    private byte wrongHitDamage = 3;
    private short maxHealth = 100;
    private short actualHealth = 100;

    // ScoreScreen Title
    private string scoreScreenTitle = "Song finished. Congratulations";

    // Selected Music
    private SongData selectedSongData;

    // Pause
    public bool isPaused = false;
    public event Action OnPauseStart;
    public event Action OnPauseEnd;

    // GameOver
    public bool GameIsOver = false;

    // test
    int subscribeCount = 0;
    int unsubscribeCount = 0;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // GameScene
    // Handle Events and send data to UI

    public void SetGameSceneReferences(SwordInteraction swordLeftRef, SwordInteraction swordRightRef, UIManager uiManagerRef, NoteEndManager noteEndManagerRef, MusicManager musicManagerRef)
    {
        swordLeft = swordLeftRef;
        swordRight = swordRightRef;
        uiManager = uiManagerRef;
        noteEndManager = noteEndManagerRef;
        musicManager = musicManagerRef;
    }

    public void SubcscribeGameSceneEvents()
    {
        swordLeft.OnCubeNoteHit += SetCombo;
        swordRight.OnCubeNoteHit += SetCombo;
        swordLeft.OnCubeNoteHit += SetPoints;
        swordRight.OnCubeNoteHit += SetPoints;
        swordLeft.OnCubeNoteHit += SetHealth;
        swordRight.OnCubeNoteHit += SetHealth;
        swordLeft.OnWrongHit += SetHealthOnWrongHit;
        swordRight.OnWrongHit += SetHealthOnWrongHit;
        swordLeft.OnWrongHit += SetComboOnMiss;
        swordRight.OnWrongHit += SetComboOnMiss;

        musicManager.OnPlaytimeUpdated += SetPlaytime;
        musicManager.OnMusicEnd += SetWinScoreScreenTitle;
        musicManager.OnMusicEnd += EndGame;

        noteEndManager.OnNoteMiss += SetComboOnMiss;
        noteEndManager.OnNoteMiss += SetHealthOnMiss;
        noteEndManager.OnNoteMiss += SetMissCount;
        ++subscribeCount;
        Debug.Log("subscribed: " + subscribeCount + " unsubscribed: " + unsubscribeCount);
    }

    public void ResetGameSceneStats()
    {
        Debug.Log("Before Reset: " + "title " + scoreScreenTitle + " PlayerPoints: " + playerPoints + " MacCombo: " + maxCombo + " playermissCount: " + playerMissCount);
        playerPoints = 0;
        playerCombo = 0;
        multiplier = 1f;
        playerMissCount = 0;
        actualHealth = 100;
        maxCombo = 0;
        Debug.Log("After Reset: " + "title " + scoreScreenTitle + " PlayerPoints: " + playerPoints + " MacCombo: " + maxCombo + " playermissCount: " + playerMissCount);
    }

    public void UnsubcscribeGameSceneEvents()
    {
        swordLeft.OnCubeNoteHit -= SetCombo;
        swordRight.OnCubeNoteHit -= SetCombo;
        swordLeft.OnCubeNoteHit -= SetPoints;
        swordRight.OnCubeNoteHit -= SetPoints;
        swordLeft.OnCubeNoteHit -= SetHealth;
        swordRight.OnCubeNoteHit -= SetHealth;
        swordLeft.OnWrongHit -= SetHealthOnWrongHit;
        swordRight.OnWrongHit -= SetHealthOnWrongHit;
        swordLeft.OnWrongHit -= SetComboOnMiss;
        swordRight.OnWrongHit -= SetComboOnMiss;


        musicManager.OnPlaytimeUpdated -= SetPlaytime;
        musicManager.OnMusicEnd -= SetWinScoreScreenTitle;
        musicManager.OnMusicEnd -= EndGame;

        noteEndManager.OnNoteMiss -= SetComboOnMiss;
        noteEndManager.OnNoteMiss -= SetHealthOnMiss;
        ++unsubscribeCount; // Check for memory leak
    }

    private void SetPlaytime(float currentPlaytime)
    {
        uiManager.UpdatePlaytime(currentPlaytime);
    }

    private void SetPoints()
    {
        if (playerCombo >= 10)
        {
            multiplier = 1 + ((playerCombo / 10) / 10); // Gain 0.1 multiplier for each 10 combostreak
        }

        playerPoints += (int)(basicPoints * multiplier);
        uiManager.UpdatePoints(playerPoints);
    }

    private void SetCombo()
    {
        playerCombo += 1;
        uiManager.UpdateCombo(playerCombo);

        if (playerCombo > maxCombo)
        {
            maxCombo = playerCombo;
        }
    }

    private void SetHealth()
    {
        if (actualHealth < maxHealth && actualHealth > 0)
        {
            actualHealth += 1;
            uiManager.UpdateHealth(actualHealth);
        }
    }

    private void SetComboOnMiss()
    {
        playerCombo = 0;
        uiManager.UpdateCombo(playerCombo);
    }

    private void SetHealthOnMiss()
    {
        actualHealth -= missDamage;
        if (actualHealth > 0)
        {
            uiManager.UpdateHealth(actualHealth);
        }

        else if (actualHealth <= 0)
        {
            actualHealth = 0;
            uiManager.UpdateHealth(actualHealth);
            musicManager.StopMusic();
            GameIsOver = true;
            SetScoreScreenTitle("You Lose");
            EndGame();
        }
    }

    private void SetHealthOnWrongHit()
    {
        actualHealth -= wrongHitDamage;
        if (actualHealth > 0)
        {
            uiManager.UpdateHealth(actualHealth);
        }

        else if (actualHealth <= 0)
        {
            actualHealth = 0;
            uiManager.UpdateHealth(actualHealth);
            musicManager.StopMusic();
            GameIsOver = true;
            SetScoreScreenTitle("You Lose");
            EndGame();
        }
    }

    private void SetMissCount()
    {
        ++playerMissCount;
    }

    private void SetScoreScreenTitle(string title)
    {
        scoreScreenTitle = title;
    }

    private void SetWinScoreScreenTitle()
    {
        scoreScreenTitle = "Song finished. Congratulations!";
    }

    public void UpdateHighScore()
    {
        // Check if the current score is higher than the high score
        if (playerPoints > selectedSongData.highScore)
        {
            selectedSongData.highScore = playerPoints;
        }

        // Check if the current combo is higher than the max combo
        if (maxCombo > selectedSongData.maxCombo)
        {
            selectedSongData.maxCombo = maxCombo;
            Debug.Log($"New max combo: {selectedSongData.maxCombo} for song: {selectedSongData.title}");
        }
    }

    private void EndGame()
    {
        GameIsOver = true;
        Debug.Log("Endgame called.");
        UpdateHighScore();
        UnsubcscribeGameSceneEvents();
        SceneManager.LoadScene("ScoreScreen");
    }


    // Pause Menu

    public void PauseGame()
    {
        OnPauseStart?.Invoke();
        isPaused = true;
        musicManager.PauseMusic();
        uiManager.ShowPauseMenu();
    }

    public void ResumeGame()
    {
        OnPauseEnd?.Invoke();
        musicManager.UnPauseMusic();
        isPaused = false;
    }

    public void RestartGame()
    {
        UnsubcscribeGameSceneEvents();
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMainMenu()
    {
        UnsubcscribeGameSceneEvents();
        SceneManager.LoadScene("MainMenu");
    }


    // ScoreScreenScene
    // GetData

    public string GetTitle()
    {
        return scoreScreenTitle;
    }

    public int GetPlayerPoints()
    {
        return playerPoints;
    }

    public int GetMaxCombo()
    {
        return maxCombo;
    }

    public int GetPlayerMissCount()
    {
        return playerMissCount;
    }


    // MainMenu
    // Get Selected Music from SongLibraryManager

    public void SetSelectedSongData(SongData song)
    {
        selectedSongData = song;
    }

    public void StartSelectedSong()
    {
        if (selectedSongData.audioClip != null)
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    public float GetRemainingCountdown()
    {
        return uiManager.GetRemainingCountdownTime();
    }

    public SongData GetSongData()
    {
        return selectedSongData;
    }

    public AudioClip GetAudioClip()
    {
        return selectedSongData.audioClip;
    }

    public float GetBpm()
    {
        return selectedSongData.bpm;
    }

    public float GetNoteSpeed()
    {
        return selectedSongData.noteSpeed;
    }
}
