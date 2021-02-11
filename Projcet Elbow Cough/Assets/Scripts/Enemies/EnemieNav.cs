using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class EnemieNav : NetworkBehaviour
{
    private NavMeshAgent agent;
    private Transform playerTransform;
    private StaticRefrences staticRefrences;
    private void Start()
    {
        staticRefrences = FindObjectOfType<StaticRefrences>();
        agent = GetComponent<NavMeshAgent>();

        // StartCoroutine(FindPlayer());
    }

    // IEnumerator FindPlayer()
    // {
    //     
    //     
    //     while (playerTransform == null)
    //     {
    //         if (staticRefrences.ListOfPlayers[0] != null){
    //             playerTransform = staticRefrences.ListOfPlayers[0].transform;
    //         }
    //         yield return null;
    //     }
    // }

    private void Update()
    {
        
        
        if (playerTransform == null) return;
        agent.SetDestination(playerTransform.position);


    }
}
