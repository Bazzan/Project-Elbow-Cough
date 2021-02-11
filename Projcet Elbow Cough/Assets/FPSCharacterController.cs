using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCharacterController : NetworkBehaviour
{
    [Header("PlayerMovement")] public float MovementSpeed;

    public float RotationSpeed;

    public LayerMask GroundedMask;
    public float GravityMultiplier;
    public float gravity;

    public float Jumpforce;

    // public float MaxFallSpeed;
    public float MaxSpeed;
    public float Decelleration;
    public float IsGroundedDistance;
    public float CameraDamp;

    private StaticRefrences staticRefrences;
    private Vector2 wasdInput;
    private Vector3 forceDirection;
    private Vector3 force;
    private RaycastHit rayHit;
    private CharacterController characterController;
    Vector3 jumpVector = Vector3.up;
    private float yawAngel;
    private Transform cameraTransform;
    private Vector3 cameraVelocity;
    private bool isGrounded;
    private int playerIndex= 0;

    public override void OnStartLocalPlayer()
    {
        staticRefrences = FindObjectOfType<StaticRefrences>();
        
        InputManager.playerController = this;

        characterController = GetComponent<CharacterController>();
        staticRefrences.CameraTransform = Camera.main.transform;
        staticRefrences.PlayerTransform = transform;
        cameraTransform = Camera.main.transform;
        
        SetPlayerIndex();
    }

    [Command]
    public void SetPlayerIndex()
    {
        // for (int i = 0; i < staticRefrences.ListOfPlayers.Count; i++)
        // {
        //     if (staticRefrences.ListOfPlayers[i] == null)
        //     {
        //         playerIndex = i;
        //         break;
        //     }
        // }

        // staticRefrences.ListOfPlayers.Add(gameObject);
        // playerIndex = staticRefrences.numberOfPlayers;

        
        // Debug.Log("player index: " + playerIndex + "number of players in game: " +
        //           staticRefrences.ListOfPlayers.Count);
    }
    // private void Awake()
    // {
    //     if (!isLocalPlayer) return;
    //
    //     characterController = GetComponent<CharacterController>();
    //     // cameraTransform = StaticRefrences.CameraTransform;
    //     // cameraTransform = transform.root.GetComponentInChildren<Camera>().transform;
    //     cameraTransform = Camera.main.transform;
    // }

    private void Update()
    {

        // Debug.LogError("player index: " + playerIndex + "number of players " + staticRefrences.numberOfPlayers);
        if (!isLocalPlayer) return;
        
        MovePlayer();
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0f);
        ;
        // cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, transform.GetChild(0).position,
        //     ref cameraVelocity, CameraDamp);
    }

    private void MovePlayer()
    {
        wasdInput = InputManager.WasdInput.normalized;

        isGrounded = characterController.isGrounded;

        forceDirection =
            ((wasdInput.x * transform.right) + (wasdInput.y * transform.forward)); //getting input direction


        if (forceDirection == Vector3.zero)
        {
            force.x *= Time.deltaTime * Decelleration;
            force.z *= Time.deltaTime * Decelleration;
        }
        else
        {
            Vector3 inputForce = (forceDirection * (MovementSpeed * Time.deltaTime));

            force = Vector3.ClampMagnitude(inputForce, MaxSpeed);
        }


        characterController.Move(force); // add final force

        if (isGrounded && jumpVector.y < -0.5f)
            jumpVector.y = -0.5f;


        CalculateGravityForce(); //gravity
        characterController.Move(jumpVector * Time.deltaTime);

        // Debug.Log(force + "   ," + forceDirection + " " + playerVelocity);
        //Debug.Log($"force: {force}, forceDirection {forceDirection}, velocity {playerBody.velocity.magnitude}");
    }

    public void Jump()
    {
        if (!isLocalPlayer) return;
        if (isGrounded)
        {
            jumpVector.y = 0f;
            jumpVector.y += (Mathf.Sqrt(Jumpforce * -3.0f * -gravity));
        }
    }

    private void CalculateGravityForce()
    {
        if (!isGrounded)
        {
            jumpVector.y += -gravity * GravityMultiplier * Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (characterController == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, characterController.velocity);

        Gizmos.DrawWireSphere(transform.position, IsGroundedDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, force);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, rayHit.normal * 2);


        //Gizmos.DrawLine(transform.position + playerBody.velocity.normalized ,transform.position + playerBody.velocity  );
    }


    #region not used stuff

    private bool isGorunded()
    {
        if (Physics.Raycast(transform.position, -transform.up, out rayHit, 2f))
        {
            // Debug.Log(rayHit.collider.gameObject.name);
            return true;
        }
        else
        {
            return false;
        }
    }


    private Vector3 GetNormal()
    {
        Physics.Raycast(transform.position, Vector3.down, out rayHit, 2f, GroundedMask, QueryTriggerInteraction.Ignore);
        return rayHit.normal;
    }

    private void RotatePlayer2()
    {
        // Vector2 targetMouseDirection = InputManager.mouseDirection;
        // currentDirection = Vector2.SmoothDamp(currentDirection, targetMouseDirection, ref rotationVelocity, DampRotation);
        // transform.rotation *= Quaternion.Euler(0f, currentDirection.x * RotationSpeed * Time.fixedDeltaTime, 0f);

        yawAngel += InputManager.mouseDirection.x * RotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.AngleAxis(yawAngel, Vector3.up);
    }

    #endregion
}