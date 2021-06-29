using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public int levelId = 0;
    [HideInInspector]
    public bool playerCanInteract = true;
    [HideInInspector]
    public bool levelCleared = false;
    public int requiredDropQuantity = 100;
    [HideInInspector]
    public int currentDropQuantity = 0;
    public string nextLevel = "";
    public static LevelManager instance;
    [HideInInspector]
    public Piece[] pieces;
    public int lvlMaxScoreMoves = 5;
    public int lvlMidScoreMoves = 7;
    public int lvlMinScoreMoves = 10;
    public int currentLvlStarsEarned = 0;
    public Text currentMovesText;

    [HideInInspector]
    public int currentLvlMoves = 0;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    private void Start()
    {
        EventManager.instance.WaitForNextLevelTrigger += GoToNextLevelFromSlides;
        pieces = FindObjectsOfType<Piece>();

    }

    private void OnDestroy()
    {
        EventManager.instance.WaitForNextLevelTrigger -= GoToNextLevelFromSlides;
    }

    public void AddWaterDrop() {
        
        currentDropQuantity++;

        if (currentDropQuantity == requiredDropQuantity) {
            checkStarScore();
            UIBehaviour.instance.toNextLevelTransition();
            levelCleared = true;
        }
    }

    public void goToNextLevel()
    {
        playerCanInteract = false;
        UIBehaviour.instance.PlayBouncyAnimation("nextLevel");
        UIBehaviour.instance.FadeTo(nextLevel);
    }

    public void restartLevel()
    {
        playerCanInteract = false;
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
        else if (currentLvlMoves > lvlMinScoreMoves) {
            currentLvlStarsEarned = 0;
        }

        //TODO agregar aca la sumatoria de estrellas 
        //GameMaster.instance.total_stars_earned = GameMaster.instance.total_stars_earned + currentLvlStarsEarned;
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
        UIBehaviour.instance.FadeTo(nextLevel);
    }
}
