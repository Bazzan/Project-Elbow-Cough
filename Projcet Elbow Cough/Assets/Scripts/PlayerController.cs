using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("PlayerMovement")]
    public float RotationSpeed;
    public float DampRotation;
    public float MovementSpeed;
    public float DampSpeed;
    public float MaxSpeed;
    public float BackwardsSpeedMultiplier;
    public float AccelerationMultiplire;
    public float AccelerationMultiplierCap;
    public float Jumpforce;
    public float IsGroundedDistance;
    public LayerMask GroundedMask;
    public float GravityMultiplier;

    public Vector2 wasdInput;
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
    private Vector3 previousVelocity;

    private Vector3 playerVelocity = Vector2.zero;
    private Vector2 rotationVelocity = Vector2.zero;
    private Vector2 currentDirection = Vector2.zero;

    private void Awake()
    {
        playerBody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        cameraController = StaticRefrences.CameraParentTransform.GetComponent<CameraController>();
        characterController = GetComponent<CharacterController>();

    }

    //todo -> gravity refactoring;
    private void FixedUpdate()
    {

        MovePlayer();

        Gravity();
        //if (!characterController.isGrounded)
        //    characterController.SimpleMove((Physics.gravity) * (GravityMultiplier + -1) * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        RotatePlayer2();

    }
    public void Jump()
    {
        if (!IsGrounded()) return;
        playerBody.velocity += Vector3.up * (Jumpforce + Mathf.Abs(playerBody.velocity.y));
    }

    private void Gravity()
    {
        if (!IsGrounded())
        {
            playerBody.velocity += Vector3.up * Physics.gravity.y * (-1 + GravityMultiplier) * Time.fixedDeltaTime;

        }
    }



    private void MovePlayer()
    {
        wasdInput = InputManager.WasdInput;
        //Debug.Log(IsGrounded());
        //if (wasdInput == Vector2.zero) return;
        forceDirection = ((wasdInput.x * transform.right) + (wasdInput.y * transform.forward)).normalized;
        //forceDirection = Vector3.ProjectOnPlane(forceDirection, GetNormal());
        force = forceDirection * MovementSpeed  /*Time.deltaTime*/;
        Debug.Log(force + "   ," + forceDirection + " " + MovementSpeed);
        playerBody.velocity = Vector3.SmoothDamp(playerBody.velocity, force,ref playerVelocity, DampSpeed);
        //Debug.Log($"force: {force}, forceDirection {forceDirection}, velocity {playerBody.velocity.magnitude}");
    }



    //private void MovePlayer()
    //{
    //    wasdInput = InputManager.WasdInput;
    //    Debug.Log(IsGrounded());
    //    if (wasdInput == Vector2.zero) return;
    //    forceDirection = ((wasdInput.x * transform.right) + (wasdInput.y * transform.forward)).normalized;
    //    forceDirection = Vector3.ProjectOnPlane(forceDirection, GetNormal());
    //    force = forceDirection * MovementSpeed * Time.fixedDeltaTime;
    //    if (wasdInput.y < 0 && playerBody.velocity.magnitude < (MaxSpeed * BackwardsSpeedMultiplier))
    //    {
    //        playerBody.AddForce(force * BackwardsSpeedMultiplier, ForceMode.VelocityChange);
    //        //Debug.Log($"force: {force}, forceDirection {forceDirection}, velocity {playerBody.velocity.magnitude}");
    //        return;
    //    }
    //    if (playerBody.velocity.magnitude <= AccelerationMultiplierCap) { 
    //        playerBody.AddForce(force *  AccelerationMultiplire, ForceMode.VelocityChange);
    //        return;
    //    }
    //    if (playerBody.velocity.magnitude < MaxSpeed)
    //        playerBody.AddForce(force, ForceMode.VelocityChange);
    //    //Debug.Log($"force: {force}, forceDirection {forceDirection}, velocity {playerBody.velocity.magnitude}");
    //}



    private Vector3 GetNormal()
    {
        Physics.Raycast(transform.position, Vector3.down, out rayHit, 2f, GroundedMask, QueryTriggerInteraction.Ignore);
        return rayHit.normal;
    }

    //private void Gravity()
    //{
    //    if (!characterController.isGrounded)
    //    {
    //        characterController.SimpleMove((Vector3.up + Physics.gravity) * (GravityMultiplier + -1) * Time.fixedDeltaTime);
    //    }


    //    Debug.Log(characterController.isGrounded);
    //}
    private void JumpGravity()
    {
        if (!IsGrounded())
        {
            Debug.Log("not grounded");
            playerBody.velocity += Vector3.up * Physics.gravity.y * (GravityMultiplier + -1) * Time.fixedDeltaTime;
        }
    }

    private void RotatePlayer2()
    {

        
        Vector2 targetMouseDirection = new Vector2(Mathf.Clamp(InputManager.mouseDirection.x, -1, 1), 0f);
        currentDirection = Vector2.SmoothDamp(currentDirection, targetMouseDirection , ref rotationVelocity, DampRotation);

        Debug.Log(currentDirection);
        transform.rotation *= Quaternion.AngleAxis(currentDirection.x * RotationSpeed, Vector3.up);

        //yawAngel += Mathf.Clamp(InputManager.mouseDirection.x, -1, 1);
        
        //Vector3 rotation = new Vector3(0f, (yawAngel) * RotationSpeed, 0f);
        
        
        //rotation = Vector3.SmoothDamp(transform.rotation.eulerAngles, rotation, ref rotationVelocity, DampRotation);

        //playerBody.MoveRotation(Quaternion.Euler(rotation));
        //transform.rotation = Quaternion.Euler(rotation);

        //transform.rotation = (Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), Time.fixedDeltaTime));
    }

    private void RotatePlayer()
    {
        yawAngel += Mathf.Clamp(InputManager.mouseDirection.x, -1, 1);
        Vector3 rotation = new Vector3(0f, (yawAngel) * RotationSpeed, 0f);
        //playerBody.MoveRotation(Quaternion.Euler(rotation));
        playerBody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), Time.fixedDeltaTime * 10));
    }


    private bool IsGrounded()
    {
        return Physics.SphereCast(transform.position + transform.up, playerCollider.radius - 0.1f, Vector3.down,
            out rayHit, playerCollider.height * 0.5f + IsGroundedDistance, GroundedMask, QueryTriggerInteraction.Ignore);
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
