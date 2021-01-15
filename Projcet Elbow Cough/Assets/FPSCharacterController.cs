using System;
using UnityEngine;

public class FPSCharacterController : MonoBehaviour
{
    [Header("PlayerMovement")]
    public float MovementSpeed;
    public float DampSpeed;
    
    public float RotationSpeed;
    public float DampRotation;

    public LayerMask GroundedMask;
    
    
    private float MaxSpeed;
    private float BackwardsSpeedMultiplier;
    private float AccelerationMultiplire;
    private float AccelerationMultiplierCap;
    private float Jumpforce;
    private float GravityMultiplier;

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
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y,0f);;
        cameraTransform.position = Vector3.SmoothDamp( cameraTransform.position ,transform.GetChild(0).position, ref cameraVelocity, CameraDamp) ;
        
        
    }

    
    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
        playerCollider = GetComponent<CapsuleCollider>();
        cameraController = StaticRefrences.CameraParentTransform.GetComponent<CameraController>();
        characterController = GetComponent<CharacterController>();
        // cameraTransform = transform.GetChild(0).transform;
    }



    private void Update()
    {
        MovePlayer();

    }

    private void FixedUpdate()
    {

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
        //Debug.Log(IsGrounded());
        //if (wasdInput == Vector2.zero) return;
        forceDirection = ((wasdInput.x * transform.right) + (wasdInput.y * transform.forward));
        // forceDirection = Vector3.ProjectOnPlane(forceDirection, GetNormal() );
        force = forceDirection * (MovementSpeed * Time.deltaTime);
        // Debug.Log(Vector3.SmoothDamp(transform.position, force , ref playerVelocity,DampSpeed,10,Time.fixedDeltaTime));
        
        // playerController.Move(Vector3.SmoothDamp(transform.position, force , ref playerVelocity,DampSpeed,10,Time.fixedDeltaTime) );
        playerController.Move(force);



        // cameraTransform.position = Vector3.SmoothDamp( cameraTransform.position ,transform.GetChild(0).position, ref cameraVelocity, Time.deltaTime) ;




        Debug.Log(force + "   ," + forceDirection + " " + playerVelocity);
        //Debug.Log($"force: {force}, forceDirection {forceDirection}, velocity {playerBody.velocity.magnitude}");
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




    //private void PitchCamera()
    //{
    //    pitchAngel += InputManager.mouseDirection.y * PitchSpeed * Time.deltaTime;
    //    pitchAngel = Mathf.Clamp(pitchAngel, MinPitchAngel, MaxPitchAngel);
    //    cameraTransform.localRotation = Quaternion.AngleAxis(pitchAngel, Vector3.left);
    //}
}
