using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogManager : MonoBehaviour
{

    [Header("Dialog UI")]
    [SerializeField] private GameObject dialogPanel;
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

    private void Start()
    {
        dialogIsPlaying = false;
        dialogPanel.SetActive(false);
    }

    private void Update()
    {
        if (!dialogIsPlaying)
        {
            return;
        }

        if (Input.GetKeyDown("e"))
        {
            Debug.Log("e was pressed");
            ContinueStory();
        }
    }

    public void EnterDialogMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogIsPlaying = true;
        dialogPanel.SetActive(true);

       
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
            Debug.Log("Current story text: " + dialogText.text);
        }
        else
        {
            ExitDialogMode();
        }
    }
}
