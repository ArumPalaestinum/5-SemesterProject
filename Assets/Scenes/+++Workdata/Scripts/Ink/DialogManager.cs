using Ink.Runtime;
using TMPro;
using UnityEngine;

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
        //if(dialogIsPlaying) return;

        currentStory = new Story(inkJSON.text);
        dialogIsPlaying = true;
        dialogPanel.SetActive(true);

        ContinueStory();
    }


    private void ExitDialogMode()
    {
        dialogIsPlaying = false;
        dialogText.text = "";
        dialogPanel.SetActive(false);
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogText.text = currentStory.Continue();
            Debug.Log("Current story text:" + dialogText.text);
        }
        else
        {
            ExitDialogMode();
        }
    }
}
