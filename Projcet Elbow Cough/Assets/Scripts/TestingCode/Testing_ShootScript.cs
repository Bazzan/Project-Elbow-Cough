using System.Collections;
using Mirror;
using UnityEngine;
//TODO: make clients spawn stuff
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
        if (!base.hasAuthority) return;
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + ShootCD;

        StartCoroutine(SpawnProjectile(transform.position, Camera.transform.rotation, Camera.transform.forward));
        //TODO: set networkAnimation trigger
        CmdShoot(transform.position, Camera.transform.rotation,Camera.transform.forward);
        
    }

    /// <summary>
    /// tell the server to fire projectile from owner
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    [Command]
    private void CmdShoot(Vector3 position, Quaternion direction, Vector3 vectorDirection)
    {
        // if (Time.time < nextAttackTime) return; // anti cd remove hack
        nextAttackTime = Time.time + ShootCD;

        // direction = direction.normalized;
        //if position client used it too far from position on server the cap position
        // simple anti cheat (shoting from thier position)
        if (Vector3.Distance(position, transform.position) > maxPositionOffset)
        {
            Vector3 posDirection = position - transform.position;
            position = transform.position + (posDirection * maxPositionOffset);
            Debug.Log("cmdShoot anti cheat");
        }

        // //spawn projectile on server
        //StartCoroutine(SpawnProjectile(position, direction)); //not needed?
        //tell other clients to spawn projectile
        RpcShoot(position, direction, vectorDirection);
    }


    /// <summary>
    /// received on clients to spawn projectile
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    [ClientRpc]
    private void RpcShoot(Vector3 position, Quaternion direction, Vector3 vectorDirection)
    {
        //already run locally on owner
        if (base.hasAuthority)
        {
            Debug.Log("RPCshoot, has authorty");
            return;
        }
        StartCoroutine(SpawnProjectile(position, direction, vectorDirection));
    }

    private void TraceForHits(Vector3 position, Vector3 vectorDirection)
    {
        Ray ray = new Ray(position, vectorDirection);
        RaycastHit hit;
        EnableColliders(false);
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            Debug.Log("hit " + hit.collider.name);
             if (base.isClient) //only instansiate if is a client
            Instantiate(hitVFX, hit.point, Quaternion.identity);
        }
        Debug.Log("traceForHits " + vectorDirection + "  " + vectorDirection.normalized);

        EnableColliders(true);
    }

    private void EnableColliders(bool active)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = active;
        }
    }

    private IEnumerator SpawnProjectile(Vector3 position, Quaternion direction, Vector3 vectorDirection)
    {
        GameObject particle;
        TraceForHits(position, vectorDirection);
        if (base.isClient)
            particle = Instantiate(projectileVFX, position, direction);
        // NetworkServer.Spawn(particle);
        Debug.Log(direction);
// base.is
        yield return null;

        // float endTime = Time.time + 10f;
        // float moveSpeed = 1f;
        // WaitForEndOfFrame wait = new WaitForEndOfFrame();
        //
        // while (Time.time < endTime)
        // {
        //     // particle.transform.position += (direction.eulerAngles * (moveSpeed * Time.deltaTime));
        //     yield return wait;
        // }
    }
}