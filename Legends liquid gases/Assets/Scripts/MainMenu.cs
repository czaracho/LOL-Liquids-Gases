﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public GameObject startButton;
    public GameObject continueButton;
    public Text startBtText;
    public Text continueBtText;
    JSONNode _lang;

    private void Start()
    {
        _lang = SharedState.LanguageDefs;
        startBtText.text = _lang["start"];
        continueBtText.text = _lang["continue"];

        if (Loader.LEVELS_UNLOCKED[1]) {
            continueButton.GetComponent<Button>().enabled = true;
        }
        else
        {
            Button button = continueButton.GetComponent<Button>();

            ColorBlock colors = button.colors;
            colors.normalColor = new Color32(125, 125, 125, 255);
            colors.highlightedColor = new Color32(125, 125, 125, 255);
            colors.pressedColor = new Color32(125, 125, 125, 255);
            colors.selectedColor = new Color32(125, 125, 125, 255);
            colors.disabledColor = new Color32(125, 125, 125, 255);
            button.colors = colors;
        }
    }

    public void StartGame() {
        SoundsFX.instance.PlayClick();
        EventManager.instance.OnButtonSimpleClick(startButton);
        Vector3 buttonScale = new Vector3(startButton.transform.localScale.x, startButton.transform.localScale.y);
        startButton.transform.DOScale(buttonScale * 0.95f, 0.1f).SetLoops(2, LoopType.Yoyo);
        GameManagerMain.instance.StartNewGame();
        DisableButtons();
    }

    public void ContinueGame() {

        if (Loader.LEVELS_UNLOCKED[1])
        {
            SoundsFX.instance.PlayClick();
            EventManager.instance.OnButtonSimpleClick(continueButton);
            Vector3 buttonScale = new Vector3(continueButton.transform.localScale.x, continueButton.transform.localScale.y);
            continueButton.transform.DOScale(buttonScale * 0.95f, 0.1f).SetLoops(2, LoopType.Yoyo);
            GameManagerMain.instance.ContinueGame();
            DisableButtons();
        }


    }

    void DisableButtons() {
        startButton.GetComponent<Button>().enabled = false;
        continueButton.GetComponent<Button>().enabled = false;
        startButton.SetActive(false);
        continueButton.SetActive(false);
    }
}   
