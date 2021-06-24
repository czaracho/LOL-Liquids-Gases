using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundsFX : MonoBehaviour
{
    AudioSource audioSource;
    public static SoundsFX instance;
    public AudioClip click;
    public AudioClip typeWriter;
    public AudioClip gameFinished;
    public AudioClip correct;
    public AudioClip incorrect;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0.2f;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlayGameFinished()
    {
        audioSource.PlayOneShot(gameFinished);
    }

    public void PlayCorrect()
    {
        audioSource.PlayOneShot(correct);
    }

    public void PlayIncorrect()
    {
        audioSource.PlayOneShot(incorrect);
    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(click);
    }

    public void PlayType()
    {
        audioSource.PlayOneShot(typeWriter, 0.1f);
    }
}
