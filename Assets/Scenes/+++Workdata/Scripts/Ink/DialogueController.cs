using Ink;
using Ink.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueController : MonoBehaviour
{
    const string speakerTag = "speaker";

    public static Action<string> InkEvent;

    [Header("Ink")]
    [SerializeField] TextAsset inkAsset;

    [SerializeField] DialogueBox dialogueBox;

    Story inkStory;
    public static DialogueController instance;
    private void Awake()
    {
        instance = this;

        inkStory = new Story(inkAsset.text);
        inkStory.onError += OnInkError;
        inkStory.BindExternalFunction<string>("Event", Event);
    }
    private void Start()
    {
        dialogueBox.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        dialogueBox.DialogueContinued += OnDialogueContinued;
        dialogueBox.ChoiceSelected += OnChoiceSelected;
    }

    void OnDisable()
    {
        dialogueBox.DialogueContinued -= OnDialogueContinued;
        dialogueBox.ChoiceSelected -= OnChoiceSelected;
    }
    void OnDestroy()
    {
        inkStory.onError -= OnInkError;
    }

    #region DialogueLifeCycle
    public void StartDialogue(string dialoguePath)
    {
        OpenDialogue();
        inkStory.ChoosePathString(dialoguePath);
        ContinueDialogue();
    }

    void ContinueDialogue()
    {
        if (IsAtEnd())
        {
            CloseDialogue();
            return;
        }
        DialogueLine dialogueLine = new();

        if (CanContinue())
        {
            string inkLine = inkStory.Continue();
            if (string.IsNullOrWhiteSpace(inkLine))
            {
                ContinueDialogue();
                return;
            }
            dialogueLine.text = inkLine;
        }
        dialogueLine.choices = inkStory.currentChoices;

        if (inkStory.currentTags?[0] != null)
        {
            dialogueLine = HandleTags(inkStory.currentTags, dialogueLine);
        }

        dialogueBox.DisplayText(dialogueLine);
    }

    void OpenDialogue()
    {
        dialogueBox.gameObject.SetActive(true);
    }

    void CloseDialogue()
    {
        EventSystem.current.SetSelectedGameObject(null);
        dialogueBox.gameObject.SetActive(false);
    }

    void SelectChoice(int choiceIndex)
    {
        inkStory.ChooseChoiceIndex(choiceIndex);
        ContinueDialogue();
    }
    #endregion

    #region Ink
    DialogueLine HandleTags(List<string> currentTags, DialogueLine dialogueLine)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError(tag + " hasnt been split correctly");
                break;
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case speakerTag:
                    dialogueLine.speaker = tagValue;
                    break;
                default:
                    Debug.LogError("There is no " + tag + " in this switchcase", gameObject);
                    break;
            }
        }
        return dialogueLine;
    }

    bool CanContinue()
    {
        return inkStory.canContinue;
    }
    bool HasChoice()
    {
        return inkStory.currentChoices.Count > 0;
    }
    bool IsAtEnd()
    {
        return !CanContinue() && !HasChoice();
    }

    void OnInkError(string message, ErrorType type)
    {
        switch (type)
        {
            case ErrorType.Author:
                Debug.LogError(message);
                break;
            case ErrorType.Warning:
                Debug.LogWarning(message);
                break;
            case ErrorType.Error:
                Debug.LogError(message);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    void OnChoiceSelected(DialogueBox dialogueBox, int choiceIndex)
    {
        SelectChoice(choiceIndex);
    }
    void OnDialogueContinued(DialogueBox dialogueBox)
    {
        ContinueDialogue();
    }
    void Event(string eventName)
    {
        InkEvent?.Invoke(eventName);
    }
    #endregion
}

public struct DialogueLine
{
    public string speaker;

    public string text;

    public List<Choice> choices;
}
