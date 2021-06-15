using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public bool waterIsPumped = false;
    public int requiredDropQuantity = 100;
    public int currentDropQuantity = 0;
    public GameObject ingameLayout;
    public GameObject nextLevelLayout;
    public string nextLevel = "level2";
    public static LevelManager instance;
    private Piece[] pieces;
    public int lvlMaxScoreMoves = 5;
    public int lvlMidScoreMoves = 7;
    public int lvlMinScoreMoves = 10;
    public int currentLvlStarsEarned = 0;
    public Text currentMovesText;
    public Text currentLevelStarsEarned;

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
        waterIsPumped = false;
        pieces = FindObjectsOfType<Piece>();
        currentMovesText.text = "Current Moves = 0";
    }

    public void AddWaterDrop() {
        currentDropQuantity++;

        if (currentDropQuantity == requiredDropQuantity) {
            ingameLayout.SetActive(false);
            nextLevelLayout.SetActive(true);
            checkStarScore();
        }
    }

    public void goToNextLevel() {
        SceneManager.LoadScene(nextLevel);
    }

    public void restartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DeactivatePieces() {
        foreach (Piece piece in pieces) {
            piece.DeactivatePiece();
        }
    }

    public void addMoveCounter() {
        currentLvlMoves = currentLvlMoves + 1;
        currentMovesText.text = "Current Moves = " + currentLvlMoves.ToString();
    }

    void checkStarScore() {

        if (currentLvlMoves <= lvlMaxScoreMoves)
        {
            currentLvlStarsEarned = 3;
            currentLevelStarsEarned.text = "Stars = 3";
        }
        else if (currentLvlMoves <= lvlMidScoreMoves)
        {
            currentLvlStarsEarned = 2;
            currentLevelStarsEarned.text = "Stars = 2";
        }
        else if (currentLvlMoves <= lvlMinScoreMoves)
        {
            currentLvlStarsEarned = 1;
            currentLevelStarsEarned.text = "Stars = 1";
        }
        else if (currentLvlMoves > lvlMinScoreMoves) {
            currentLvlStarsEarned = 0;
            currentLevelStarsEarned.text = "Stars = 0";
        }
    }
}
