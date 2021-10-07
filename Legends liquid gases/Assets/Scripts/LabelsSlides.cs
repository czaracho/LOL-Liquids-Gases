using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LabelsSlides : MonoBehaviour
{
    JSONNode _lang;
    public string[] labelTextsId;
    public Text[] labelTexts;

    private void Start()
    {
        _lang = SharedState.LanguageDefs;

        for (int i = 0; i < labelTexts.Length; i++) { 
            labelTexts[i].text = _lang[labelTextsId[i]];
        }
    }
}
