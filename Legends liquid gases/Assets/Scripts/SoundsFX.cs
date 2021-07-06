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
    public AudioClip levelFinished;
    public AudioClip correct;
    public AudioClip rotatePiece;
    public AudioClip pipePlaced;
    public AudioClip hint;
    public AudioClip closeButton;
    public AudioClip startWater;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0.95f;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this);
        }
    }

    public void PlayLevelFinished()
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(levelFinished);
    }

    public void PlayCorrect()
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(correct);
    }

    public void PlayRotatePiece()
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(rotatePiece);
    }

    public void PlayClick()
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(click);
    }

    public void PlayType()
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(typeWriter, 0.1f);
    }

    public void PlayClose() {
        audioSource.PlayOneShot(closeButton);
    }

    public void PlayPipePlaced() {
        audioSource.PlayOneShot(pipePlaced);
    }

    public void PlayHint() {
        audioSource.PlayOneShot(hint);
    }

    public void PlayStartWater() {
        audioSource.PlayOneShot(startWater);
    }
}
