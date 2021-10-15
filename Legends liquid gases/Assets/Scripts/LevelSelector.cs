using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelSelector : MonoBehaviour
{
    public int levelId = 0;
    public string level = "";
    public GameObject lockedIcon;
    public GameObject unlockedIcon;
    public GameObject[] starsIcon;
    public GameObject levelNumber;
    bool levelIsUnlocked = false;
    float[] delay = {0, 0.2f, 0.35f, 0.4f, 0.5f };
    float[] yMovement = {1.0f, 1.5f, 2.0f, 2.5f };

    // Start is called before the first frame update
    void Start()
    {
        CheckIfLevelUnlocked();
        StartCoroutine(WiggleBottle());
    }

    void CheckIfLevelUnlocked() {

        if (Loader.LEVELS_UNLOCKED[levelId-1]) {
            levelIsUnlocked = true;
            lockedIcon.SetActive(false);
            unlockedIcon.SetActive(true);
            levelNumber.SetActive(true);
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

    public void GoToSelectedLevel() {
        if (levelIsUnlocked) {
            UIBehaviour.instance.BouncyAnimationButton(this.gameObject);
            GameManagerMain.instance.nextLevel = level;
            GameManagerMain.instance.GoToNextLevelFromSlides();
        }
    }

    IEnumerator WiggleBottle() {
        int i = Random.Range(0, 4);
        yield return new WaitForSeconds(delay[i]);
        unlockedIcon.transform.DOMoveY(unlockedIcon.transform.position.y + yMovement[i] * 2.5f, 1f).SetLoops(-1, LoopType.Yoyo);
    }
}
