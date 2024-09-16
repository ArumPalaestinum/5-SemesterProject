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

    private Vector3 previousMousePosition;
    private Vector2 mouseDelta;
    private InputAction mouseMovementAction;

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
        Vector2  mousePosition = Mouse.current.position.ReadValue<Vector2>();
        mouseDelta = mousePosition - previousMousePosition;

        for (int i = 0; i <parallaxLayers.Length; i++)
        {
            Vector3 newPosition = parallaxLayers[i].position;
            newPosition.x += mouseDelta.x * parallaxScales[i] * Time.deltaTime;
            newPosition.y += mouseDelta.y * parallaxScales[i] * Time.deltaTime;

            parallaxLayers[i].position = Vector3.Lerp(parallaxLayers[i].position, newPosition, smoothing * Time.deltaTime);
        }

        previousMousePosition = mousePosition;
    }
}
