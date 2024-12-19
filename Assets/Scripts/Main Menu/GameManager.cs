using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    // GameScene
    // UI
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

    // MainMenuScene
    // Settings
    [SerializeField] private SettingsManager settingsManager;
    private float playerOffsetTime = 0;

    // GameModifications
    private bool noFailModeActive = false;
    private float speedMultiplier = 1f;

    // Save and Load Game
    private SaveData saveData;

    // test memoryleak
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

    private void Start()
    {
        saveData = GameSaveManager.LoadGame();
        LoadSaveData();
        playerOffsetTime = settingsManager.GetPlayerOffset();
    }

    // GameScene
    // Handle Events and Reset GameScene

    public void SetGameSceneReferences(SwordInteraction swordLeftRef, SwordInteraction swordRightRef, UIManager uiManagerRef, NoteEndManager noteEndManagerRef, MusicManager musicManagerRef)
    {
        swordLeft = swordLeftRef;
        swordRight = swordRightRef;
        uiManager = uiManagerRef;
        noteEndManager = noteEndManagerRef;
        musicManager = musicManagerRef;
        ApplyMusicSpeed();
    }

    public void ResetGameSceneStats()
    {
        playerPoints = 0;
        playerCombo = 0;
        multiplier = 1f;
        playerMissCount = 0;
        actualHealth = 100;
        maxCombo = 0;
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


    // Update UI

    private void SetPlaytime(float currentPlaytime)
    {
        uiManager.UpdatePlaytime(currentPlaytime);
    }

    private void SetPoints()
    {
        if (playerCombo >= 10)
        {
            multiplier = 1f + (playerCombo / 10) * 0.1f; // Gain 0.1 multiplier for each 10 combostreak
        }
  
        playerPoints += (int)(basicPoints * multiplier);
        uiManager.UpdatePoints(playerPoints);
        uiManager.UpdateMultiplier(multiplier);
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
        multiplier = 1;
        uiManager.UpdateCombo(playerCombo);
    }

    private void SetHealthOnMiss()
    {
        if (noFailModeActive)
        { return; }

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
        if (noFailModeActive)
            { return; }

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

    public void SetPlayerOffset(float offset)
    {
        playerOffsetTime = offset;
    }

    public float GetPlayerOffset()
    {
        return playerOffsetTime;
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
        // Try to find an existing song in the saveData
        var songSaveData = saveData.songSaveDatas.FirstOrDefault(data => data.songTitle == selectedSongData.title);

        if (songSaveData == null)
        {
            // Create a new entry if its the first time the song is completed
            songSaveData = new SongSaveData(selectedSongData.title, playerPoints, maxCombo);
            saveData.songSaveDatas.Add(songSaveData);
            selectedSongData.highScore = playerPoints;
            selectedSongData.maxCombo = maxCombo;
        }
        else
        {
            // Update the existing entry if new scores are higher
            songSaveData.highScore = Mathf.Max(songSaveData.highScore, playerPoints);
            selectedSongData.highScore = songSaveData.highScore;
            songSaveData.maxCombo = Mathf.Max(songSaveData.maxCombo, maxCombo);
            selectedSongData.maxCombo = songSaveData.maxCombo;
        }

        // Save the updated data
        GameSaveManager.SaveGame(saveData);
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
        isPaused = true;
        OnPauseStart?.Invoke();
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
            CheckGameModifications();
            SceneManager.LoadScene("GameScene");
        }
    }

    private void CheckGameModifications()
    {
        GameModiManager gameModiManager = FindObjectOfType<GameModiManager>();

        if (gameModiManager != null)
        {
            // Check if faster game is selected
            if (gameModiManager.IsFasterGameSelected())
            {
                SetGameSpeed(1.25f);
            }
            else if (gameModiManager.IsSlowerGameSelected())
            {
                SetGameSpeed(0.7f);
            }
            else
            {
                SetGameSpeed(1f); // Normal speed
            }

            if (gameModiManager.IsNoFailModeSelected())
            {
                noFailModeActive = true;
            }
            else
            {
                noFailModeActive = false;
            }
        }   
    }

    public void SetGameSpeed(float newSpeedMultiplier)
    {
        speedMultiplier = newSpeedMultiplier;
        Time.timeScale = speedMultiplier;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // make sure physic calculations stay consistent with the new timeScale
    }
    public float GetSpeedMultiplier()
    {
        return speedMultiplier;
    }

    private void ApplyMusicSpeed()
    {
        // Change Music speed with pitch, accordong to speedmultiplier, set by gameModification
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        if (musicManager != null && musicManager.audioSource != null)
        {
            musicManager.audioSource.pitch = speedMultiplier;
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

    private void LoadSaveData()
    {
        foreach (var songData in Resources.FindObjectsOfTypeAll<SongData>())
        {
            SongSaveData savedData = saveData.songSaveDatas.Find(data => data.songTitle == songData.title);
            if (savedData != null)
            {
                songData.highScore = savedData.highScore;
                songData.maxCombo = savedData.maxCombo;
            }
        }
    }
}
