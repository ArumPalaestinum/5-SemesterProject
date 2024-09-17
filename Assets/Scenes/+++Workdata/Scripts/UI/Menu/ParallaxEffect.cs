using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ParallaxEffect : MonoBehaviour
{
    public Transform[] parallaxLayers;
    public float[] parallaxScales;
    public float smoothing = 1f;

    private Vector2 previousMousePosition;
    private Vector2 mouseDelta;
    private InputAction mouseMovementAction;

    private bool initialized = false;

    

    private void Awake()
    {
        var inputActions = new Controlls();
        mouseMovementAction = inputActions.Player.MouseMovement;
    }

    private void OnEnable()
    {
        mouseMovementAction.Enable();
    }

    private void OnDisable()
    {
        mouseMovementAction.Disable();
    }

    private void Start()
    { 
        previousMousePosition = Mouse.current.position.ReadValue();
    }

    private void Update()
    {
        Vector2  mousePosition = mouseMovementAction.ReadValue<Vector2>();

       //Checking if mouse is outsite the window 
       if (mousePosition.x < 0 || mousePosition.x > Screen.width || mousePosition.y < 0 || mousePosition.y > Screen.height)
        {
            return; //stop mouse communication
        }

        //continue parralax if inside 
        if (!initialized)
        {
            previousMousePosition = mousePosition;
            initialized = true;
            return;
        }

        mouseDelta = mousePosition - previousMousePosition;

        for (int i = 0; i <parallaxLayers.Length; i++)
        {
            Vector3 newPosition = parallaxLayers[i].position;
            newPosition.x += mouseDelta.x * parallaxScales[i] * Time.deltaTime;
            newPosition.y += mouseDelta.y * parallaxScales[i] * Time.deltaTime;

            parallaxLayers[i].position = Vector2.Lerp(parallaxLayers[i].position, newPosition, smoothing * Time.deltaTime);
        }

        previousMousePosition = mousePosition;

        Debug.Log("MouseDelta: " + mouseDelta);
    }

    private void ResetParallax()
    {
        for (int i = 0; i < parallaxLayers.Length; i++)
        {
            parallaxLayers[i].position = Vector3.Lerp(parallaxLayers[i].position, new Vector3(0, 0, parallaxLayers[i].position.z), smoothing * Time.deltaTime);
        }
    }
}
