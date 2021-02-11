using System;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class EnemieNav : NetworkBehaviour
{
    private NavMeshAgent agent;
    private Transform playerTransform;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = StaticRefrences.PlayerTransform;

    }


    private void Update()
    {
        agent.SetDestination(playerTransform.position);


    }
}
