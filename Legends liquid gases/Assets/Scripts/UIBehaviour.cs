using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIBehaviour : MonoBehaviour
{
    //Layouts
    public GameObject ingameLayout;
    public GameObject nextLevelLayout;
    public GameObject nextLevelPanel;

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

    //Transitions
    public float moveDuration = 0.45f;
    public float panelScaleDuration = 0.15f;
    public float levelCompleteDuration = 0.15f;
    public float starsMoveDuration = 0.25f;
    public GameObject levelCompleteText;

    //Stars
    public GameObject[] stars;

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
        //StartCoroutine(startTransition());

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

    public void toNextLevelTransition() {

        ingameLayout.SetActive(false);
        nextLevelLayout.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(nextLevelPanel.transform.DOLocalMove(new Vector2(0,0), moveDuration));
        seq.Append(nextLevelPanel.transform.DOScale(new Vector2(1, 0.85f), panelScaleDuration));
        seq.Append(nextLevelPanel.transform.DOScale(new Vector2(1, 1), panelScaleDuration));
        seq.Append(levelCompleteText.transform.DOScale(new Vector2(1.25f, 1.25f), levelCompleteDuration * 0.5f));
        seq.Append(levelCompleteText.transform.DOScale(new Vector2(1, 1), levelCompleteDuration));

        int currentStars = LevelManager.instance.currentLvlStarsEarned;

        for (int i = 0; i < currentStars; i++)
        {
            seq.Append(stars[i].transform.DOScale(new Vector2(1.25f, 1.25f), levelCompleteDuration * 0.5f));
            seq.Append(stars[i].transform.DOScale(new Vector2(1, 1), levelCompleteDuration));
        }

        nextLvlButton.transform.DOScale(new Vector2(1.1f, 1.1f), 1f).SetLoops(-1, LoopType.Yoyo);
    }

    IEnumerator startTransition() {
        yield return new WaitForSeconds(1f);
        toNextLevelTransition();
    }

    IEnumerator starsTransition() {

        LevelManager.instance.currentLvlStarsEarned = 3;
        int currentStars = LevelManager.instance.currentLvlStarsEarned;

        Sequence sStars = DOTween.Sequence();


        for (int i = 0; i < currentStars; i++) { 
        
        }

        yield return new WaitForSeconds(0.25f);
    }
}
