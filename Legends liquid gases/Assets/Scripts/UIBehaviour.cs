using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIBehaviour : MonoBehaviour
{
    //UI Type
    public bool isSlideLevel = false;

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
    public GameObject Bubble;

    //Stars
    public GameObject[] stars;

    [HideInInspector]
    public static UIBehaviour instance;

    //Scene Fader
    public Image img;
    public AnimationCurve curve;

    //Bubble Text Controller
    public TypeWriter TypeWriter;
    public SpriteRenderer[] slides;
    public string[] dialogLines;
    private int currentLine = 0;
    public int lineToSwitchImages = 0;
    bool isWaitingForClick = false;

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

        if (dialogLines.Length > 0) {
            EventManager.instance.WaitingForClickTrigger += SetWaitingForClickStatus;

            StartCoroutine(waitforReadFirstTime());
        }
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
                    bouncyAnimationButton(nextLvlButton, nextLevelPressedSprite);
                break;
        }
    }

    IEnumerator FadeIn()
    {
        LevelManager.instance.playerCanInteract = true;
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

        Vector3 buttonScale = new Vector3(button.transform.localScale.x, button.transform.localScale.y);
        button.GetComponent<Button>().image.sprite = pressedSprite;
        button.transform.DOScale(buttonScale * 0.9f, 10f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutQuad).SetSpeedBased();
        button.enabled = false;
    }

    public void toNextLevelTransition() {

        ingameLayout.SetActive(false);
        nextLevelLayout.SetActive(true);


        Sequence seq = DOTween.Sequence();
        seq.Append(nextLevelPanel.transform.DOLocalMove(new Vector2(0,0), moveDuration));
        seq.Append(nextLevelPanel.transform.DOScale(new Vector2(1, 0.85f), panelScaleDuration));
        seq.Append(nextLevelPanel.transform.DOScale(new Vector2(1, 1), panelScaleDuration));
        seq.Insert(0.25f, levelCompleteText.transform.DOScale(new Vector2(1.2f, 1.2f), levelCompleteDuration * 0.5f));
        seq.Insert(0.35f, levelCompleteText.transform.DOScale(new Vector2(1, 1), levelCompleteDuration));

        int currentStars = LevelManager.instance.currentLvlStarsEarned;

        for (int i = 0; i < currentStars; i++)
        {
            seq.Append(stars[i].transform.DOScale(new Vector2(1.25f, 1.25f), levelCompleteDuration * 0.5f));
            seq.Append(stars[i].transform.DOScale(new Vector2(1, 1), levelCompleteDuration * 0.75f));
        }

        nextLvlButton.transform.DOScale(new Vector2(1.1f, 1.1f), 1f).SetLoops(-1, LoopType.Yoyo);
    }

    public void SwitchToAnotherImage(SpriteRenderer image1, SpriteRenderer image2) {
       
        Sequence seq = DOTween.Sequence();
        seq.Append(image1.DOFade(0f, 1f));
        seq.Append(image2.DOFade(1f, 1f));
    }

    public void BubbleTextController(bool activateBubble) {

        Sequence seq = DOTween.Sequence();

        if (activateBubble)
        {
            seq.Append(Bubble.transform.DOScale(1.05f, 0.25f));
            seq.Append(Bubble.transform.DOScale(1.0f, 0.25f));
        }
        else {
            seq.Append(Bubble.transform.DOScale(1.05f, 0.25f));
            seq.Append(Bubble.transform.DOScale(0f, 0.25f));
        }
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!LevelManager.instance.playerCanInteract)
            {
                if (isWaitingForClick)
                {
                    BubbleAnimOnClick();
                    isWaitingForClick = false;

                    if (currentLine > 0 && currentLine < dialogLines.Length - 1)
                    {
                        TypeWriter.WriteText(dialogLines[currentLine]);
                        currentLine++;

                        if (lineToSwitchImages > 0 && currentLine == lineToSwitchImages)
                        {
                            if (slides.Length > 1)
                            {
                                SwitchToAnotherImage(slides[0], slides[1]);
                            }
                        }

                    }
                    else if (currentLine == dialogLines.Length - 1)
                    {
                        //Si el nivel es de slides, ir directo a la siguiente pantalla                        
                        LevelManager.instance.playerCanInteract = true;

                        currentLine++;

                        if (isSlideLevel)
                        {
                            TypeWriter.isLastSlide = true;
                            TypeWriter.WriteText(dialogLines[currentLine-1]);
                        }
                        else
                        {
                            TypeWriter.HideAll();
                        }
                    }
                }
            }
        }
    }

    IEnumerator waitforReadFirstTime()
    {
        yield return new WaitForSeconds(1.0f);
        BubbleTextController(true);
        yield return new WaitForSeconds(1.0f);
        TypeWriter.WriteText(dialogLines[0]);
        currentLine++;
    }

    void SetWaitingForClickStatus()
    {
        LevelManager.instance.playerCanInteract = false;
        isWaitingForClick = true;
    }

    private void OnDestroy()
    {
        EventManager.instance.WaitingForClickTrigger -= SetWaitingForClickStatus;
    }

    void BubbleAnimOnClick() {
        Sequence seqBubble = DOTween.Sequence();
        seqBubble.Append(Bubble.transform.DOScale(new Vector2(1.05f, 1.05f), 0.25f));
        seqBubble.Append(Bubble.transform.DOScale(new Vector2(1f, 1f), 0.25f));

    }
}
