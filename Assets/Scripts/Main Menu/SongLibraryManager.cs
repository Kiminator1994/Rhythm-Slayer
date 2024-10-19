using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections;

public class SongLibraryManager : MonoBehaviour
{
    [SerializeField] private List<SongData> songLibrary;  // List of ScriptableObjects SongData
    [SerializeField] private TextMeshProUGUI mainMenuSongInterpret;
    [SerializeField] private TextMeshProUGUI mainMenuSongTitle;
    [SerializeField] private TextMeshProUGUI highscore;
    [SerializeField] private TextMeshProUGUI maxCombo;
    [SerializeField] private Image mainMenuImage;
    [SerializeField] private GameObject templateButton;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject selectedSong;
    [SerializeField] private float sampletime = 20f;

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
            Debug.Log(song.title);
            for (int i = 0; i < song.noteList.Count; i++)
            {
                Debug.Log("LineLayer: " + song.noteList[i].lineLayer + " Index:" + song.noteList[i].lineIndex + " timestamp: " + song.noteList[i].timestamp + " direction: " + song.noteList[i].cutDirection + " type: " + song.noteList[i].type);
            }

            GameObject newButton = Instantiate(templateButton, buttonContainer);
            newButton.SetActive(true);

            TextMeshProUGUI songNameText = newButton.transform.Find("SongTitle").GetComponent<TextMeshProUGUI>();
            songNameText.text = song.title;

            TextMeshProUGUI songInterpretText = newButton.transform.Find("SongInterpret").GetComponent<TextMeshProUGUI>();
            songInterpretText.text = song.interpret;

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
        mainMenuSongInterpret.text = song.interpret;
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
        StartCoroutine(StopSample(sampletime));
    }

    private IEnumerator StopSample(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
