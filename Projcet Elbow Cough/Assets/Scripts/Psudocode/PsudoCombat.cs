using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PsudoCombat : NetworkBehaviour
{

    private float nextAttackTime = 0;

    private Animator animator;
    private NetworkAnimator networkAnimator;
    private void Awake()
    {
        //get networkAnimator;
    }

    private void Update()
    {
        if(!base.hasAuthority) return;
        CheckAttack();
        
    }


    private void CheckAttack()
    {
        if (Time.time < nextAttackTime) return;

        nextAttackTime = Time.time + 0.5f;
        
        networkAnimator.SetTrigger("Attack");

    }
}
