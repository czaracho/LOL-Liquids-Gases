using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private Queue<string> sentences;
    DialogModel levelDialogs = new DialogModel();
    public Dialogue dialogue;

    //ui
    public GameObject dialogBox;
    public Text characterNameText;
    public Text dialogText;

    private void Start()
    {
        string json = File.ReadAllText(Application.dataPath + "/Utils/dialogs.json");
        levelDialogs = JsonUtility.FromJson<DialogModel>(json);
        sentences = new Queue<string>();
        StartDialogue();
    }

    public void StartDialogue() {
        
        sentences.Clear();

        foreach (string sentenceId in dialogue.dialogueIds)
        {
            string dialogId = "lvl" + dialogue.level.Trim() + "_text" + (sentenceId).ToString();

            for (int i = 0; i < levelDialogs.dialogs.Length; i++) {

                if (levelDialogs.dialogs[i].id == dialogId) {
                    sentences.Enqueue(levelDialogs.dialogs[i].dialogText);
                }
            }            
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {

        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }
         
        string sentence = sentences.Dequeue();
        dialogText.text = sentence;
    }

    void EndDialogue() {
        Debug.Log("End of conversation");
    }
}


