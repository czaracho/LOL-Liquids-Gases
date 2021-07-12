using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LoLSDK;

public class GameManagerMain : MonoBehaviour
{
    public int levelId = 0;
    public bool waterIsOrange = false;
    [HideInInspector]
    public bool levelIsReseting = false;
    [HideInInspector]
    public bool playerCanInteractGame = true;
    [HideInInspector]
    private bool waterIsRunning = false;
    [HideInInspector]
    public bool hasWatchedHint = false;
    [HideInInspector]
    public bool levelCleared = false;
    public int requiredDropQuantity = 100;   
    [HideInInspector]
    public int currentDropQuantity = 0;
    public string nextLevel = "";
    public static GameManagerMain instance;
    [HideInInspector]
    public Piece[] pieces;
    public int lvlMaxScoreMoves = 5;
    public int lvlMidScoreMoves = 7;
    public int lvlMinScoreMoves = 10;
    public int currentLvlStarsEarned = 0;
    public Text currentMovesText;
    string currentLevel = "";

    [HideInInspector]
    public int currentLvlMoves = 0;

    private void Awake()
    {
        if (instance != null) {
            return;
        }
        instance = this;

    }

    private void Start()
    {
        EventManager.instance.WaitForNextLevelTrigger += GoToNextLevelFromSlides;
        pieces = FindObjectsOfType<Piece>();
        currentLevel = SceneManager.GetActiveScene().name;
        //Check the UI Manager
        //Loader.TOTAL_STARS_EARNED -= Loader.CURRENT_STARS_EARNED_PER_LEVEL[levelId - 1];
        //earnedStarsCurrent.text = Loader.TOTAL_STARS_EARNED.ToString();
    }

    private void OnDestroy()
    {
        EventManager.instance.WaitForNextLevelTrigger -= GoToNextLevelFromSlides;
    }

    public void AddWaterDrop() {

        if (!waterIsRunning) {
            waterIsRunning = true;
        }

        currentDropQuantity++;

        if (currentDropQuantity == requiredDropQuantity) {


            if (currentLevel == "tutorialStart" || currentLevel == "tutorialBurner" || currentLevel == "tutorialCondenser")
            {
                EventManager.instance.OnStopWaterTankSoundTrigger();
                UIBehaviour.instance.FadeTo(nextLevel);
            }
            else {
                checkStarScore();
                UIBehaviour.instance.toNextLevelTransition();
                levelCleared = true;
                EventManager.instance.OnStopWaterTankSoundTrigger();
                EventManager.instance.OnPlayCatAnimationTrigger("celebration");
            }

        }
    }

    public void GoToNextLevel()
    {
        if (nextLevel == "EndGame") {
            EndGame();
            return;
        }

        playerCanInteractGame = false;
        UIBehaviour.instance.PlayBouncyAnimation("nextLevel");
        UIBehaviour.instance.FadeTo(nextLevel);
    }

    public void restartLevel()
    {
        levelIsReseting = true;

        if (!UIBehaviour.instance.hasTutorial) {
            //If level is a tutorial level, restarting won't take stars from the student
            Loader.CURRENT_STARS_EARNED_PER_LEVEL[levelId - 1] = 0;
            Loader.TOTAL_STARS_EARNED -= currentLvlStarsEarned;
            LOLSDK.Instance.SubmitProgress(Loader.TOTAL_STARS_EARNED, Loader.CURRENT_PROGRESS, Loader.MAX_PROGRESS);
            Loader.SaveData();
        }

        playerCanInteractGame = false;
        UIBehaviour.instance.PlayBouncyAnimation("restart");
        UIBehaviour.instance.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void DeactivatePieces() {
        foreach (Piece piece in pieces) {
            piece.DeactivatePiece();
        }
    }

    public void addMoveCounter() {
        currentLvlMoves = currentLvlMoves + 1;
        currentMovesText.text = currentLvlMoves.ToString();
    }

    void checkStarScore() {

        //If the student watched the hint, stars can't be earned for this level
        if (!hasWatchedHint)
        {
            if (currentLvlMoves <= lvlMaxScoreMoves)
            {
                currentLvlStarsEarned = 3;
            }
            else if (currentLvlMoves <= lvlMidScoreMoves && currentLvlMoves > lvlMaxScoreMoves)
            {
                currentLvlStarsEarned = 2;
            }
            else if (currentLvlMoves <= lvlMinScoreMoves && currentLvlMoves > lvlMidScoreMoves)
            {
                currentLvlStarsEarned = 1;
            }
            else if (currentLvlMoves > lvlMinScoreMoves)
            {
                currentLvlStarsEarned = 0;
            }
        }

        AddGameProgress();

    }

    public void RestartToNonSelectedPiece()
    {
        foreach (Piece piece in pieces)
        {
            if (piece.isSelected) {
                piece.isSelected = false;
                piece.CancelPulsatingAnimation();
                piece.ResumeFloatAnimation();
            }
        }
    }

    public void GoToNextLevelFromSlides() {

        Loader.CURRENT_PROGRESS++;
        LOLSDK.Instance.SubmitProgress(Loader.TOTAL_STARS_EARNED, Loader.CURRENT_PROGRESS, Loader.MAX_PROGRESS);
        Loader.SaveData();

        UIBehaviour.instance.FadeTo(nextLevel);
    }


    public void GoToLevelSelectionScreen() {
        UIBehaviour.instance.pauseLayout.SetActive(false);
        SoundsFX.instance.PlayClick();
        UIBehaviour.instance.FadeTo("LevelSelector");
    }

    public void StartNewGame() {
        int[] currentStarsUnlockedTemp = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }; //no stars at the start of the game
        bool[] levelsUnlockedTemp = { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false }; //first level is always unlocked
        Loader.CURRENT_PROGRESS = 0;
        Loader.TOTAL_STARS_EARNED = 0;
        Loader.CURRENT_STARS_EARNED_PER_LEVEL = currentStarsUnlockedTemp;
        Loader.LEVELS_UNLOCKED = levelsUnlockedTemp;
        LOLSDK.Instance.SubmitProgress(Loader.TOTAL_STARS_EARNED, Loader.CURRENT_PROGRESS, Loader.MAX_PROGRESS);

        SaveData();
        UIBehaviour.instance.FadeTo(nextLevel); //always has to be slide1
    }

    void SaveData()
    {
        ProgressData progressData = new ProgressData();
        progressData.CURRENT_PROGRESS = Loader.CURRENT_PROGRESS;
        progressData.TOTAL_STARS_EARNED = Loader.TOTAL_STARS_EARNED;
        progressData.CURRENT_STARS_EARNED_PER_LEVEL = Loader.CURRENT_STARS_EARNED_PER_LEVEL;

        LOLSDK.Instance.SaveState(progressData);
    }

    public void ContinueGame() {       
        LOLSDK.Instance.SubmitProgress(Loader.TOTAL_STARS_EARNED, Loader.CURRENT_PROGRESS, Loader.MAX_PROGRESS);
        SoundsFX.instance.PlayClick();
        UIBehaviour.instance.FadeTo("LevelSelector"); //always has to be LevelSelector
    }

    void AddGameProgress() {

        Loader.CURRENT_STARS_EARNED_PER_LEVEL[levelId - 1] = currentLvlStarsEarned; //This is set to 0 at the start of the level
        Loader.TOTAL_STARS_EARNED += currentLvlStarsEarned;

        //If is the first time the level is beaten, then add progress only the first time
        if (Loader.LEVEL_PROGRESSED[levelId - 1] == false) {
            Loader.LEVEL_PROGRESSED[levelId - 1] = true;
            Loader.CURRENT_PROGRESS++;
        }
        
        Loader.LEVELS_UNLOCKED[levelId] = true; //levelId of current level, since arrays id starts at 0
        LOLSDK.Instance.SubmitProgress(Loader.TOTAL_STARS_EARNED, Loader.CURRENT_PROGRESS, Loader.MAX_PROGRESS);
        Loader.SaveData();

        Debug.Log("LOL-Liquids - " + "CURRENT LEVEL = " + SceneManager.GetActiveScene().name);
        Debug.Log("LOL-Liquids - " + "TOTAL STARS EARNED FOR NOW: " + Loader.TOTAL_STARS_EARNED.ToString());
        Debug.Log("LOL-Liquids - " + "CURRENT PROGRESS = " + Loader.CURRENT_PROGRESS.ToString());
    }

    public void EndGame() {
        MusicController.instance.StopMainSong();
        LOLSDK.Instance.CompleteGame();
        UIBehaviour.instance.FadeTo(nextLevel);
    }

    public void DebugLevel(string level) {
        UIBehaviour.instance.FadeTo(level);
    }


    public void SubstractLevelStars() {
        Loader.CURRENT_STARS_EARNED_PER_LEVEL[levelId - 1] = currentLvlStarsEarned;
        Loader.TOTAL_STARS_EARNED -= 3;
        LOLSDK.Instance.SubmitProgress(Loader.TOTAL_STARS_EARNED, Loader.CURRENT_PROGRESS, Loader.MAX_PROGRESS);
        Loader.SaveData();
    }
}
