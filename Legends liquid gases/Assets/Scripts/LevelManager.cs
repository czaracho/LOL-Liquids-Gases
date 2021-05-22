using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    public int requiredDropQuantity = 100;
    public int currentDropQuantity = 0;
    public GameObject nextLevelLayout;
    public string nextLevel = "level2";

    public void AddWaterDrop() {
        currentDropQuantity++;

        if (currentDropQuantity == requiredDropQuantity) {
            nextLevelLayout.SetActive(true);
        }
    }

    public void goToNextLevel() {
        SceneManager.LoadScene(nextLevel);
    }

    public void restartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
