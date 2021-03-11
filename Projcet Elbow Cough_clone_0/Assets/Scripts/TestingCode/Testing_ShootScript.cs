using System;
using System.Collections;
using System.Threading;
using Mirror;
using UnityEngine;

public class Testing_ShootScript : NetworkBehaviour
{
    [SerializeField] private GameObject projectileVFX = null;
    [SerializeField] private GameObject hitVFX = null;

    [SerializeField] private float ShootCD;
    private float nextAttackTime = 0f;
    private NetworkAnimator networkAnimator = default;
    private Camera Camera;
    private int numberOfTImes;

    private float maxPositionOffset = 1f;
    private Collider[] colliders;
    private void Awake()
    {
        // InputManager.testingShootScript = this;
        networkAnimator = GetComponent<NetworkAnimator>();
        Camera = Camera.main;
        colliders = GetComponentsInChildren<Collider>();

    }

//TODO: working on clients not over network
    public void CheckAttack()
    {
        if (Time.time < nextAttackTime) return;

        // StartCoroutine(SpawnProjectile()); //fortsätt med positions spawn o network raycast
        nextAttackTime = Time.time + ShootCD;
        Debug.Log("shoot");

        StartCoroutine(SpawnProjectile(transform.position, Camera.transform.rotation));
        
        //set networkAnimation trigger
        CmdShoot(transform.position, Camera.transform.forward);
    }

    /// <summary>
    /// tell the server to fire projectile from owner
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    [Command]
    private void CmdShoot(Vector3 position, Vector3 direction)
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

        // //spawn projectile on server
        StartCoroutine(SpawnProjectile(transform.position, Camera.transform.rotation)); //not needed?
        //tell other clients to spawn projectile
        RpcShoot(position, direction);
    }


    /// <summary>
    /// received on clients to spawn projectile
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    [ClientRpc]
    private void RpcShoot(Vector3 position, Vector3 direction)
    {
        //already run localy
        if (base.hasAuthority) return;
        StartCoroutine(SpawnProjectile(position,Quaternion.Euler(direction)) );
    }

    private void TraceForHits(Vector3 position, Vector3 direction)
    {
        Ray ray = new Ray(position, direction.normalized);
        RaycastHit hit;
        
        EnableColliders(false);
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            Debug.Log("hit " + hit.collider.name);
            if (base.isClient) //only instansiate if is a client
                Instantiate(hitVFX, hit.point, Quaternion.identity);
        }

        EnableColliders(true);
    }

    private void EnableColliders(bool active)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = active;
        }
    }
    
    private IEnumerator SpawnProjectile(Vector3 position, Quaternion direction)
    {
        TraceForHits(position, direction * Vector3.forward);

        if (base.isClient) // run vfx on clients only
        {
            // Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            GameObject particle =
                Instantiate(projectileVFX, position,direction);
            Debug.Log(direction.eulerAngles.normalized);
            // Debug.Log(Quaternion.Euler(direction).normalized);

            // Debug.Log(numberOfTImes);
            float endTime = Time.time + 10f;
            float moveSpeed = 1f;
            WaitForEndOfFrame wait = new WaitForEndOfFrame();

            while (Time.time < endTime)
            {
                // particle.transform.position += (direction.eulerAngles * (moveSpeed * Time.deltaTime));
                yield return wait;
            }

        }
    }
}