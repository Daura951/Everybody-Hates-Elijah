using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueParser : MonoBehaviour
{
    [Header("Parsing Components")]
    public TextAsset txtFile;
    private string[] splitLines;
    private int index;


    [Header("Cutscene Logical Components")]
    public bool isInCutscene = false;
    public bool isInit = false;


    public CutsceneTrigger cutsceneTrigger;
    public Character[] characters;

    [Header("UI Components")]
    private Character characterLeft, characterRight;
    private TMP_Text dialogueText;
    private Image leftImage, rightImage;
    private Image speechBubble;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if(isInit)
        {
            ParseDialogue();
            isInit = false;
        }

        if((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space)) && isInCutscene)
        {
            index++;
            ParseDialogue();
        }
    }

    public void ParseDialogue()
    {
        string currentLine = splitLines[index];

        if(currentLine == "END")
        {
            EndCutscene();
        }

        else if(currentLine.Contains("L=") || currentLine.Contains("R="))
        {
            SetCharacter(currentLine);
        }
        else
        {
            DisplayDialogue(currentLine);
        }

    }

    void EndCutscene()
    {
        index = 0;
        dialogueText.text = "";
        cutsceneTrigger.cutsceneEnded = true;
    }

    void SetCharacter(string currentLine)
    {
        string[] splitTokens = currentLine.Split('=');
        string position = splitTokens[0];
        string characterName = splitTokens[1].Trim();

        foreach (Character character in characters)
        {
            if (character.name == characterName)
            {
                if (position == "L")
                {
                    characterLeft = character;
                    leftImage.sprite = character.cutsceneImage;
                }
                else
                {
                    characterRight = character;
                    rightImage.sprite = character.cutsceneImage;
                }
                break;
            }
        }

        index++;
        ParseDialogue();
    }

    void DisplayDialogue(string currentLine)
    {
        string[] splitTokens = currentLine.Split(':');
        SetSpeechBubbleDirection(splitTokens);
        dialogueText.text = splitTokens[1];
    }

    void SetSpeechBubbleDirection(string[] splitTokens)
    {
        if (splitTokens[0] == characterRight.name)
        {
            speechBubble.transform.eulerAngles = new Vector2(0, 180);
        }
        else if (splitTokens[0] == characterLeft.name)
        {
            speechBubble.transform.eulerAngles = new Vector2(0, 0);
        }
    }

    public void InitalizeDialogueParser()
    {
        foreach (Transform child in cutsceneTrigger.getCutsceneUi().transform)
        {
            switch (child.name)
            {
                case "LeftFace":
                    leftImage = child.GetComponent<Image>();
                    break;
                case "RightFace":
                    rightImage = child.GetComponent<Image>();
                    break;
                case "DialogueText":
                    dialogueText = child.GetComponent<TMP_Text>();
                    break;
                case "SpeechBubble":
                    speechBubble = child.GetComponent<Image>();
                    break;
            }
        }

        splitLines = txtFile.text.Split('\n');
        for (int i = 0; i < splitLines.Length - 1; i++)
        {
            splitLines[i] = splitLines[i].Remove(splitLines[i].Length - 1, 1);
        }
    }
}
