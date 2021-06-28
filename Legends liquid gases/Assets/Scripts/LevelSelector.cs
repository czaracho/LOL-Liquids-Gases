using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public int levelId = 0;
    public string level = "";
    public GameObject lockedIcon;
    public GameObject unlockedIcon;
    public GameObject[] starsIcon;

    // Start is called before the first frame update
    void Start()
    {
        if (levelId == 1) {

            for (int i = 0; i < Loader.CURRENT_STARS_EARNED_PER_LEVEL.Length; i++) {
                Debug.Log(Loader.CURRENT_STARS_EARNED_PER_LEVEL[i]);
            }
        }

        CheckIfLevelUnlocked();   
    }

    void CheckIfLevelUnlocked() {

        if (levelId <= Loader.TOTAL_LEVELS_UNLOCKED) {
            lockedIcon.SetActive(false);
            unlockedIcon.SetActive(true);
            GetLevelStars();
        }
    }

    void GetLevelStars() {

        for (int i = 0; i < Loader.CURRENT_STARS_EARNED_PER_LEVEL.Length; i++)
        {
            if (levelId == i+1)
            {
                ShowStars(Loader.CURRENT_STARS_EARNED_PER_LEVEL[i]);
            }
        }
    }

    void ShowStars(int starsEarned) {

        for (int i = 0; i < starsEarned; i++) {
            starsIcon[i].SetActive(true);
        }
    }

    void GoToSelectedLevel() {
        LevelManager.instance.nextLevel = level;
        LevelManager.instance.GoToNextLevelFromSlides();
    }
}
