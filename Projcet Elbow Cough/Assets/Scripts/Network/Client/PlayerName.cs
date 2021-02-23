using Mirror;
using TMPro;
using UnityEngine;

public class PlayerName : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    [SyncVar(hook = nameof(OnNameUpdate))] private string syncronizedName;
    private void Awake()
    {
        Debug.Log(base.hasAuthority);
         Debug.Log(NetworkClient.connection.identity + " " +  netIdentity + " " + netId);
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        // syncronizedName = ClientInstance.ReturnClientInstance().playerPrefab.gameObject.name;
    }
    /// <summary>
    ///syncvar hook for syncronizedName
    /// </summary>
    private void OnNameUpdate(string previous, string next)
    {
        text.text = next;
    }

    /// <summary>
    /// setts playerName for owner
    /// this method can only be called from a client
    /// </summary>
    /// <param name="name"></param>
    [Client]
    public void SetName(string name)
    {
        Debug.Log(gameObject.name + ", " + base.hasAuthority +"this is the PlayerName class");

        CmdSetName(name);

    }

    /// <summary>
    /// setts the name for this character
    /// </summary>
    /// <param name="name"></param>
    [Command]
    private void CmdSetName(string name)
    {
        syncronizedName = name;
    }

}