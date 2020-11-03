using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static PlayerInputAction inputActions;


    public static Vector2 mouseDirection;
    public static Vector2 WasdInput;

    private void Awake()
    {
        inputActions = new PlayerInputAction();
    }

    private void OnEnable()
    {
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





}
