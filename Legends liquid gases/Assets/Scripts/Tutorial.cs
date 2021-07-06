using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;

public class Tutorial : MonoBehaviour
{
    public GameObject[] slides;
    public GameObject backBT;
    public GameObject nextBT;
    public GameObject closeButton;
    JSONNode _lang;
    private int currentSlide = 1;
    public float transitionTime = 0.45f;

    void Awake()
    {
        _lang = SharedState.LanguageDefs;

        if (currentSlide == 1)
        {
            backBT.SetActive(false);
        }
    }

    private void Start()
    {
        LevelManager.instance.playerCanInteractGame = false;
    }

    public void PreviousSlide() {

        SoundsFX.instance.PlayClick();
        EventManager.instance.OnButtonSimpleClick(backBT);

        slides[currentSlide-1].gameObject.GetComponent<TutorialSlide>().EndSlide();

        if (currentSlide > 1) {
            currentSlide--;
        }

        if (currentSlide == 1) {
            backBT.SetActive(false);
        }
        else {
            nextBT.SetActive(true);
        }

        slides[currentSlide-1].SetActive(true);
        slides[currentSlide-1].GetComponent<TutorialSlide>().StartSlide();

    }

    public void NextSlide() {

        SoundsFX.instance.PlayClick();
        EventManager.instance.OnButtonSimpleClick(backBT);

        slides[currentSlide-1].gameObject.GetComponent<TutorialSlide>().EndSlide();

        if (currentSlide < slides.Length){
            currentSlide++;
        }

        if (currentSlide == slides.Length)
        {
            nextBT.SetActive(false);
        }
        else {
            backBT.SetActive(true);
        }

        slides[currentSlide - 1].SetActive(true);
        slides[currentSlide - 1].GetComponent<TutorialSlide>().StartSlide();

    }

    public string ReadSlide(string jsonKey) {

        LOLSDK.Instance.SpeakText(jsonKey);
        string textToType = _lang[jsonKey];

        return textToType;
    }

    public void CloseTutorial() {
        LevelManager.instance.playerCanInteractGame = true;
        UIBehaviour.instance.HideMenu(this.gameObject);
        UIBehaviour.instance.ingameLayout.SetActive(true);
    }
}
