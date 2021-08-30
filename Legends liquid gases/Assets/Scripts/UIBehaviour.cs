using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    [HideInInspector]
    public static UIBehaviour instance;

    //Basics
    [Header("Basics")]
    public bool isSlideLevel = false;
    public bool hasTutorial = false;
    [HideInInspector]
    public bool playerCanInteractUI = true;
    JSONNode _lang;

    //Layouts
    [Header("UILayouts")]
    public GameObject ingameLayout;
    public GameObject nextLevelLayout;
    public GameObject pauseLayout;
    public GameObject hintQuestionLayout;
    public GameObject hintImageLayout;
    public GameObject tutorialLayout;

    //Ingame
    [Header("Ingame elements")]
    public Button playButton;
    public Button restartButton;
    public Sprite playPressedSprite;
    public Sprite restartPressedSprite;
    public Button skipLevelButton;

    //Next Level Layout
    [Header("Next level elements")]
    public Button restartButtonNxt;
    public Sprite restartPressedNextSprite;
    public Button nextLvlButton;
    public Sprite nextLevelPressedSprite;
    public GameObject nextLevelPanel;
    public GameObject[] stars;

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject hintQuestionMenu;
    public GameObject hintImageMenu;

    [Header("Go back buttons")]
    public GameObject returnFromPauseBT;
    public GameObject returnFromHintBT;

    [Header("Backgrounds")]
    public GameObject bgPause;
    public GameObject bgHintPrompt;
    public GameObject bgHintImage;

    [Header("Transitions values")]
    public float moveDuration = 0.45f;
    public float panelScaleDuration = 0.15f;
    public float levelCompleteDuration = 0.15f;
    public float starsMoveDuration = 0.25f;
    public GameObject levelCompleteText;
    public GameObject bubbleContainer;

    [Header("Fader")]
    public Image faderImg;
    public AnimationCurve curve;

    //Dialog and slides
    [Header("Bubble text")]
    public TypeWriter TypeWriter;
    public GameObject[] slidesGroup;
    private List<SlideSprites> spritesSlidesList;
    public string[] dialogLines;
    private int currentLine = 0;
    public int[] lineToSwitchImages;  //dialog line
    private int slideChangeIndex = 0;
    private int dialogIndexCounter = 0;
    bool isWaitingForClick = false;

    [Header("Texts")]
    public TextMeshProUGUI levelSelectionTxt;
    public TextMeshProUGUI skipLevelTxt;
    public TextMeshProUGUI oneStarMovesTxt;
    public TextMeshProUGUI twoStarMovesTxt;
    public TextMeshProUGUI threeStarMovesTxt;
    public TextMeshProUGUI skipLevelScreenText;
    public TextMeshProUGUI currentStarsText;
    public TextMeshProUGUI solutionQuestionText;
    public TextMeshProUGUI costYouText;
    public TextMeshProUGUI wontEarnText;
    public TextMeshProUGUI earnedStarsCurrent;
    public Text howToPlayText;

    public class SlideSprites {
        public int slideIndex;
        public List<SpriteRenderer> sprites;
    }

    private void Awake()
    {
        if (instance != null) {
            return;
        }
        else {
            instance = this;
        }

    }

    private void Start()
    {
        _lang = SharedState.LanguageDefs;

        StartCoroutine(FadeIn());

        if (dialogLines.Length > 0) {
            EventManager.instance.WaitingForClickTrigger += SetWaitingForClickStatus;

            StartCoroutine(waitforReadFirstTime());
        }

        if (hasTutorial) {
            ingameLayout.SetActive(false);
            StartCoroutine(WaitToStartTutorial());
        }

        if (!isSlideLevel)
        {

            if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "LevelSelector"
                && SceneManager.GetActiveScene().name != "EndGame")
            {
                SetUITexts();
            }
        }
        else {
            GetAllSlideSprites();
        }

        EventManager.instance.ButtonClickAnimTrigger += BouncyAnimationButton;


    }

    private void OnDestroy()
    {
        EventManager.instance.WaitingForClickTrigger -= SetWaitingForClickStatus;
        EventManager.instance.ButtonClickAnimTrigger -= BouncyAnimationButton;
    }

    public void PlayBouncyAnimation(string button) {

        switch (button) {
            case "spawnWater":
                BouncyAnimation(playButton, playPressedSprite);
                break;

            case "restart":
                if (!GameManagerMain.instance.levelCleared)
                {
                    BouncyAnimation(restartButton, restartPressedSprite);
                }
                else
                {
                    BouncyAnimation(restartButtonNxt, restartPressedNextSprite);
                }
               break;

            case "nextLevel":
                    BouncyAnimation(nextLvlButton, nextLevelPressedSprite);
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
            faderImg.color = new Color(0f, 0f, 0f, a);
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
            faderImg.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }

        SceneManager.LoadScene(scene);
    }

    public void FadeTo(string scene)
    {
        ingameLayout.SetActive(false);
        StartCoroutine(FadeOut(scene));
    }

    void BouncyAnimation(Button button, Sprite pressedSprite) {
        SoundsFX.instance.PlayClick();
        Vector3 buttonScale = new Vector3(button.transform.localScale.x, button.transform.localScale.y);
        button.GetComponent<Button>().image.sprite = pressedSprite;
        button.transform.DOScale(buttonScale * 0.9f, 10f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutQuad).SetSpeedBased();
        button.enabled = false;
    }

    public void BouncyAnimationButton(GameObject button) {
        Vector3 buttonScale = new Vector3(button.transform.localScale.x, button.transform.localScale.y);
        button.transform.DOScale(buttonScale * 0.9f, 0.1f).SetLoops(2, LoopType.Yoyo);
    }

    public void toNextLevelTransition() {

        //if the level is restarting, even if the water fills the containers, the next level layout won't appear
        if (!GameManagerMain.instance.levelIsReseting) {
            ingameLayout.SetActive(false);
            nextLevelLayout.SetActive(true);
            SoundsFX.instance.PlayLevelFinished();

            Sequence seq = DOTween.Sequence();
            seq.Append(nextLevelPanel.transform.DOLocalMove(new Vector2(0, 0), moveDuration));
            seq.Append(nextLevelPanel.transform.DOScale(new Vector2(1, 0.85f), panelScaleDuration));
            seq.Append(nextLevelPanel.transform.DOScale(new Vector2(1, 1), panelScaleDuration));
            seq.Insert(0.25f, levelCompleteText.transform.DOScale(new Vector2(1.2f, 1.2f), levelCompleteDuration * 0.5f));
            seq.Insert(0.35f, levelCompleteText.transform.DOScale(new Vector2(1, 1), levelCompleteDuration));

            int currentStars = GameManagerMain.instance.currentLvlStarsEarned;

            for (int i = 0; i < currentStars; i++)
            {
                seq.Append(stars[i].transform.DOScale(new Vector2(1.25f, 1.25f), levelCompleteDuration * 0.5f));
                seq.Append(stars[i].transform.DOScale(new Vector2(1, 1), levelCompleteDuration * 0.75f));
            }

            nextLvlButton.transform.DOScale(new Vector2(1.1f, 1.1f), 1f).SetLoops(-1, LoopType.Yoyo);
        }
    }

    void FadeImages() {

        Debug.Log("Entered Fade Images");
        
        for (int i = 0; i < spritesSlidesList.Count; i++) {

            Debug.Log("i = " + i + " - slideChangeIndex = " + slideChangeIndex);

            if (i == slideChangeIndex) {

                Debug.Log("Entramos al i == slideChangeIndex");

                StartCoroutine(FadeSprites(spritesSlidesList[i], spritesSlidesList[i+1]));
                
                break;

            }

        }

        slideChangeIndex++;

    }

    IEnumerator FadeSprites(SlideSprites slideGroupFirst, SlideSprites slideGroupSecond)
    {
        foreach (SpriteRenderer sprite in slideGroupFirst.sprites) {
            sprite.DOFade(0f, 1f);
        }

        yield return new WaitForSeconds(1f);

        foreach (SpriteRenderer sprite in slideGroupSecond.sprites)
        {
            sprite.DOFade(1f, 1f);
        }

    }

    public void BubbleTextController(bool activateBubble) {

        Sequence seq = DOTween.Sequence();

        if (activateBubble)
        {
            seq.Append(bubbleContainer.transform.DOScale(1.05f, 0.25f));
            seq.Append(bubbleContainer.transform.DOScale(1.0f, 0.25f));
        }
        else {
            seq.Append(bubbleContainer.transform.DOScale(1.05f, 0.25f));
            seq.Append(bubbleContainer.transform.DOScale(0f, 0.25f));
        }
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!GameManagerMain.instance.playerCanInteractGame)
            {
                if (isWaitingForClick)
                {
                    BubbleAnimOnClick();
                    isWaitingForClick = false;

                    if (isSlideLevel && currentLine < dialogLines.Length - 1)
                    {
                        //Debug.Log("lineToSwitchImages[dialogIndexCounter] = " + lineToSwitchImages[dialogIndexCounter].ToString() + " - currentLine = " + currentLine);
                        
                        TypeWriter.WriteText(dialogLines[currentLine]);

                        if (lineToSwitchImages.Length-1 >= dialogIndexCounter) {

                            if (lineToSwitchImages[dialogIndexCounter] == currentLine)
                            {
                                dialogIndexCounter++;
                                FadeImages();
                            }
                        }

                        currentLine++;

                    }
                    else if (currentLine == dialogLines.Length - 1)
                    {
                        //Si el nivel es de slides, ir directo a la siguiente pantalla                        
                        GameManagerMain.instance.playerCanInteractGame = true;

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
        yield return new WaitForSeconds(0.5f);
        BubbleTextController(true);
        yield return new WaitForSeconds(0.5f);
        TypeWriter.WriteText(dialogLines[0]);
        currentLine++;
    }

    void SetWaitingForClickStatus()
    {
        GameManagerMain.instance.playerCanInteractGame = false;
        isWaitingForClick = true;
    }

    void BubbleAnimOnClick() {
        Sequence seqBubble = DOTween.Sequence();
        seqBubble.Append(bubbleContainer.transform.DOScale(new Vector2(1.05f, 1.05f), 0.25f));
        seqBubble.Append(bubbleContainer.transform.DOScale(new Vector2(1f, 1f), 0.25f));

    }

    void OpenVerticalTransition(GameObject layoutMenu, GameObject layoutObject) {
        
        returnFromHintBT.SetActive(true);
        returnFromPauseBT.SetActive(true);

        ingameLayout.SetActive(false);
        layoutObject.SetActive(true);
        Sequence pauseSeq = DOTween.Sequence();
        pauseSeq.Append(layoutMenu.transform.DOLocalMove(new Vector2(0, 0), moveDuration));
        pauseSeq.Insert(0.25f, layoutMenu.transform.DOScale(new Vector2(1, 0.85f), panelScaleDuration));
        pauseSeq.Append(layoutMenu.transform.DOScale(new Vector2(1, 1), panelScaleDuration));
    }

    void OpenVerticalTransitionHint(GameObject layoutMenu)
    {
        returnFromHintBT.SetActive(true);
        returnFromPauseBT.SetActive(true);

        ingameLayout.SetActive(false);
        hintImageLayout.SetActive(true);
        Sequence pauseSeq = DOTween.Sequence();
        pauseSeq.Append(layoutMenu.transform.DOLocalMove(new Vector2(0, 0), moveDuration));
        pauseSeq.Insert(0.25f, layoutMenu.transform.DOScale(new Vector2(1, 0.85f), panelScaleDuration));
        pauseSeq.Append(layoutMenu.transform.DOScale(new Vector2(1, 1), panelScaleDuration));
    }

    public void ReturnToGameFromPause(GameObject menu) {
        SoundsFX.instance.PlayClose();
        bgPause.SetActive(false);
        Sequence returnPauseSeq = DOTween.Sequence();
        returnPauseSeq.Append(menu.transform.DOLocalMove(new Vector2(0, -900), moveDuration * 0.5f));
        StartCoroutine(WaitToActivateIngameLayout());
    }

    public void ReturnToGameFromHint(GameObject menu)
    {
        SoundsFX.instance.PlayClose();
        bgHintImage.SetActive(false);
        bgHintPrompt.SetActive(false);
        Sequence returnPauseSeq = DOTween.Sequence();
        returnPauseSeq.Append(menu.transform.DOLocalMove(new Vector2(0, -600), moveDuration *0.5f));
        StartCoroutine(WaitToActivateIngameLayout());
    }

    IEnumerator WaitToActivateIngameLayout() {
        playerCanInteractUI = false;
        returnFromHintBT.SetActive(false);
        returnFromPauseBT.SetActive(false);
        ingameLayout.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        playerCanInteractUI = true;
        pauseLayout.SetActive(false);
        hintImageLayout.SetActive(false);
        hintQuestionLayout.SetActive(false);
        GameManagerMain.instance.playerCanInteractGame = true;

    }

    public void PauseMenu() {

        if (playerCanInteractUI) {
            SoundsFX.instance.PlayClick();
            GameManagerMain.instance.playerCanInteractGame = false;
            bgPause.SetActive(true);
            OpenVerticalTransition(pauseMenu, pauseLayout);
        }
    }

    public void HintMenu() {
        if (playerCanInteractUI) {
            SoundsFX.instance.PlayClick();
            GameManagerMain.instance.playerCanInteractGame = false;
            bgHintPrompt.SetActive(true); ;
            OpenVerticalTransition(hintQuestionMenu, hintQuestionLayout);
        }
    }
    public void HintImage()
    {
        if (playerCanInteractUI) {
            SoundsFX.instance.PlayHint();
            GameManagerMain.instance.playerCanInteractGame = false;
            bgHintPrompt.SetActive(false);
            bgHintImage.SetActive(true);
            hintQuestionMenu.transform.localPosition = new Vector2(0, -600);
            hintQuestionLayout.SetActive(false);
            OpenVerticalTransitionHint(hintImageMenu);
            GameManagerMain.instance.hasWatchedHint = true;
        }
    }

    public void ShowMenu(GameObject menu) {
        SoundsFX.instance.PlayClick();
        menu.SetActive(true);
    }

    public void HideMenu(GameObject menu)
    {
        SoundsFX.instance.PlayClose();
        menu.SetActive(false);
    }

    public void SkipLevel() {

        earnedStarsCurrent.text = Loader.TOTAL_STARS_EARNED.ToString();
        SoundsFX.instance.PlayClick();
        pauseLayout.SetActive(false);
        GameManagerMain.instance.SubstractLevelStars();
        GameManagerMain.instance.GoToNextLevel();
    }

    void SetUITexts() {

        if (hasTutorial) {
            howToPlayText.text = _lang["how_to_play"];
        }

        if (!hasTutorial && !isSlideLevel) {

            levelSelectionTxt.text = _lang["level_selection"];
            skipLevelTxt.text = _lang["skip_level"];

            //oneStarMovesTxt.text =  + " " + _lang["or_less_moves"];
            //twoStarMovesTxt.text = GameManagerMain.instance.lvlMidScoreMoves.ToString() + " " + _lang["or_less_moves"];
            //threeStarMovesTxt.text = GameManagerMain.instance.lvlMaxScoreMoves.ToString() + " " + _lang["or_less_moves"];

            //*This is wrong order, the correct is the commented code above, but because of time, in the editor we put the texts in the wrong order
            //so this is the "correct" order for now
            //*/
            oneStarMovesTxt.text = GameManagerMain.instance.lvlMaxScoreMoves.ToString() + " " + _lang["or_less_moves"];
            twoStarMovesTxt.text = GameManagerMain.instance.lvlMidScoreMoves.ToString() + " " + _lang["or_less_moves"];
            threeStarMovesTxt.text = GameManagerMain.instance.lvlMinScoreMoves.ToString() + " " + _lang["or_less_moves"];
            /******************************/

            skipLevelScreenText.text = _lang["skip_level_question"];
            currentStarsText.text = _lang["current_stars"];
            solutionQuestionText.text = _lang["watch_the_solution"];
            costYouText.text = _lang["cost_you"];
            wontEarnText.text = _lang["wont_earn_stars"];

            if (SceneManager.GetActiveScene().name != "MainMenu" && 
                SceneManager.GetActiveScene().name != "LevelSelector" && 
                SceneManager.GetActiveScene().name != "EndGame") {

                Loader.TOTAL_STARS_EARNED -= Loader.CURRENT_STARS_EARNED_PER_LEVEL[GameManagerMain.instance.levelId - 1];
                earnedStarsCurrent.text = Loader.TOTAL_STARS_EARNED.ToString();
            }

            if (Loader.TOTAL_STARS_EARNED < 3)
            {
                skipLevelButton.interactable = false;
            }
        }
    }

    IEnumerator WaitToStartTutorial() {
        yield return new WaitForSeconds(1.0f);
        tutorialLayout.SetActive(true);
    }

    void GetAllSlideSprites() {
                
        if (isSlideLevel) {

            spritesSlidesList = new List<SlideSprites>();

            for (int i = 0; i < slidesGroup.Length; i++)
            {
                SlideSprites slideElementSprite = new SlideSprites();
                slideElementSprite.sprites = new List<SpriteRenderer>();

                for (int j = 0; j < slidesGroup[i].transform.childCount; j++) {
                    slideElementSprite.sprites.Add(slidesGroup[i].transform.GetChild(j).gameObject.GetComponent<SpriteRenderer>()); 
                }

                slideElementSprite.slideIndex = i;
                spritesSlidesList.Add(slideElementSprite);
            }
        }
    }
}
