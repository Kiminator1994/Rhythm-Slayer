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

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
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
        while (IsPlaying())
        {
            OnPlaytimeUpdated?.Invoke(GetCurrentTime());
            yield return null; // pause coroutine and resume in next frame, otherwise game will freeze as long as music is playing
        }
    }

    private IEnumerator CheckMusicEnd()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);

        if (GameManager.Instance.isPaused == false)
        {
            Debug.Log("Music ended");
            OnMusicEnd?.Invoke();
        }
    }
}
