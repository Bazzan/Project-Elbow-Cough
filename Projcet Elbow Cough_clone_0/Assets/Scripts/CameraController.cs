using System;
using Mirror;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    public Transform CameraTarget;
    public float MaxPitchAngel;
    public float MinPitchAngel;
    public float PitchSpeed;
    public float YawSpeed;
    public float lerpTime;

    private Transform cameraTransform;

    private float pitchAngel;
    private float rotX;
    private float rotY;


    private void Start()
    {
        if (!base.hasAuthority) return;
        cameraTransform = Camera.main.transform;
        cameraTransform.SetParent(transform);
    }

    private void LateUpdate()
    {
        if (!base.hasAuthority) return;

        rotX = InputManager.mouseDirection.x * YawSpeed * Time.deltaTime;
        rotY += InputManager.mouseDirection.y * PitchSpeed * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, MinPitchAngel, MaxPitchAngel);
        Quaternion TargetRotation = Quaternion.Euler(-rotY, rotX, 0f);
        cameraTransform.localRotation = Quaternion.Lerp(cameraTransform.localRotation, TargetRotation, lerpTime);
        cameraTransform.position = CameraTarget.position;
    }

}