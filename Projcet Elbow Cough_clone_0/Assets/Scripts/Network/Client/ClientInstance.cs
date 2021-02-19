using System;
using Mirror;
using Unity.Mathematics;
using UnityEngine;

public class ClientInstance : NetworkBehaviour
{
    public static ClientInstance Instance; //singelton that only exist on the client;

    /// <summary>
    /// called when a character is spawned for the player
    /// this cant be done on the server
    /// </summary>
    public static Action<GameObject> OnOwnerCharacterSpawned;


    [Tooltip("Prefab for the player")] [SerializeField]
    private NetworkIdentity playerPrefab = null;



    public void InvokeCharacterSpawned(GameObject gameObject)
    {
        OnOwnerCharacterSpawned?.Invoke(gameObject);
    }
    
    
    /// <summary>
    /// request a spawn for character.
    /// </summary>
    [Command]
    private void CmdRequestToSpawn()
    {
        SpawnPlayerOnNetwork();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        if (Instance != null)
            Instance = null;
        Instance = this;
        
        CmdRequestToSpawn();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        SpawnPlayerOnNetwork();
    }

    [Server]
    private void SpawnPlayerOnNetwork()
    {
        GameObject playerGameObject = Instantiate(playerPrefab.gameObject, transform.position, quaternion.identity);
        NetworkServer.Spawn(playerGameObject, base.connectionToClient);
    }

    /// <summary>
    /// if the server and connection is not null.
    /// when trying to access as a server connection it
    /// will always contain a value, but if trying
    /// to accesss as a client it will be null
    /// </summary>
    /// <param name="conn"></param>
    /// <returns></returns>
    public static ClientInstance ReturnClientInstance(NetworkConnection conn = null)
    {
        if (NetworkServer.active && conn != null)
        {
            NetworkIdentity localPlayer;
            if (MyNetworkManager.LocalPlayers.TryGetValue(conn, out localPlayer))
                return localPlayer.GetComponent<ClientInstance>();
            else
                return null;
        }
        else
            return Instance;
    }
}