using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Spine.Unity;
using LoLSDK;
using SimpleJSON;

public class TypeWriter : MonoBehaviour
{
    //public SkeletonGraphic Star;
    public GameObject Bar;

    TextMeshProUGUI text;
    //[SerializeField] private TextMeshProUGUI textContinue;
    //[SerializeField] private TextMeshProUGUI textFinished;
    public string JSONKey = "empty";

    string textToType = "";
    string currentText = "";

    private bool _isGameSpace = false;

    JSONNode _lang;

    //[SerializeField] private GameObject CatGray;
    //[SerializeField] private GameObject CatNormal;
    //[SerializeField] private GameObject CatSpaceship;
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        _lang = SharedState.LanguageDefs;
        HideText();
        text.text = "";
    }

    public void SetSpaceGame()
    {
        _isGameSpace = true;
        HideAll();
    }

    public void ShowCatGray()
    {
        HideAll();
        //CatGray.SetActive(true);
    }

    public void ShowCatNormal()
    {
        HideAll();
        //CatNormal.SetActive(true);
    }

    public void ShowCatSpaceship()
    {
        HideAll();
        //CatSpaceship.SetActive(true);
    }

    public void ShowStar()
    {
        HideAll();
        //Star.gameObject.SetActive(true);
    }

    public void HideAll()
    {
        //Star.gameObject.SetActive(false);
        //CatGray.SetActive(false);
        //CatNormal.SetActive(false);
        //CatSpaceship.SetActive(false);
    }



    private void Start()
    {
        //EventsManager.instance.HasClickedAfterWaitingTrigger += HideText;
        //Star.AnimationState.SetAnimation(0, "idle", true);

    }

    private void OnDestroy()
    {
        //EventsManager.instance.HasClickedAfterWaitingTrigger -= HideText;
        StopAllCoroutines();
    }

    private void HideText()
    {
        StopAllCoroutines();
        text.text = "";
        currentText = "";
        textToType = "";
        //textContinue.gameObject.SetActive(false);

        if (Bar != null)
        {
            Bar?.SetActive(false);
        }

        if (_isGameSpace)
        {
            HideAll();
        }
        else
        {

        }
    }

    public void WriteTextGameSpace(string jsonKey)
    {
        _isGameSpace = true;
        StopAllCoroutines();
        Bar?.SetActive(true);
        //textContinue.gameObject.SetActive(false);
        LOLSDK.Instance.SpeakText(jsonKey);
        text.text = "";
        currentText = "";
        textToType = _lang[jsonKey];
        StartCoroutine(TypeTextSpaceGame());
    }

    IEnumerator TypeTextSpaceGame()
    {
        Bar?.SetActive(true);
        while (currentText.Length != textToType.Length)
        {
            for (int i = 0; i < textToType.Length; i++)
            {
                yield return new WaitForSeconds(0.025f);
                SoundsFX.instance.PlayType();
                currentText += textToType[i];
                text.text = currentText;
            }

        }

        StartCoroutine(WaitForClick());
    }

    IEnumerator WaitForClick()
    {
        yield return new WaitForSeconds(3f);
        //textContinue.gameObject.SetActive(true);
        //EventsManager.instance.OnWaitingForClickTrigger();
    }

    public void WriteNewText(string jsonKey)
    {
        StopAllCoroutines();
        Bar?.SetActive(true);
        //textContinue.gameObject.SetActive(false);
        //Star.AnimationState.SetAnimation(0, "talking", true);
        LOLSDK.Instance.SpeakText(jsonKey);
        text.text = "";
        currentText = "";
        textToType = _lang[jsonKey];
        StartCoroutine(TypeText());
    }

    public void ShowFinished()
    {
        StopAllCoroutines();
        Bar?.SetActive(true);
        //textContinue.gameObject.SetActive(false);
        //Star.AnimationState.SetAnimation(0, "talking", true);
        LOLSDK.Instance.SpeakText("gameFinished");
        //textFinished.gameObject.SetActive(true);
        //textFinished.text = _lang["gameFinished"];
        //textContinue.gameObject.SetActive(true);
        //EventsManager.instance.OnWaitingForClickTrigger();
    }

    public void WriteNewTextAndHide(string jsonKey)
    {
        StopAllCoroutines();
        //textContinue.gameObject.SetActive(false);
        //Star.AnimationState.SetAnimation(0, "talking", true);
        LOLSDK.Instance.SpeakText(jsonKey);
        Bar?.SetActive(true);
        text.text = "";
        currentText = "";
        textToType = _lang[jsonKey];
        StartCoroutine(TypeTextAndHide());
    }

    IEnumerator ClearText()
    {
        while (currentText.Length > 0)
        {
            yield return new WaitForSeconds(0.001f);
            currentText = currentText.Substring(0, currentText.Length - 1);
            text.text = currentText;
        }
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        Bar?.SetActive(true);
        while (currentText.Length != textToType.Length)
        {
            for (int i = 0; i < textToType.Length; i++)
            {
                yield return new WaitForSeconds(0.025f);
                SoundsFX.instance.PlayType();
                currentText += textToType[i];
                text.text = currentText;
            }

        }
        //Star.AnimationState.SetAnimation(0, "idle", true);
        StartCoroutine(WaitForClick());
    }

    IEnumerator TypeTextAndHide()
    {
        while (currentText.Length != textToType.Length)
        {
            for (int i = 0; i < textToType.Length; i++)
            {
                yield return new WaitForSeconds(0.025f);
                SoundsFX.instance.PlayType();
                currentText += textToType[i];
                text.text = currentText;
            }

        }
        //Star.AnimationState.SetAnimation(0, "idle", true);
        yield return new WaitForSeconds(7f);
        text.text = "";
        currentText = "";
        textToType = "";
        Bar?.SetActive(false);
    }
}
