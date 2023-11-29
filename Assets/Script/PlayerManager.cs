using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerLocomotion playerLocomotion;
    GameManager gameManager;

    public bool isInteracting;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (gameManager.isGameActive)
        {
            inputManager.HandleALlInput();
            cameraManager.HandleAllCameraMovement();
        }      
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

}
