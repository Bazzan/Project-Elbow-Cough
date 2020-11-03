using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float MovementSpeed;
    private Vector2 wasdInput;
    private InputManager inputManager;
    private Vector3 forceDirection;
    private Rigidbody playerBody;
    

    private void Awake()
    {
        playerBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        wasdInput = InputManager.WasdInput;
        forceDirection = new Vector3(wasdInput.x, 0, wasdInput.y);
        playerBody.AddForce(forceDirection * MovementSpeed);
       
    }
}
