using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    public Transform CameraTarget;
    public Vector3 CameraOffset;
    public float MaxPitchAngel;
    public float MinPitchAngel;
    public float PitchSpeed;
    public float YawSpeed;
    public float slerpTime;
    private float pitchAngel;
    private float rotX;
    private float rotY;
 
    // private void FixedUpdate()
    // {
    //     transform.position = CameraTarget.position;
    // }

    private void LateUpdate()
    {
        rotX += InputManager.mouseDirection.x * YawSpeed * Time.deltaTime;
        rotY += InputManager.mouseDirection.y * PitchSpeed * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, MinPitchAngel, MaxPitchAngel);
        Quaternion TargetRotation = Quaternion.Euler(-rotY,rotX,0f);
        
        // transform.localRotation = Quaternion.Euler(-rotY, rotX, 0f);
        transform.localRotation = Quaternion.Slerp(transform.localRotation,TargetRotation, slerpTime);
        transform.position = CameraTarget.position;
        // PitchCamera();
    }
    private void PitchCamera()
    {
        pitchAngel -= InputManager.mouseDirection.y * PitchSpeed * Time.deltaTime;
        pitchAngel = Mathf.Clamp(pitchAngel, MinPitchAngel, MaxPitchAngel);
        transform.localRotation = Quaternion.AngleAxis(pitchAngel, Vector3.right);
    }
    

    

}
