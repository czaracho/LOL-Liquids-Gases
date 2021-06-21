using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIBehaviour : MonoBehaviour
{
    public Button playButton;
    public Button restartButton;
    public GameObject playUnpressedImg;
    public GameObject playPressedImg;
    public GameObject restartUnpressedImg;
    public GameObject restartPressedImg;
    [HideInInspector]
    public static UIBehaviour instance;

    //Scene Fader
    public Image img;
    public AnimationCurve curve;

    private void Start()
    {
        if (instance != null)
        {
            return;
        }
        else {
            instance = this;
        }

        StartCoroutine(FadeIn());

    }

    public void PlayBouncyAnimation(string button) {

        if (button == "spawnWater") {
            playUnpressedImg.SetActive(false);
            playPressedImg.SetActive(true);
            playButton.transform.DOScale(new Vector3(1.1f, 1.1f), 5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutQuad).SetSpeedBased();
            playButton.enabled = false;


        }
        else if (button == "restart") {

            restartUnpressedImg.SetActive(false);
            restartPressedImg.SetActive(true);
            restartButton.transform.DOScale(new Vector3(1.1f, 1.1f), 5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutQuad).SetSpeedBased();
            restartButton.enabled = false;
        }
    }

    IEnumerator FadeIn()
    {

        float t = 2f;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }

    IEnumerator FadeOut(string scene)
    {
        float t = 0f;

        while (t < 2f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }

        SceneManager.LoadScene(scene);


    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

}
