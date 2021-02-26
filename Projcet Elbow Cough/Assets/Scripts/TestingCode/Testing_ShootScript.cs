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
    private void Awake()
    {
        InputManager.testingShootScript = this;
        networkAnimator = GetComponent<NetworkAnimator>();
        Camera= Camera.main;
    }


    public void CheckAttack()
    {
        if (Time.time < nextAttackTime) return;

        // StartCoroutine(SpawnProjectile()); //fortsätt med positions spawn o network raycast
        nextAttackTime = Time.time + ShootCD;
        
        //set networkAnimation trigger
        
    }

    private void CmdShoot(Vector3 position, Vector3 direction)
    {
        
    }


    private IEnumerator SpawnProjectile(Vector3 position, Vector3 direction)
    {
        // Ray ray = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        GameObject particle =
            Instantiate(projectileVFX, position ,quaternion.Euler(direction) );
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