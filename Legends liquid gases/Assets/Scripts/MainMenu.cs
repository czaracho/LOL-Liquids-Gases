using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using DG.Tweening;
using LoLSDK;


public class MainMenu : MonoBehaviour
{
    //ProgressData progressData;
    public GameObject startButton;
    public GameObject continueButton;
    public Text startBtText;
    public Text continueBtText;
    JSONNode _lang;
    State<ProgressData> stateNew;

#pragma warning disable 0649
    [SerializeField, Header("Initial State Data")] ProgressData progressData;
#pragma warning restore 0649

    private void Start()
    {
        _lang = SharedState.LanguageDefs;
        startBtText.text = _lang["start"];
        continueBtText.text = _lang["continue"];


        ContinueButtonManager();    
    }

    public void StartGame() {

        SaveDataNewGame();
        DisableButtons();
    }

    /// <summary>
    /// Start a new fresh game reseting all player progress
    /// </summary>
    void SaveDataNewGame() {
        
        int[] TEMP_CURRENT_STARS_EARNED_PER_LEVEL = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //no stars earned at the start of the game
        bool[] TEMP_LEVELS_UNLOCKED = { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false }; //first level is always unlocked
        bool[] TEMP_LEVEL_PROGRESSED = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };


        ProgressData progressData = new ProgressData();
        progressData.CURRENT_PROGRESS = 0;
        progressData.TOTAL_STARS_EARNED = 0;           //total stars earned
        progressData.CURRENT_STARS_EARNED_PER_LEVEL = TEMP_CURRENT_STARS_EARNED_PER_LEVEL;
        progressData.LEVELS_UNLOCKED = TEMP_LEVELS_UNLOCKED; //first level is always unlocked
        progressData.LEVEL_PROGRESSED = TEMP_LEVEL_PROGRESSED;


        SoundsFX.instance.PlayClick();
        EventManager.instance.OnButtonSimpleClick(startButton);
        Vector3 buttonScale = new Vector3(startButton.transform.localScale.x, startButton.transform.localScale.y);
        startButton.transform.DOScale(buttonScale * 0.95f, 0.1f).SetLoops(2, LoopType.Yoyo);
        LOLSDK.Instance.SubmitProgress(0, Loader.CURRENT_PROGRESS, Loader.MAX_PROGRESS);
        LOLSDK.Instance.SaveState(progressData);
        GameManagerMain.instance.StartNewGame();

    }

    public void ContinueGame() {
        SoundsFX.instance.PlayClick();
        EventManager.instance.OnButtonSimpleClick(continueButton);
        Vector3 buttonScale = new Vector3(continueButton.transform.localScale.x, continueButton.transform.localScale.y);
        continueButton.transform.DOScale(buttonScale * 0.95f, 0.1f).SetLoops(2, LoopType.Yoyo);
        LOLSDK.Instance.SubmitProgress(stateNew.score, stateNew.currentProgress, stateNew.maximumProgress);
        GameManagerMain.instance.ContinueGame();
        DisableButtons();
    }

    void DisableButtons() {
        startButton.GetComponent<Button>().enabled = false;
        continueButton.GetComponent<Button>().enabled = false;
        startButton.SetActive(false);
        continueButton.SetActive(false);
    }

    void ContinueButtonManager()
    {
        LOLSDK.Instance.LoadState<ProgressData>(state =>
        {
            if (state != null)
            {
                if (state.data.LEVELS_UNLOCKED[1])
                {
                    continueButton.gameObject.SetActive(true);
                    stateNew = new State<ProgressData>();
                    stateNew.score = state.score;
                    stateNew.currentProgress = state.currentProgress;
                    stateNew.maximumProgress = state.maximumProgress;
                    progressData = state.data;
                    LoadLoader(progressData);
                }
            }

        });        
    }

    void LoadLoader(ProgressData progressData) {
        Loader.CURRENT_PROGRESS = progressData.CURRENT_PROGRESS;
        Loader.TOTAL_STARS_EARNED = progressData.TOTAL_STARS_EARNED;
        Loader.CURRENT_STARS_EARNED_PER_LEVEL = progressData.CURRENT_STARS_EARNED_PER_LEVEL;
        Loader.LEVELS_UNLOCKED = progressData.LEVELS_UNLOCKED;
        Loader.LEVEL_PROGRESSED = progressData.LEVEL_PROGRESSED;
    }
}   
