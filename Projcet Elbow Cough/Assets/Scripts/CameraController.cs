using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 CameraOffset;
    public float MaxPitchAngel;
    public float MinPitchAngel;
    public float PitchSpeed;

    private float pitchAngel;


    private void LateUpdate()
    {
        PitchCamera();
    }
    private void PitchCamera()
    {
        pitchAngel += InputManager.mouseDirection.y * PitchSpeed * Time.deltaTime;
        pitchAngel = Mathf.Clamp(pitchAngel, MinPitchAngel, MaxPitchAngel);
        transform.localRotation = Quaternion.AngleAxis(pitchAngel, Vector3.left);
    }

}
