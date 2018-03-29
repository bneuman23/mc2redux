using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;
    CharacterStats charStats;

    public float stopDistance = 1;
    public bool moveToPosition;
    public Vector3 destPosition;

    public bool run;
    public bool crouch;

    public float walkSpeed = 1;
    public float runSpeed = 2;
    public float crouchSpeed = 0.8f;

    public float maxStance = 0.9f;
    public float minStance = 0.1f;
    float targetStance;
    float stance;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        //SetupAnimator();
        agent = GetComponent<NavMeshAgent>();
        charStats = GetComponent<CharacterStats>();
        agent.stoppingDistance = stopDistance - 0.1f;

        agent.updateRotation = true;

        //InitRagdoll();
	}

    // Update is called once per frame
    void Update ()
    {
        run = charStats.run;

        if (moveToPosition)
        {
            agent.SetDestination(destPosition);

            float distanceToTarget = Vector3.Distance(transform.position, destPosition);

            if(distanceToTarget <= stopDistance)
            {
                moveToPosition = false;
                charStats.run = false;
            }
        }

        HandleSpeed();
        //HandleAnimation();
        HandleStates();
	}

    void HandleStates()
    {
        if (charStats.run)
        {
            targetStance = minStance;
        }
        else
        {
            if (charStats.crouch)
            {
                targetStance = maxStance;
            }
            else
            {
                targetStance = minStance;
            }
        }

        stance = Mathf.Lerp(stance, targetStance, Time.deltaTime * 3);
        //anim.SetFloat("Stance", stance);
    }

    void HandleSpeed()
    {
        if (!run)
        {
            if(!crouch)
            {
                agent.speed = walkSpeed;
            }
            else
            {
                agent.speed = crouchSpeed;
            }
        }
        else
        {
            agent.speed = runSpeed;
        }
    }

    void HandleAnimation()
    {
        Vector3 relativeDirection = (transform.InverseTransformDirection(agent.desiredVelocity)).normalized;
        float animValue = relativeDirection.z;

        if (!run)
        {
            animValue = Mathf.Clamp(animValue, 0, 0.5f);
        }

        anim.SetFloat("Forward", animValue, 0.3f, Time.deltaTime);
    }
    
    void SetupAnimator()
    {
        //This is a reference to the animator component on the root.
        anim = GetComponent<Animator>();

        //We use avatar from a child animator component if present.
        //This is to enable easy swapping of the character model as a child node.
        foreach (var childAnimator in GetComponentsInChildren<Animator>())
        {
            if(childAnimator != anim)
            {
                anim.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break; //if you find the first animator, stop searching.
            }
        }
    }

    void InitRagdoll()
    {
        Rigidbody[] rigB = GetComponentsInChildren<Rigidbody>();
        Collider[] cols = GetComponentsInChildren<Collider>();

        for(int i = 0; i < rigB.Length; i++)
        {
            rigB[i].isKinematic = true;
        }
        
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].isTrigger = true;
        }
    }
}
