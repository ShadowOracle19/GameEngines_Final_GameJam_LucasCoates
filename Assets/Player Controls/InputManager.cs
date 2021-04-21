using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance
    {
        get { return _instance; }
    }

    private PlayerController playerController;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
            

        playerController = new PlayerController();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        playerController.Enable();
    }

    private void OnDisable()
    {
        playerController.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerController.Player.Movement.ReadValue<Vector2>();
    }
    public Vector2 GetMouseDelta()
    {
        return playerController.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumpedThisFrame()
    {
        return playerController.Player.Jump.triggered;
    }

    public bool PlayerPressedPickup()
    {
        return playerController.Player.Pickup.triggered;
    }

    public bool PlayerPressedPause()
    {
        return playerController.Player.Pause.triggered;
    }

}
