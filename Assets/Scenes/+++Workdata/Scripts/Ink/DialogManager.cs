using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogManager : MonoBehaviour
{

    [Header("Dialog UI")]
    [SerializeField] private GameObject dialogPanal;
    [SerializeField] private TextMeshProUGUI dialogText;

    private Story currentStory;
    private bool dialogIsPlaying;

    private static DialogManager instance;

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogWarning("Found more than one Dialog Manager in scene");
        }

        instance = this;
    }

    public static DialogManager GetInstance()
    {
        return instance;
    }

    private void start()
    {
        dialogIsPlaying = false;
    }

    private void Update()
    {
        if (!dialogIsPlaying)
        {
            return;
        }

        if (Input.GetKeyDown("e"))
        {
            ContinueStory();
        }
    }

    public void EnterDialogMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogIsPlaying = true;

        ContinueStory();
    }


    private void ExitDialogMode()
    {
        dialogIsPlaying = false;
        dialogText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogText.text = currentStory.Continue();
        }
        else
        {
            ExitDialogMode();
        }
    }
}
