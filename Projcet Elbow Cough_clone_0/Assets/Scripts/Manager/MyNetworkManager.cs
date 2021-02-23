using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkManager : Mirror.NetworkManager
{

    /// <summary>
    /// All spawned local players, only kept on the server.
    /// </summary>
    public static Dictionary<NetworkConnection, NetworkIdentity> LocalPlayers = new Dictionary<NetworkConnection, NetworkIdentity>();
    public static List<NetworkIdentity> LocalPlayersList = new List<NetworkIdentity>();
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server has started");
    }

/// <summary>
/// invoked when client is started
/// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("client started" + " " + numPlayers);
        
    }

    
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        Debug.Log(conn.identity + " has left the game");
    }

    
    /// <summary>
    /// same method, only added so that the player gets added to the dictionary LocalPlayers
    /// </summary>
    /// <param name="conn"></param>
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);
        
        LocalPlayers[conn] = player.GetComponent<NetworkIdentity>(); // only line added 
        LocalPlayersList.Add(player.GetComponent<NetworkIdentity>());
        NetworkServer.AddPlayerForConnection(conn, player);        
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        LocalPlayersList.Remove(LocalPlayers[conn]);
        LocalPlayers.Remove(conn);

    }
}
