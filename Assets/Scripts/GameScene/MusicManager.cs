using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    public event Action<float> OnPlaytimeUpdated;
    public event Action OnMusicEnd;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(UpdatePlaytime());
        StartCoroutine(CheckMusicEnd());
    }

    public float GetCurrentTime()
    {
        return audioSource.time;
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }

    public void UnPauseMusic()
    {
        audioSource.UnPause();
    }

    private IEnumerator UpdatePlaytime()
    {
        while (true)
        {
            OnPlaytimeUpdated?.Invoke(GetCurrentTime());
            yield return null; 
        }
    }

    private IEnumerator CheckMusicEnd()
    {
        while (audioSource.clip.length != GetCurrentTime())
        {
            yield return null;
        }
        OnMusicEnd?.Invoke();
    }
}
