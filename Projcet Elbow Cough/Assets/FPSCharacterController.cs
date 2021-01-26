using System;
using UnityEngine;

public class FPSCharacterController : MonoBehaviour
{
    [Header("PlayerMovement")] public float MovementSpeed;
    public float DampSpeed;

    public float RotationSpeed;
    public float DampRotation;

    public LayerMask GroundedMask;
    public float GravityMultiplier;
    public float gravity;

    public float Jumpforce;
    public float MaxFallSpeed;
    public float MaxSpeed;
    public float Decelleration;

    [HideInInspector] public bool isGoingToJump = false;
    private float BackwardsSpeedMultiplier;
    private float AccelerationMultiplire;
    private float AccelerationMultiplierCap;
    private Vector3 gravityForce;
    public float IsGroundedDistance;

    private Vector2 wasdInput;
    private Vector3 forceDirection;
    private CharacterController playerController;
    private Vector3 force;
    private RaycastHit rayHit;
    private CapsuleCollider playerCollider;
    private CameraController cameraController;
    private CharacterController characterController;

    private Vector3 playerVelocity = Vector2.zero;
    private Vector2 rotationVelocity = Vector2.zero;

    private Vector2 currentDirection = Vector2.zero;


    Vector3 jumpVector = Vector3.up;

    //public float PitchSpeed;
    //public float MinPitchAngel;
    //public float MaxPitchAngel;

    private float yawAngel;

    public Transform cameraTransform;

    public float CameraDamp;
    private Vector3 cameraVelocity;


    private void LateUpdate()
    {
        // cameraTransform.position = transform.GetChild(0).position;
    }


    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
        playerCollider = GetComponent<CapsuleCollider>();
        cameraController = StaticRefrences.CameraTransform.GetComponent<CameraController>();
        characterController = GetComponent<CharacterController>();
        // gravity = -Physics.gravity.y;


        // cameraTransform = transform.GetChild(0).transform;
    }


    private void Update()
    {
        // Debug.Log(isGoingToJump + "yooo ");

        MovePlayer();
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0f);
        ;
        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, transform.GetChild(0).position,
            ref cameraVelocity, CameraDamp);
    }


    private void RotatePlayer2()
    {
        // Vector2 targetMouseDirection = InputManager.mouseDirection;
        // currentDirection = Vector2.SmoothDamp(currentDirection, targetMouseDirection, ref rotationVelocity, DampRotation);
        // transform.rotation *= Quaternion.Euler(0f, currentDirection.x * RotationSpeed * Time.fixedDeltaTime, 0f);

        yawAngel += InputManager.mouseDirection.x * RotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.AngleAxis(yawAngel, Vector3.up);
    }

    private void MovePlayer()
    {
        wasdInput = InputManager.WasdInput.normalized;

        // if (characterController.isGrounded && force.y < 0)
        // {
        //     force.y = 0f;
        // }

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

        playerController.Move(force); // add final force

        // if (InputManager.inputActions.Player.Jump. && characterController.isGrounded)
        // {
        //     force.y += Mathf.Sqrt(Jumpforce * -3.0f * -gravity);
        //     Debug.Log("jump-> fps");
        //
        // }
        if (!isGorunded())
            CalculateGravityForce(); //gravity


        // Jump();
        // Debug.Log(force + "   ," + forceDirection + " " + playerVelocity);
        //Debug.Log($"force: {force}, forceDirection {forceDirection}, velocity {playerBody.velocity.magnitude}");
    }

    public void Jump()
    {
        Debug.Log(isGoingToJump);
        Ray ray = new Ray(transform.position, -transform.up);

        Debug.Log(Physics.Raycast(ray, 2f));
        Debug.Log(transform.position);
        
        if (isGoingToJump)
        {
            jumpVector.y = 0;
            jumpVector.y += Jumpforce;
            // jumpVector.y += Mathf.Sqrt(Jumpforce * -3.0f * -gravity);
            Debug.Log(characterController.name);
            characterController.Move(jumpVector);
            Debug.Log(jumpVector);
            isGoingToJump = false;
        }
    }

    private void CalculateGravityForce()
    {
        if (!characterController.isGrounded)
        {
            Debug.Log((-gravity * (-1 + GravityMultiplier) * Time.deltaTime));
            // gravityForce += Vector3.up * (-gravity * (-1 + GravityMultiplier) * Time.deltaTime);
            jumpVector.y += (-gravity * (-1 + GravityMultiplier) * Time.deltaTime);
            characterController.Move(Vector3.ClampMagnitude(jumpVector, MaxFallSpeed));
            // force += gravityForce;
            Debug.Log("not grounded " + characterController.isGrounded);
        }
        else
        {
            force.y = 0f;
            // gravityForce = 0f:    
            // Debug.Log("grounded");
        }
    }


    private Vector3 GetNormal()
    {
        Physics.Raycast(transform.position, Vector3.down, out rayHit, 2f, GroundedMask, QueryTriggerInteraction.Ignore);
        return rayHit.normal;
    }

    private bool isGorunded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        if (playerController == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, playerController.velocity);

        Gizmos.DrawWireSphere(transform.position, IsGroundedDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, force);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, rayHit.normal * 2);


        //Gizmos.DrawLine(transform.position + playerBody.velocity.normalized ,transform.position + playerBody.velocity  );
    }
}