using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;

public class LevelSelectionScreen : MonoBehaviour
{
    public Text levelsText;
    JSONNode _lang;

    private void Start()
    {
        _lang = SharedState.LanguageDefs;
        levelsText.text = _lang["levels"];

    }
}
