using Mirror;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    public Transform Player;
    public Transform CameraTarget;
    public Vector3 CameraOffset;
    public float MaxPitchAngel;
    public float MinPitchAngel;
    public float PitchSpeed;
    public float YawSpeed;
    public float slerpTime;
    public float lerpTime;

    private Transform cameraTransform;
    
    private float pitchAngel;
    private float rotX;
    private float rotY;
    
    public override void OnStartLocalPlayer()
    {
        cameraTransform = Camera.main.transform;
        cameraTransform.SetParent(transform);
        
    }

    // private void Awake()
    // {
    //     if (!isLocalPlayer) return;
    //     cameraTransform = Camera.main.transform;
    //     cameraTransform.SetParent(transform);
    //     
    // }

    private void LateUpdate()
    {
        if (!isLocalPlayer) return;        
        
        // Debug.Log(InputManager.mouseDirection.x);
        rotX = InputManager.mouseDirection.x * YawSpeed * Time.deltaTime;
        rotY += InputManager.mouseDirection.y * PitchSpeed * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, MinPitchAngel, MaxPitchAngel);
        Quaternion TargetRotation = Quaternion.Euler(-rotY,rotX,0f);
        
        // transform.localRotation = Quaternion.Euler(-rotY, rotX, 0f);
        // transform.localRotation = Quaternion.Slerp(transform.localRotation,TargetRotation, slerpTime);
            cameraTransform.localRotation = Quaternion.Lerp(cameraTransform.localRotation,TargetRotation, lerpTime);
        cameraTransform.position = CameraTarget.position;
        // PitchCamera();
    }
    private void PitchCamera()
    {
        pitchAngel -= InputManager.mouseDirection.y * PitchSpeed * Time.deltaTime;
        pitchAngel = Mathf.Clamp(pitchAngel, MinPitchAngel, MaxPitchAngel);
        transform.localRotation = Quaternion.AngleAxis(pitchAngel, Vector3.right);
    }
    

    

}
