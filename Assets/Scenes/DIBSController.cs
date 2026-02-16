using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIBSController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    public float jumpVelocity;
    public float flyVelocity;
    public float timeFlying;
    public float runSpeed;
    public float climbSpeed;
    public float wallJump;
    public CameraFollow bS;

    bool facingRight;
    bool isGrounded;
    bool isFlying = false;
    bool wallTouchL = false;
    bool wallTouchR = false;
    bool wallTouchTopL;
    bool wallTouchTopR;
    bool onTheWallR = false;
    bool onTheWallL = false;
    public bool isCrouching = false;

    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    Transform groundCheckL;
    [SerializeField]
    Transform groundCheckR;
    [SerializeField]
    Transform wallCheckR;
    [SerializeField]
    Transform wallCheckL;
    [SerializeField]
    Transform wallCheckTopR;
    [SerializeField]
    Transform wallCheckTopL;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if ((Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"))) ||
          (Physics2D.Linecast(transform.position, groundCheckR.position, 1 << LayerMask.NameToLayer("Ground"))) ||
          (Physics2D.Linecast(transform.position, groundCheckL.position, 1 << LayerMask.NameToLayer("Ground"))))
        {
            isGrounded = true;
            timeFlying = 0;
        }
        else
        {
            isGrounded = false;
        }

        if ((Physics2D.Linecast(transform.position, wallCheckL.position, 1 << LayerMask.NameToLayer("Walls"))))
        {
            wallTouchL = true;
        }
        else
        {
            wallTouchL = false;
        }

        if ((Physics2D.Linecast(transform.position, wallCheckR.position, 1 << LayerMask.NameToLayer("Walls"))))
        {
            wallTouchR = true;
        }
        else
        {
            wallTouchR = false;
        }

        if ((Physics2D.Linecast(transform.position, wallCheckTopL.position, 1 << LayerMask.NameToLayer("Walls"))))
        {
            wallTouchTopL = true;
        }
        else
        {
            wallTouchTopL = false;
        }

        if ((Physics2D.Linecast(transform.position, wallCheckTopR.position, 1 << LayerMask.NameToLayer("Walls"))))
        {
            wallTouchTopR = true;
        }
        else
        {
            wallTouchTopR = false;
        }

        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            rb2d.linearVelocity = new Vector2(runSpeed, rb2d.linearVelocity.y);
            if (isGrounded)
                animator.Play("Player_walk");
            spriteRenderer.flipX = true;
            facingRight = true;
        }
        else if (Input.GetKey("a") || Input.GetKey("left"))
        {
            rb2d.linearVelocity = new Vector2(-runSpeed, rb2d.linearVelocity.y);
            if (isGrounded)
                animator.Play("Player_walk");
            spriteRenderer.flipX = false;
            facingRight = false;
        }

        else
        {

            rb2d.linearVelocity = new Vector2(0, rb2d.linearVelocity.y);

            if (Input.GetKey("down"))
            {
                isCrouching = true;
                animator.Play("Player_crouch");
            }

            else
            {
                isCrouching = false;
            }

            if (isGrounded && !isCrouching)
            {
                animator.Play("Player_idle");
            }

        }
        //jump
        if (Input.GetKey("up") && isGrounded)
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpVelocity);
            animator.Play("Player_jump");
        }
        //fly
        if (Input.GetKey("space") && !wallTouchR && !wallTouchL && timeFlying < 12)
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, flyVelocity);
            animator.Play("Player_fly");
            timeFlying++;
        }
        //climb right
        if (Input.GetKey("right") && wallTouchR)
        {
            rb2d.linearVelocity = new Vector2(0, climbSpeed);
            onTheWallR = true;
            wallTouchTopR = true;
            timeFlying = 0;
            animator.Play("Player_climb");
        }
        //climb left
        if (Input.GetKey("left") && wallTouchL)
        {
            rb2d.linearVelocity = new Vector2(0, climbSpeed);
            onTheWallL = true;
            wallTouchTopL = true;
            timeFlying = 0;
            spriteRenderer.flipX = true;
            animator.Play("Player_climb");
        }
        //walljumpIhope
        if (Input.GetKey("left") && onTheWallR)
        {
            spriteRenderer.flipX = true;
            animator.Play("Player_jump");
            onTheWallR = false;
            rb2d.linearVelocity = new Vector2(-wallJump, wallJump);
        }

        if (Input.GetKey("right") && onTheWallL)
        {
            spriteRenderer.flipX = true;
            animator.Play("Player_jump");
            onTheWallL = false;
            rb2d.linearVelocity = new Vector2(wallJump, wallJump);
        }
        //hoist left
        if (Input.GetKey("up") && onTheWallL && !wallTouchTopL)
        {
            animator.Play("Player_jump");
            rb2d.linearVelocity = new Vector2(0, wallJump);
            onTheWallL = false;
        }
        //hoist right
        if (Input.GetKey("up") && onTheWallR && !wallTouchTopR)
        {
            spriteRenderer.flipX = false;
            animator.Play("Player_jump");
            rb2d.linearVelocity = new Vector2(0, wallJump);
            onTheWallR = false;
        }
    }
}
