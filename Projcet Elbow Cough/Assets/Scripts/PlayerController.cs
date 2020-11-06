using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("PlayerMovement")]
    public float RotationSpeed;
    public float MovementSpeed;
    public float MaxSpeed;
    public float AccelerationMultiplire;
    public float AccelerationMultiplierCap;
    public float IsGroundedDistance;
    public LayerMask GroundedMask;
    public float gravityMultiplier;

    private Vector3 playerVelocity;
    private Vector2 wasdInput;
    private InputManager inputManager;
    private Vector3 forceDirection;
    private Rigidbody playerBody;
    private float pitchAngel;
    private float yawAngel;
    private Vector3 force;
    private RaycastHit rayHit;
    private CapsuleCollider playerCollider;
    private CameraController cameraController;
    private CharacterController characterController;
    private void Awake()
    {
        //playerBody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        cameraController = StaticRefrences.CameraParentTransform.GetComponent<CameraController>();
        characterController = GetComponent<CharacterController>();
    }

    //todo -> gravity refactoring;
    private void FixedUpdate()
    {
        RotatePlayer2();
        MovePlayer();
        characterController.Move((Vector3.up + Physics.gravity) * (gravityMultiplier + -1) * Time.fixedDeltaTime);
    }


    private void MovePlayer()
    {
        wasdInput = InputManager.WasdInput;
        if (wasdInput == Vector2.zero) return;

        forceDirection = ((wasdInput.x * transform.right) + (wasdInput.y * transform.forward)).normalized;
        if (characterController.velocity.magnitude <= MaxSpeed && characterController.isGrounded)
        {
            if (characterController.velocity.magnitude < AccelerationMultiplierCap)
            {
                force = forceDirection * MovementSpeed * AccelerationMultiplire * Time.fixedDeltaTime;
                characterController.Move(Vector3.ClampMagnitude(force, MaxSpeed));
                return;
            }
            force = forceDirection * MovementSpeed * Time.fixedDeltaTime;
            characterController.Move(Vector3.ClampMagnitude(force, MaxSpeed * 0.99f));
        }
    }
    private Vector3 getNormal()
    {
        Physics.Raycast(transform.position, Vector3.down, out rayHit, 2f, GroundedMask, QueryTriggerInteraction.Ignore);
        return rayHit.normal;
    }

    private void Gravity()
    {
        if (!characterController.isGrounded) { 
            characterController.SimpleMove((Vector3.up + Physics.gravity) * (gravityMultiplier + -1) * Time.fixedDeltaTime);
        }
  

        Debug.Log(characterController.isGrounded);
    }
    private void JumpGravity()
    {
        if (!IsGrounded())
        {
            Debug.Log("not grounded");
            playerBody.velocity += Vector3.up * Physics.gravity.y * (gravityMultiplier + -1) * Time.fixedDeltaTime;
        }
    }

    private void RotatePlayer2()
    {
        yawAngel += Mathf.Clamp(InputManager.mouseDirection.x, -1, 1);
        Vector3 rotation = new Vector3(0f, (yawAngel) * RotationSpeed, 0f);
        //playerBody.MoveRotation(Quaternion.Euler(rotation));
        transform.rotation =(Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), Time.fixedDeltaTime * 10));
    }

    private void RotatePlayer()
    {
        yawAngel += Mathf.Clamp(InputManager.mouseDirection.x, -1, 1);
        Vector3 rotation = new Vector3(0f, (yawAngel) * RotationSpeed, 0f);
        //playerBody.MoveRotation(Quaternion.Euler(rotation));
        playerBody.MoveRotation(Quaternion.Slerp(transform.rotation,Quaternion.Euler(rotation), Time.fixedDeltaTime * 10));
    }


    private bool IsGrounded()
    {
        return Physics.SphereCast(transform.position + Vector3.up, playerCollider.radius - 0.1f, Vector3.down,
            out rayHit, IsGroundedDistance, GroundedMask, QueryTriggerInteraction.Ignore);
    }

    private void OnDrawGizmosSelected()
    {
        if (playerBody == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, playerBody.velocity);

        Gizmos.DrawWireSphere(transform.position, IsGroundedDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, force);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, rayHit.normal * 2);


        //Gizmos.DrawLine(transform.position + playerBody.velocity.normalized ,transform.position + playerBody.velocity  );
    }


}
