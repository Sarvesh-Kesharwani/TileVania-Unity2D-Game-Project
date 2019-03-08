using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRidigbody;
    private Animator animator;
    private CapsuleCollider2D capsulecollider2d;
    private BoxCollider2D boxcollider;
    

    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpSpeed = 11f;
    private float climbSpeed = 4f;
    private float GravityScaleAtStart;
    private bool IsAlive = true;
    [SerializeField] private Vector2 DeathKick = new Vector2(0f,25f);
    void Start()
    {
        myRidigbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsulecollider2d = GetComponent<CapsuleCollider2D>();
        boxcollider = GetComponent<BoxCollider2D>();
        GravityScaleAtStart = myRidigbody.gravityScale;
    }

    void Update()
    {
        if (!IsAlive)
        {
            return;
        }
        Run();
        FlipSprite();
        Jump();
        ClimbLadder();
        Die();
    }


    private void ClimbLadder()
    {
        if (!capsulecollider2d.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            animator.SetBool("Climbing",false);
            myRidigbody.gravityScale = GravityScaleAtStart;
            return;
        }
        else
        {
            myRidigbody.gravityScale = 0f;
            myRidigbody.velocity = new Vector2(myRidigbody.velocity.x,CrossPlatformInputManager.GetAxis("Vertical") * climbSpeed);
            animator.SetBool("Climbing",Mathf.Abs(myRidigbody.velocity.y) > Mathf.Epsilon); 
            }
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal") * runSpeed,myRidigbody.velocity.y);
        myRidigbody.velocity = playerVelocity;

         animator.SetBool("Running",Mathf.Abs(playerVelocity.x) > Mathf.Epsilon);
    }

    private void FlipSprite()
    {
        if(Mathf.Abs(myRidigbody.velocity.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRidigbody.velocity.x),1);
        }
    }

    private void Jump()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump") && boxcollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myRidigbody.velocity += new Vector2(0f, jumpSpeed);
            animator.SetBool("Jump", true);
        }
    }

    private void Die()
    {
        if (capsulecollider2d.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            IsAlive = false;
            animator.SetTrigger("Die");
            GetComponent<Rigidbody2D>().velocity = DeathKick;
            capsulecollider2d.isTrigger = true;
        }
    }
}
