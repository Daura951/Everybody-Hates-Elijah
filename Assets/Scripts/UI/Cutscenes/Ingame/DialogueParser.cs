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
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    private Image leftImage, rightImage;
    private Image speechBubble;

    private bool canAdvanceDialogue = false;



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
            StartCoroutine(WaitForAnimation());
            isInit = false;
        }

        if((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space)) && isInCutscene && canAdvanceDialogue)
        {
            index++;
            ParseDialogue();
        }
    }

    private IEnumerator WaitForAnimation()
    {
        canAdvanceDialogue = false; // Disable input during animation

        for (int i = 0; i < 25; i++)
        {
            yield return null; // Waits for one frame
        }

        canAdvanceDialogue = true; // Allow user to progress dialogue
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
        nameText.text = "";
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
        SetDarknessofCharacters(splitTokens);
        nameText.text = splitTokens[0];

        if(nameText.text == characterLeft.name)
        {
            nameText.color = characterLeft.nameColor;
        }
        else
        {
            nameText.color = characterRight.nameColor;
        }

        dialogueText.text = splitTokens[1];
    }

    void SetDarknessofCharacters(string[] splitTokens)
    {
        Image darkenedImage = splitTokens[0] == characterRight.name ? leftImage : rightImage;
        Image lightImage = splitTokens[0] != characterRight.name ? leftImage : rightImage;
        darkenedImage.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        lightImage.color = new Color(1f, 1f, 1f, 1f);

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
                case "SpeechBubble":
                    speechBubble = child.GetComponent<Image>();
                    foreach (Transform dChild in speechBubble.transform)
                    {
                        switch (dChild.name)
                        {
                            case "DialogueText":
                                dialogueText = dChild.GetComponent<TMP_Text>();
                                break;

                            case "NameText":
                                nameText = dChild.GetComponent<TMP_Text>();
                                break;
                        }
                    }
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
