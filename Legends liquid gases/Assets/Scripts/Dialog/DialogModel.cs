using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogModel
{
    public Dialog[] dialogs;
} 

[Serializable]
public class Dialog {
    public string id;
    public string dialogText;
}
