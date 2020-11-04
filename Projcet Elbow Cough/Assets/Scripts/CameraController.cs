using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 CameraOffset;
    public float SmoothValue;
    public float CameraSpeed;

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

        cameraTarget = playerTransform.position;
        transform.position = Vector3.SmoothDamp(transform.position, cameraTarget, ref cameraVelocity, SmoothValue, CameraSpeed);

        pitchAngel -=  InputManager.mouseDirection.y;
        transform.localRotation = Quaternion.Euler(pitchAngel, 0f , 0f);

    }


}
