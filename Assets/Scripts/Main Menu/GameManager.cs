using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton

    // GameScene UI
    private UIManager hudManager;

    // Playtime
    private NoteSpawnManager noteSpawnManager;

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
    private byte damage = 10;
    private short maxHealth = 100;
    private short actualHealth = 100;

    // ScoreScreen Title
    private string scoreScreenTitle = "Song finished. Congratulations";

    // Selected Music
    private SongData selectedSongData;


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

    public void SetGameSceneReferences(SwordInteraction swordLeftRef, SwordInteraction swordRightRef, UIManager hudManagerRef, NoteSpawnManager noteSpawnManagerRef, NoteEndManager noteEndManagerRef, MusicManager musicManagerRef)
    {
        swordLeft = swordLeftRef;
        swordRight = swordRightRef;
        hudManager = hudManagerRef;
        noteSpawnManager = noteSpawnManagerRef;
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

        musicManager.OnPlaytimeUpdated += SetPlaytime;
        musicManager.OnMusicEnd += SetWinScoreScreenTitle;
        musicManager.OnMusicEnd += EndGame;

        noteEndManager.OnNoteMiss += SetComboOnMiss;
        noteEndManager.OnNoteMiss += SetHealthOnMiss;
        noteEndManager.OnNoteMiss += SetMissCount;
    }

    public void ResetGameSceneStats()
    {
        Debug.Log("After Reset: " + "title " + scoreScreenTitle + " PlayerPoints: " + playerPoints + " MacCombo: " + maxCombo + " playermissCount: " + playerMissCount);
        playerPoints = 0;
        playerCombo = 0;
        multiplier = 1f;
        playerMissCount = 0;
        actualHealth = 100;
        maxCombo = 0;
        Debug.Log("After Reset: " + "title " + scoreScreenTitle + " PlayerPoints: "  + playerPoints + " MacCombo: " + maxCombo + " playermissCount: " + playerMissCount);
    }

    private void UnsubcscribeGameSceneEvents()
    {
        swordLeft.OnCubeNoteHit -= SetCombo;
        swordRight.OnCubeNoteHit -= SetCombo;
        swordLeft.OnCubeNoteHit -= SetPoints;
        swordRight.OnCubeNoteHit -= SetPoints;
        swordLeft.OnCubeNoteHit -= SetHealth;
        swordRight.OnCubeNoteHit -= SetHealth;

        musicManager.OnPlaytimeUpdated -= SetPlaytime;
        musicManager.OnMusicEnd -= EndGame;

        noteEndManager.OnNoteMiss -= SetComboOnMiss;
        noteEndManager.OnNoteMiss -= SetHealthOnMiss;
    }

    private void SetPlaytime(float currentPlaytime)
    {
        hudManager.UpdatePlaytime(currentPlaytime);
    }

    private void SetPoints()
    {
        if (playerCombo >= 10)
        {
            multiplier = 1 + ((playerCombo / 10) / 10); // Gain 0.1 multiplier for each 10 combostreak
        }

        playerPoints += (int)(basicPoints * multiplier);
        hudManager.UpdatePoints(playerPoints);
    }

    private void SetCombo()
    {
        playerCombo += 1;
        hudManager.UpdateCombo(playerCombo);

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
            hudManager.UpdateHealth(actualHealth);
        }
    }

    private void SetComboOnMiss()
    {
        playerCombo = 0;
        hudManager.UpdateCombo(playerCombo);
    }

    private void SetHealthOnMiss()
    {
        actualHealth -= damage;
        if (actualHealth > 0)
        {
            hudManager.UpdateHealth(actualHealth);
        }

        else if (actualHealth <= 0)
        {
            actualHealth = 0;
            hudManager.UpdateHealth(actualHealth);
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

    private void EndGame()
    {
        UnsubcscribeGameSceneEvents();
        SceneManager.LoadScene("ScoreScreen");
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
        Debug.Log(selectedSongData.audioClip);
    }

    public void StartSelectedSong()
    {
        Debug.Log(selectedSongData.audioClip + " " + selectedSongData.bpm);
        if (selectedSongData.audioClip != null && selectedSongData.bpm != 0f)
        {
            SceneManager.LoadScene("GameScene");
        }
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

}
