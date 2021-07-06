using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioSource audioSource;
    public static MusicController instance;
    public AudioClip mainThemeSong;
    public AudioClip endingSong;
    public float secondsToFadeOut = 3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0.25f;
            audioSource.clip = mainThemeSong;
            audioSource.loop = true;
            audioSource.playOnAwake = true;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start(){
        audioSource.Play();
    }

    public void PlayFinalTheme() {
        PlayFinalSong();
    }

    public void StopMainSong() {
        StartCoroutine(FadeOutSong());
    }


    IEnumerator FadeOutSong()
    {
        // Check Music Volume and Fade Out
        while (audioSource.volume > 0.01f)
        {
            audioSource.volume -= Time.deltaTime / secondsToFadeOut;
            yield return null;
        }

        // Make sure volume is set to 0
        audioSource.volume = 0;

        // Stop Music
        audioSource.Stop();
    }

    void PlayFinalSong() {
        audioSource.volume = 0.35f;
        audioSource.clip = endingSong;
        audioSource.Play();
    }

}

