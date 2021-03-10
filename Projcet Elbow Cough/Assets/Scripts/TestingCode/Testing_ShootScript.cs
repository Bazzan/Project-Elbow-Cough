using System;
using System.Collections;
using Mirror;
using Unity.Mathematics;
using UnityEngine;

public class Testing_ShootScript : NetworkBehaviour
{
    [SerializeField] private GameObject projectileVFX = null;

    [SerializeField] private float ShootCD;
    private float nextAttackTime = 0f;
    private NetworkAnimator networkAnimator = default;
    private Camera Camera;
    private int numberOfTImes;

    private float maxPositionOffset = 1f;

    private void Awake()
    {
        InputManager.testingShootScript = this;
        networkAnimator = GetComponent<NetworkAnimator>();
        Camera = Camera.main;
    }


    public void CheckAttack()
    {
        if (Time.time < nextAttackTime) return;

        // StartCoroutine(SpawnProjectile()); //fortsätt med positions spawn o network raycast
        nextAttackTime = Time.time + ShootCD;
        Debug.Log("shoot");
        StartCoroutine(SpawnProjectile(transform.position, Camera.transform.rotation));
        //set networkAnimation trigger
        CmdShoot(transform.position, Camera.transform.rotation);
    }
/// <summary>
 /// tell the server to fire projectile from owner
 /// </summary>
 /// <param name="position"></param>
 /// <param name="direction"></param>
    [Command]
    private void CmdShoot(Vector3 position, quaternion direction)
    {
        if (Time.time < nextAttackTime) return; // anti cd remove hack
        nextAttackTime = Time.time + ShootCD;

        //if position client used it too far from position on server the cap position
        if (Vector3.Distance(position, transform.position) > maxPositionOffset
        ) // simple anti cheat (shoting from thier position)
        {
            Vector3 posDirection = position - transform.position;
            position = transform.position + (posDirection * maxPositionOffset);
        }
        //spawn projectile on server
        StartCoroutine(SpawnProjectile(position, direction));
       //tell other clients to spawn projectile
        RpcShoot(position, direction);
        
    }

/// <summary>
/// received on clients to spawn projectile
/// </summary>
/// <param name="position"></param>
/// <param name="direction"></param>
    [ClientRpc]
    private void RpcShoot(Vector3 position, quaternion direction)
    {
        //already run localy
        if (base.hasAuthority) return;

        StartCoroutine(SpawnProjectile(position, direction));
    }


    private IEnumerator SpawnProjectile(Vector3 position, quaternion direction)
    {
        // Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        GameObject particle =
            Instantiate(projectileVFX, position, direction);
        numberOfTImes++;

        // Debug.Log(numberOfTImes);
        float endTime = Time.time + 0.9f;
        float moveSpeed = 1.5f;
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (Time.time < endTime)
        {
            // particle.transform.position += (direction * (moveSpeed * Time.deltaTime));
            yield return wait;
        }

        Destroy(particle);
    }
}