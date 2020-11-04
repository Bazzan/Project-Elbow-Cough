using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float RotationSpeed;
    public float MovementSpeed;
    public float MaxSpeed;
    public float IsGroundedDistance;
    public LayerMask GroundedMask;
    public float gravityMultip;
    private Vector2 wasdInput;
    private InputManager inputManager;
    private Vector3 forceDirection;
    private Rigidbody playerBody;
    private float pitchAngel;
    private float yawAngel;
    private Vector3 force;
    private RaycastHit rayHit;
    private CapsuleCollider playerCollider;

    private void Awake()
    {
        playerBody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        MovePlayer2();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        wasdInput = InputManager.WasdInput;

        forceDirection = ((wasdInput.x * transform.right) + (wasdInput.y * transform.forward)).normalized;

        //Debug.Log("grounded");

        forceDirection = Vector3.ProjectOnPlane(forceDirection, rayHit.normal);
        force = forceDirection * MovementSpeed * Time.fixedDeltaTime;
        //Debug.Log($"force: {force}, forceDirection {forceDirection}, velocity {playerBody.velocity.magnitude}");

        if (!IsGrounded())
        {
            //if (playerBody.velocity.y < 0f)
            playerBody.velocity += Vector3.up * Physics.gravity.y * ( - 1) * Time.fixedDeltaTime;
        }

        if (playerBody.velocity.magnitude < MaxSpeed)
            playerBody.AddForce(force, ForceMode.Impulse);

    }


    private void MovePlayer2()
    {

        wasdInput = InputManager.WasdInput;
        forceDirection = ((wasdInput.x * transform.right) + (wasdInput.y * transform.forward)).normalized;

        forceDirection = Vector3.ProjectOnPlane(forceDirection, rayHit.normal);
        force = forceDirection * MovementSpeed * Time.fixedDeltaTime;

        if (!IsGrounded())
        {
            Debug.Log("grounded");
            playerBody.velocity += Vector3.up * Physics.gravity.y * (gravityMultip +-1) * Time.fixedDeltaTime;

        }
        //playerBody.MovePosition(transform.position + force);

        if (wasdInput == Vector2.zero) return;
        if(playerBody.velocity.magnitude < 10)
            playerBody.velocity += force;


    }

    private void RotatePlayer()
    {
        yawAngel += Mathf.Clamp(InputManager.mouseDirection.x, -1, 1);
        Vector3 rotation = new Vector3(0f, (yawAngel) * RotationSpeed, 0f);
        playerBody.MoveRotation(Quaternion.Euler(rotation));
    }


    private bool IsGrounded()
    {
        return Physics.SphereCast(transform.position + Vector3.up, playerCollider.radius, Vector3.down,
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
