using System;
using Mirror;
using UnityEngine;

public class FPSCharacterController : NetworkBehaviour
{
    [Header("PlayerMovement")] 
    
    [SerializeField] private float MovementSpeed;
    [SerializeField] private LayerMask GroundedMask;
    [SerializeField] private float GravityMultiplier;
    [SerializeField] private float gravity;
    [SerializeField] private float Jumpforce;
    [SerializeField] private float MaxSpeed;
    [SerializeField] private float Decelleration;
    [SerializeField] private float IsGroundedDistance;

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
    private int playerIndex;


    private void Start()
    {
        if (!base.hasAuthority) return;
        characterController = GetComponent<CharacterController>();
        StaticRefrences.CameraTransform = Camera.main.transform;
        StaticRefrences.PlayerTransform = transform;

        cameraTransform = Camera.main.transform;
        InputManager.playerController = this;
    }
    private void Update()
    {
        if (!base.hasAuthority) return;
        MovePlayer();
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0f);
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
        
        characterController.Move(force); // add move force
        
        if (isGrounded && jumpVector.y < -0.5f) 
            jumpVector.y = -0.5f;
        CalculateGravityForce(); //gravity
        characterController.Move(jumpVector * Time.deltaTime); // add gravity and jumpforce

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
            jumpVector.y += -gravity * GravityMultiplier * Time.deltaTime;
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

    #endregion
}