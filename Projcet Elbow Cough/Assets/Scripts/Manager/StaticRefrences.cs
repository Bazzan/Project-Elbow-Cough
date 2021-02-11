using Mirror;
using UnityEngine;


[DefaultExecutionOrder(-100)]
public class StaticRefrences : NetworkBehaviour
{
    public Transform Player;
    public Camera Camera;

    public Transform PlayerTransform;
    public Transform CameraTransform;
    public Transform CameraParentTransform;
     // public SyncList< GameObject> ListOfPlayers = new  SyncList<GameObject>();
}