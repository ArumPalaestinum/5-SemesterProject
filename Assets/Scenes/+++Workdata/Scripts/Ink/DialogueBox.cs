using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public event Action<DialogueBox> DialogueContinued;
    public event Action<DialogueBox, int> ChoiceSelected;


    [SerializeField] TextMeshProUGUI dialogueSpeaker;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Button continueButton;

    [Header("Choices")]
    [SerializeField] Transform choiceContainer;
    [SerializeField] Button choiceButtonPrefab;

    Coroutine displayLineCoroutine;

    void Awake()
    {
        continueButton.onClick.AddListener(() => { DialogueContinued?.Invoke(this); });
    }

    void OnEnable()
    {
        dialogueSpeaker.SetText(string.Empty);
        dialogueText.SetText(string.Empty);
    }

    public void DisplayText(DialogueLine dialogueLine)
    {
        if (dialogueLine.speaker != null)
        {
            dialogueSpeaker.SetText(dialogueLine.speaker);
        }

        if (displayLineCoroutine != null)
            StopCoroutine(displayLineCoroutine);

        displayLineCoroutine = StartCoroutine(DisplayLine(dialogueLine.text));

        DisplayButtons(dialogueLine.choices);
    }

    IEnumerator DisplayLine(String line)
    {
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;

            yield return new WaitForSeconds(0.05f);
        }
    }

    void DisplayButtons(List<Choice> choices)
    {
        Selectable newSelection;

        if (choices == null || choices.Count == 0)
        {
            ShowContinueButton(true);
            Showchoices(false);
            newSelection = continueButton;
        }
        else
        {
            ClearChoices();
            List<Button> choiceButtons = GenerateChoices(choices);

            ShowContinueButton(false);
            Showchoices(true);
            newSelection = choiceButtons[0];
        }
        StartCoroutine(DelayedSelection(newSelection));
    }
    void ClearChoices()
    {
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowContinueButton(bool show)
    {
        continueButton.gameObject.SetActive(show);
    }

    public void Showchoices(bool show)
    {
        choiceContainer.gameObject.SetActive(show);
    }

    List<Button> GenerateChoices(List<Choice> choices)
    {
        List<Button> choiceButtons = new List<Button>(choices.Count);

        for (int i = 0; i < choices.Count; i++)
        {
            Choice choice = choices[i];
            Button button = Instantiate(choiceButtonPrefab, choiceContainer);

            int index = i;
            button.onClick.AddListener(() => ChoiceSelected?.Invoke(this, index));


            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.SetText(choice.text);
            button.name = choice.text;

            choiceButtons.Add(button);
        }

        return choiceButtons;
    }


    IEnumerator DelayedSelection(Selectable selectable)
    {
        yield return null;
        selectable.Select();
    }
}
