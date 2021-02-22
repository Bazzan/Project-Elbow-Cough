using System;
using System.Collections;
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
        
        
        // playerTransform = NetworkManager
        StartCoroutine(GetTarget());
    }

    private void GetClosestPlayer()
    {
        float shortest = float.MaxValue;
        for (int i = 0; i < MyNetworkManager.LocalPlayers.Count; i++)
        {

            Transform t = MyNetworkManager.LocalPlayersList[i].transform;
            if (Vector3.Distance(t.position, transform.position) < shortest)
            {
                playerTransform = t;
            }
        }   
    }

    private IEnumerator GetTarget()
    {
        GetClosestPlayer();
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(GetTarget());

    }
    private void Update()
    {
        agent.SetDestination(playerTransform.position);
    
    
    }
}
