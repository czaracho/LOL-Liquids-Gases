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
    TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI textContinue;

    string textToType = "";
    string currentText = "";
    [HideInInspector]
    public bool isLastSlide = false;
    [HideInInspector]
    public bool canSpeedUptext = false;
    [HideInInspector]
    public float textspeed = 0.025f;

    JSONNode _lang;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        _lang = SharedState.LanguageDefs;
        HideAll();
        text.text = "";
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) {
            textspeed = 0.025f / 3f;
        }

        if (Input.GetMouseButtonUp(0)) {
            textspeed = 0.025f;

        }

    }

    public void HideAll()
    {
        StopAllCoroutines();
        text.text = "";
        currentText = "";
        textToType = "";
        textContinue.gameObject.SetActive(false);

        if (UIBehaviour.instance.Bubble != null)
        {
            UIBehaviour.instance.Bubble.SetActive(false);
        }
    }

    public void WriteText(string jsonKey)
    {
        canSpeedUptext = true;
        StopAllCoroutines();
        UIBehaviour.instance.Bubble.SetActive(true);
        textContinue.gameObject.SetActive(false);
        LOLSDK.Instance.SpeakText(jsonKey);
        text.text = "";
        currentText = "";
        textToType = _lang[jsonKey];
        StartCoroutine(TypeTextInBubble());
    }

    IEnumerator TypeTextInBubble()
    {
        while (currentText.Length != textToType.Length)
        {
            for (int i = 0; i < textToType.Length; i++)
            {
                yield return new WaitForSeconds(textspeed);
                SoundsFX.instance.PlayType();
                currentText += textToType[i];
                text.text = currentText;
            }
        }

        if (!isLastSlide) {
            StartCoroutine(WaitForClick());
        }
        else {
            StartCoroutine(WaitForLastSlide());
        }
    }


    IEnumerator WaitForClick()
    {
        yield return new WaitForSeconds(0.5f);
        
        textContinue.gameObject.SetActive(true);
        EventManager.instance.OnWaitingForClickTrigger();
    }

    IEnumerator WaitForLastSlide() {
        yield return new WaitForSeconds(0.5f);
        HideAll();
        EventManager.instance.OnWaitForNextLevelTrigger();
    }


    public void WriteNewText(string jsonKey)
    {
        StopAllCoroutines();
        UIBehaviour.instance.Bubble.SetActive(true);
        textContinue.gameObject.SetActive(false);
        LOLSDK.Instance.SpeakText(jsonKey);
        text.text = "";
        currentText = "";
        textToType = _lang[jsonKey];
        StartCoroutine(TypeText());
    }


    IEnumerator TypeText()
    {
        UIBehaviour.instance.Bubble.SetActive(true);
        while (currentText.Length != textToType.Length)
        {
            for (int i = 0; i < textToType.Length; i++)
            {
                yield return new WaitForSeconds(textspeed);
                SoundsFX.instance.PlayType();
                currentText += textToType[i];
                text.text = currentText;
            }
        }

        StartCoroutine(WaitForClick());
    }

}
