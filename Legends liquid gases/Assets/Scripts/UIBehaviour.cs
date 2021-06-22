using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIBehaviour : MonoBehaviour
{
    //Ingame
    public Button playButton;
    public Button restartButton;
    public Sprite playPressedSprite;
    public Sprite restartPressedSprite;

    //Next Level Layout
    public Button restartButtonNxt;
    public Sprite restartPressedNextSprite;
    public Button nextLvlButton;
    public Sprite nextLevelPressedSprite;


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

        switch (button) {
            case "spawnWater":
                bouncyAnimationButton(playButton, playPressedSprite);
                break;

            case "restart":
                if (!LevelManager.instance.levelCleared)
                {
                    bouncyAnimationButton(restartButton, restartPressedSprite);
                }
                else
                {
                    bouncyAnimationButton(restartButtonNxt, restartPressedNextSprite);
                }
               break;

            case "nextLevel":
                    bouncyAnimationButton(nextLvlButton, restartPressedSprite);
                break;
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

    void bouncyAnimationButton(Button button, Sprite pressedSprite) {

        button.GetComponent<Button>().image.sprite = pressedSprite;
        button.transform.DOScale(new Vector3(0.9f, 0.9f), 10f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutQuad).SetSpeedBased();
        button.enabled = false;
    }
}
