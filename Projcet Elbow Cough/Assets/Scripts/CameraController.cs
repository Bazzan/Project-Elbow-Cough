using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 CameraOffset;
    public float SmoothValue;
    public float CameraSpeed;
    public float MaxPitchAngel;
    public float MinPitchAngel;
    public float maxDegreesToRotate;
    public float PitchSpeed;
    public float RotationSpeed;


    [Header("Collision values")]
    public float CameraCollider;
    public float CameraCollisionDistance;
    public LayerMask CameraCollisionMask;




    private Transform playerTransform;
    private float pitchAngel;
    private Rigidbody playerBody;
    private float yawAngel;


    Vector2 currentDirection;
    Vector2 rotationVelocity;
    float DampRotation = 0.03f;
    private void Awake()
    {
        playerTransform = transform.parent.transform;
        playerBody = playerTransform.GetComponent<Rigidbody>();
    }
    private void LateUpdate()
    {
        PitchRotation();
    }
    public void PitchRotation()
    {
        //Vector2 targetMouseDirection = new Vector2(0f, Mathf.Clamp(InputManager.mouseDirection.y, -1, 1));
        //currentDirection = Vector2.SmoothDamp(currentDirection, targetMouseDirection, ref rotationVelocity, DampRotation);

        //Debug.Log(currentDirection);

        //transform.localRotation *= Quaternion.AngleAxis(currentDirection.y * RotationSpeed, Vector3.left);


        pitchAngel -= Mathf.Clamp( InputManager.mouseDirection.y, -1,1) * PitchSpeed;
        pitchAngel = Mathf.Clamp(pitchAngel, MinPitchAngel, MaxPitchAngel);
        transform.localRotation = Quaternion.Euler(pitchAngel, 0f, 0f);
    }
    //public void RotatePlayer()
    //{


    //    yawAngel = Mathf.Clamp(InputManager.mouseDirection.x, -1, 1);
    //    Vector3 rotation = new Vector3(0f, (yawAngel * RotationSpeed), 0f);
    //    Debug.Log(rotation.y +"   " + yawAngel );
    //    playerBody.MoveRotation(Quaternion.Euler(rotation + playerTransform.rotation.eulerAngles));
    //    //playerTransform.rotation =   Quaternion.Euler(rotation + playerTransform.rotation.eulerAngles );
    //    //playerBody.rotation =  yawAngel *  
    //    //playerBody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), Time.fixedDeltaTime));
    //}
    //public void RotateCameraWithPlayer()
    //{
    //    transform.rotation = Quaternion.Euler(transform.rotation.x, playerTransform.rotation.eulerAngles.y, 0f);
    //}
}
