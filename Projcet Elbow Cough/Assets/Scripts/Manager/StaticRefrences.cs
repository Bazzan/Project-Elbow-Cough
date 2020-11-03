using UnityEngine;


[DefaultExecutionOrder(-100)]
public class StaticRefrences : MonoBehaviour
{
    public Transform Player;
    public Camera Camera;
    

    public static Transform PlayerTransform;
    public static Transform CameraTransform;


    private void Awake()
    {
        PlayerTransform = Player.transform;
        CameraTransform = Camera.transform;
    }
}
