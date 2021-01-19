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
    public float MaxSpeed;
    public float Decelleration;

    [HideInInspector] public bool isGoingToJump;
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
        cameraController = StaticRefrences.CameraParentTransform.GetComponent<CameraController>();
        characterController = GetComponent<CharacterController>();
        // gravity = -Physics.gravity.y;


        // cameraTransform = transform.GetChild(0).transform;
    }


    private void Update()
    {
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


        forceDirection =
            ((wasdInput.x * transform.right) + (wasdInput.y * transform.forward)); //getting input direction


        if (forceDirection == Vector3.zero)
        {
            force *= Time.deltaTime * Decelleration;
        }
        else
        {
            Vector3 inputForce = (forceDirection * (MovementSpeed * Time.deltaTime));

            force = Vector3.ClampMagnitude(inputForce, MaxSpeed) + force.y * Vector3.up;
        }

        if (isGoingToJump)
            force += Vector3.up * Jumpforce;
            CalculateGravityForce(); //gravity
        // Jump(); // if is going to jump adding force

        playerController.Move(force); // add final force


        // Debug.Log(force + "   ," + forceDirection + " " + playerVelocity);
        //Debug.Log($"force: {force}, forceDirection {forceDirection}, velocity {playerBody.velocity.magnitude}");
    }

    public void OnJump()
    {
        isGoingToJump = true;
        // // if (isGoingToJump)
        // // {
        //     Debug.Log("jump?");
        //     force += new Vector3(0f, Jumpforce, 0f);
        //     // isGoingToJump = false;
        // // }
    }

    private void CalculateGravityForce()
    {
        if (!characterController.isGrounded)
        {
            gravityForce += Vector3.up * (-gravity * (-1 + GravityMultiplier) * Time.deltaTime);
            force += gravityForce;
            // Debug.Log("not grounded");
        }
        else
        {
            gravityForce = Vector3.zero;
            // Debug.Log("grounded");
        }
    }


    private Vector3 GetNormal()
    {
        Physics.Raycast(transform.position, Vector3.down, out rayHit, 2f, GroundedMask, QueryTriggerInteraction.Ignore);
        return rayHit.normal;
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