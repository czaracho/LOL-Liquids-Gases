using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LoLSDK;
using SimpleJSON;
using DG.Tweening;


public class TutorialSlide : MonoBehaviour
{
    private Image slide;
    private Text slideText;
    public string jsonKey;
    private Tutorial tutorialParent;
    private Image handPointer;

    private void Awake()
    {
        tutorialParent = gameObject.GetComponentInParent<Tutorial>();
        slide = transform.Find("slide").GetComponent<Image>();
        slideText = transform.Find("slidetext").GetComponent<Text>();
        handPointer = transform.Find("hand").GetComponent<Image>();
    }

    private void Start()
    {
        StartSlide();
    }

    public void StartSlide() {
        StartCoroutine(WaitToStartSlide());
    }

    IEnumerator WaitToStartSlide() {
        yield return new WaitForSeconds(0.5f);
        slideText.text = tutorialParent.ReadSlide(jsonKey);
        slide.DOFade(1, tutorialParent.transitionTime);
        slideText.DOFade(1, tutorialParent.transitionTime);
        handPointer.DOFade(1, tutorialParent.transitionTime);
        handPointer.transform.DOScale(new Vector2(handPointer.transform.localScale.x * 0.9f, handPointer.transform.localScale.y * 0.9f), 0.75f).SetLoops(-1, LoopType.Yoyo);
    }

    public void EndSlide() {
        slide.DOFade(0, tutorialParent.transitionTime);
        slideText.DOFade(0, tutorialParent.transitionTime);
        handPointer.DOFade(0, tutorialParent.transitionTime);
        StartCoroutine(WaitToDeactivate());
    }

    IEnumerator WaitToDeactivate() {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
