using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Linq;
using UnityEngine.Networking;

public class SongLibraryManager : MonoBehaviour
{
    [SerializeField] private List<SongData> songLibrary;
    [SerializeField] private List<SongData> songLibrary2;
    [SerializeField] private TextMeshProUGUI mainMenuSongInterpret;
    [SerializeField] private TextMeshProUGUI mainMenuSongTitle;
    [SerializeField] private TextMeshProUGUI highscore;
    [SerializeField] private TextMeshProUGUI maxCombo;
    [SerializeField] private Image mainMenuImage;
    [SerializeField] private GameObject templateButton;
    [SerializeField] private Transform scrollbar;
    [SerializeField] private Transform scrollbar2;
    [SerializeField] private GameObject selectedSong;
    [SerializeField] private float sampletime = 20f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SongMapImporter songMapImporter;

    private string customSongsFolder = "Resources/CustomSongs";

    private void Start()
    {
        DisplaySongs(songLibrary, scrollbar);
        DisplaySongs(songLibrary2, scrollbar2);

        // Show the first song or the one selected befofore
        SongData selectedSongData = GameManager.Instance.GetSongData();
        if (selectedSongData != null)
        {
            OnSongSelected(selectedSongData);
        }
        else if (songLibrary.Count > 0)
        {
            SongData firstSong = songLibrary[0];
            OnSongSelected(firstSong, false);
        }
    }

    private void DisplaySongs(List<SongData> songList, Transform scrollbar)
    {
        foreach (var song in songList)
        {
            if (!string.IsNullOrEmpty(song.noteDataFilePath))
            {
                string filePath = Path.Combine(Application.dataPath, customSongsFolder, song.noteDataFilePath); // Path to the .dat file with infos from Beatmapper
                if (File.Exists(filePath))
                {
                    var map = songMapImporter.LoadMap(filePath);
                    song.noteList = songMapImporter.ConvertToNoteData(map);
                    song.eventList = songMapImporter.ConvertToEventData(map);
                    CreateSongButton(song, scrollbar);
                }
                else
                {
                    Debug.LogError($"Note file not found for song: {song.title} at {filePath}");
                }
            }
        }
    }


    private void CreateSongButton(SongData song, Transform scrollbar)
    {
        GameObject newButton = Instantiate(templateButton, scrollbar);
        newButton.SetActive(true);

        TextMeshProUGUI songNameText = newButton.transform.Find("SongTitle").GetComponent<TextMeshProUGUI>();
        songNameText.text = song.title;

        TextMeshProUGUI songInterpretText = newButton.transform.Find("SongInterpret").GetComponent<TextMeshProUGUI>();
        songInterpretText.text = song.interpret;

        TextMeshProUGUI difficultyText = newButton.transform.Find("Difficulty").GetComponent<TextMeshProUGUI>();
        difficultyText.text = song.difficulty;

        Image backgroundImage = newButton.transform.Find("Image").GetComponent<Image>();
        backgroundImage.sprite = song.backgroundImage;

        Button buttonComponent = newButton.GetComponent<Button>();
        buttonComponent.onClick.AddListener(() => OnSongSelected(song));
    }

    private void OnSongSelected(SongData song, bool playSample = true)
    {
        if (playSample)
        {
            PlaySample(song);
        }

        GameManager.Instance.SetSelectedSongData(song);
        mainMenuSongTitle.text = song.title;
        mainMenuSongInterpret.text = song.interpret;
        mainMenuImage.sprite = song.backgroundImage;
        highscore.text = song.highScore.ToString();
        maxCombo.text = song.maxCombo.ToString();
        selectedSong.SetActive(true);
    }

    private void PlaySample(SongData song)
    {
        audioSource.clip = song.audioClip;
        audioSource.Play();
        StartCoroutine(StopSample(sampletime));
    }

    private IEnumerator StopSample(float time)
    {
        yield return new WaitForSeconds(time);
        audioSource.Stop();
    }
}
