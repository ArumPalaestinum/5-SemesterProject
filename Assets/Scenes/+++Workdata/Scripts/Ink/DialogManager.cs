using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{

    [Header("Dialog UI")]
    [SerializeField] private Gameobject dialogPanal;
    [SerializeField] private TextMeshProUGUI dialogText;

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

}
