using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SongLibraryManager : MonoBehaviour
{
    [SerializeField] private List<SongData> songLibrary;  // List of ScriptableObjects SongData
    [SerializeField] private GameObject templateButton;
    [SerializeField] private Transform buttonContainer;

    private void Start()
    {
        DisplaySongs();
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
        }
    }

    private void OnSongSelected(SongData song)
    {
        GameManager.Instance.StartSelectedSong(song.audioClip, song.bpm);
    }
}
