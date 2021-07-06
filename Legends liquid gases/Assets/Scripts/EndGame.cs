using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    AudioSource audioSource;
    public Image faderImg;
    public AnimationCurve curve;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();    
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
        StartCoroutine(WaitToPlayFinalSFX());
        StartCoroutine(WaitToPlayFinalSong());
    }


    IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            faderImg.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }

    IEnumerator WaitToPlayFinalSFX() {
        yield return new WaitForSeconds(0.25f);
        audioSource.Play();
    }

    IEnumerator WaitToPlayFinalSong() {
        yield return new WaitForSeconds(1.25f);
        MusicController.instance.PlayFinalTheme();
    }
}
