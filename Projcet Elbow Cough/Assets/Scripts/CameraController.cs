using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 CameraOffset;
    public float SmoothValue;
    public float CameraSpeed;
    public float MaxPitchAngel;
    public float MinPitchAngel;
    public float maxDegreesToRotate;
    private Vector3 cameraTarget;
    private Transform playerTransform;
    private Vector3 cameraVelocity;

    private Transform camera;
    private float pitchAngel;
    private void Awake()
    {
        playerTransform = StaticRefrences.PlayerTransform;
        camera = transform.GetChild(0).transform;
        camera.localPosition = CameraOffset;
        transform.position = playerTransform.position;
    }


    private void FixedUpdate()
    {
        MoveCamera();
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.Euler(transform.rotation.x, playerTransform.rotation.eulerAngles.y, transform.rotation.z),
            Time.fixedDeltaTime * 10f);

        //transform.rotation = Quaternion.RotateTowards(transform.rotation ,
        //    Quaternion.Euler(transform.rotation.x, playerTransform.rotation.eulerAngles.y, transform.rotation.z),
        //    maxDegreesToRotate);
        PitchRotation();



    }

    private void MoveCamera()
    {
        cameraTarget = playerTransform.position;
        transform.position = Vector3.SmoothDamp(transform.position, cameraTarget, ref cameraVelocity, SmoothValue, CameraSpeed, Time.fixedDeltaTime);
    }

    private void PitchRotation()
    {
        pitchAngel -= InputManager.mouseDirection.y;
        pitchAngel = Mathf.Clamp(pitchAngel, MinPitchAngel, MaxPitchAngel);

        transform.rotation = Quaternion.Euler(pitchAngel, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    public void RotateCameraWithPlayer(float yRotation)
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, yRotation * Mathf.Rad2Deg, transform.rotation.z);

    }
}
