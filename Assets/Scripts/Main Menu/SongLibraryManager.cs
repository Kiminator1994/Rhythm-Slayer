using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections;

public class SongLibraryManager : MonoBehaviour
{
    [SerializeField] private List<SongData> songLibrary;  // List of ScriptableObjects SongData
    [SerializeField] private TextMeshProUGUI mainMenuSongTitle;
    [SerializeField] private TextMeshProUGUI highscore;
    [SerializeField] private TextMeshProUGUI maxCombo;
    [SerializeField] private Image mainMenuImage;
    [SerializeField] private GameObject templateButton;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject selectedSong;

    private void Start()
    {
        DisplaySongs();

        int playerPoints = GameManager.Instance.GetPlayerPoints();
        int maxCombo = GameManager.Instance.GetMaxCombo();
        SongData selectedSongData = GameManager.Instance.GetSongData();
        UpdateSongDataHighscoreStats(playerPoints, maxCombo, selectedSongData);

        if (selectedSongData != null)
        {
            OnSongSelected(selectedSongData);
        }
        else
        {
            SongData firstSong = songLibrary.First();
            OnSongSelected(firstSong);
        }
    }

    private void DisplaySongs()
    {
        foreach (var song in songLibrary)
        {
            GameObject newButton = Instantiate(templateButton, buttonContainer);
            newButton.SetActive(true);

            TextMeshProUGUI songNameText = newButton.transform.Find("SongTitle").GetComponent<TextMeshProUGUI>();
            songNameText.text = song.title;

            Image backgroundImage = newButton.transform.Find("Image").GetComponent<Image>();
            backgroundImage.sprite = song.backgroundImage;

            Button buttonComponent = newButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnSongSelected(song));
            buttonComponent.onClick.AddListener(() => PlaySample(song));

        }
    }

    public void UpdateSongDataHighscoreStats(int playerPoints, int maxCombo, SongData selectedSongData)
    {
        SongData songdata = songLibrary.Find(songData => songData == selectedSongData);
        if (songdata != null)
        {
            if (playerPoints > songdata.highScore)
            {
                songdata.highScore = playerPoints;
            }

            if (maxCombo > selectedSongData.maxCombo)
            {
                songdata.maxCombo = maxCombo;
            }
        }
    }

    private void OnSongSelected(SongData song)
    {
        GameManager.Instance.SetSelectedSongData(song);
        mainMenuSongTitle.text = song.title;
        mainMenuImage.sprite = song.backgroundImage;
        highscore.text = song.highScore.ToString();
        maxCombo.text = song.maxCombo.ToString();
        selectedSong.SetActive(true);
    }

    private void PlaySample(SongData song)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = song.audioClip;
        audioSource.Play();
        StartCoroutine(StopSample(20f));
    }

    private IEnumerator StopSample(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
