using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    [SerializeField] private Inventory inventory = null; //testing inventory and item generation

    public static PlayerInputAction inputActions;


    public static Vector2 mouseDirection;
    public static Vector2 WasdInput;


    private FPSCharacterController playerController;


    private void Awake()
    {
        inputActions = new PlayerInputAction();
        playerController = StaticRefrences.PlayerTransform.GetComponent<FPSCharacterController>();

    }

    private void OnEnable()
    {
        //inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Fire.performed += OnAttack;
        inputActions.Player.OpenInventory.performed += OnOpenInventory;
        inputActions.Player.TestInventory.performed += OnTestInventory;
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        WasdInput = inputActions.Player.Move.ReadValue<Vector2>();
        mouseDirection = inputActions.Player.Look.ReadValue<Vector2>();

    }


    //public void OnJump(InputAction.CallbackContext callbackContext)
    //{
    //    playerController.OnJump();
    //}
    public void OnAttack(InputAction.CallbackContext callbackContext)
    {

    }


    public void OnOpenInventory(InputAction.CallbackContext callbackContext)
    {
        inventory.OpenInventory(); //testing inventory and item generation
    }

    public void OnTestInventory(InputAction.CallbackContext callbackContext)
    {
        inventory.GenerateItem(); //testing inventory and item generation
    }

}
