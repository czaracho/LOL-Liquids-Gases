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
    JSONNode _lang;
    private int currentSlide = 1;
    public float transitionTime = 0.45f;
    private TutorialSlide[] tutorialSlides;

    void Awake()
    {
        _lang = SharedState.LanguageDefs;

        if (currentSlide == 1)
        {
            backBT.SetActive(false);
        }

        //for (int i = 0; i < slides.Length; i++)
        //{
        //    tutorialSlides[i] = slides[i].GetComponent<TutorialSlide>();
        //}
    }

    private void Start()
    {
        LevelManager.instance.playerCanInteractGame = false;
    }

    public void PreviousSlide() {

        slides[currentSlide-1].gameObject.GetComponent<TutorialSlide>().EndSlide();
        //tutorialSlides[currentSlide - 1].EndSlide();


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
        //tutorialSlides[currentSlide - 1].StartSlide();
        slides[currentSlide-1].GetComponent<TutorialSlide>().StartSlide();

    }

    public void NextSlide() {

        slides[currentSlide-1].gameObject.GetComponent<TutorialSlide>().EndSlide();
        //tutorialSlides[currentSlide - 1].EndSlide();


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
        //tutorialSlides[currentSlide - 1].StartSlide();

    }

    public string ReadSlide(string jsonKey) {

        LOLSDK.Instance.SpeakText(jsonKey);
        string textToType = _lang[jsonKey];

        return textToType;
    }

    public void CloseTutorial() {
        LevelManager.instance.playerCanInteractGame = true;
        UIBehaviour.instance.HideMenu(this.gameObject);
    }
}
