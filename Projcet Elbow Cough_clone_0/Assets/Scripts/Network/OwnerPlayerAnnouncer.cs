using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Rendering;

public class OwnerPlayerAnnouncer : NetworkBehaviour
{
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        AnnouncePlayer();
    }

    private void AnnouncePlayer()
    {
        ClientInstance ci = ClientInstance.ReturnClientInstance();
        ci.InvokeCharacterSpawned(gameObject);
    }
}
