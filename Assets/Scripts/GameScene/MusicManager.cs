using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    public event Action<float> OnPlaytimeUpdated;
    public event Action OnMusicEnd;

    private bool paused = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip clip)
    {
        audioSource.clip = clip;
        paused = false;
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
        paused = true;
        audioSource.Pause();
    }

    public void ContinueMusic()
    {
        audioSource.UnPause();
        paused = false;
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

        if (paused == false)
        {
            OnMusicEnd?.Invoke();
        }
    }
}
