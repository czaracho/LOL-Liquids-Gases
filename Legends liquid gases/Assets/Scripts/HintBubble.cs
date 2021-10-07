using LoLSDK;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HintBubble : MonoBehaviour
{
    JSONNode _lang;
    public Text hintText;
    public string hintLabel;
    public bool autoClose = true;
    public float startDelay = 0.5f;

    void Start()
    {
        _lang = SharedState.LanguageDefs;
        hintText.text = _lang[hintLabel];
        GameManagerMain.instance.playerCanInteractGame = false;
        StartCoroutine(StartHint());

        if (autoClose) {
            StartCoroutine(CloseHint());
        }
    }

    public void CloseBubble() {
        SoundsFX.instance.PlayClose();
        GameManagerMain.instance.playerCanInteractGame = true;
        gameObject.SetActive(false);
    }


    public IEnumerator CloseHint() {
        yield return new WaitForSeconds(10f);
        CloseBubble();
    }

    public IEnumerator StartHint() {
        
        yield return new WaitForSeconds(startDelay);

        foreach (Transform child in transform)
        {
            if (!child.gameObject.active) {
                child.gameObject.SetActive(true);
            }
        }

        LOLSDK.Instance.SpeakText(hintLabel);
    }
}
