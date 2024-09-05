using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;


    private void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Debug.Log(inkJSON.text);
        }
    }
}
