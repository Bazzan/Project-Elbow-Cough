using UnityEngine;


[DefaultExecutionOrder(-100)]
public class StaticRefrences : MonoBehaviour
{
    public Transform Player;
    public Camera Camera;

    public static Transform PlayerTransform;
    public static Transform CameraTransform;
    public static Transform CameraParentTransform;

    public static GameObject[] ListOfPlayers;

}
