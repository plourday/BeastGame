using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Player : MonoBehaviour
{

   private Rigidbody2D rb; // 2D Rigid Body attached to player
   private Animator animator; // Animator Object attached to player
   private BoxCollider2D boxCollider; // 2d box collider attached to player
   private float movementSpeed = 3.0f; // player move speed
   private int jumpCount; // holds the number of jumps a player has
   private float wallJumpCooldown; // cooldown for wall jumping
   [SerializeField] private LayerMask groundLayer; // layer for the ground for reference 
   [SerializeField] private LayerMask wallLayer;// layer for the wall for reference
   [SerializeField] private float movement; // Movement vector for getting x/y player movement
   [SerializeField] private float jumpForce = 10f; // force to be applied to jump 

    enum CharStates // Holds the animation states that will be applied in the animator
    {
        idle = 0,
        walk = 1,
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    // Update is called once per frame all methods will be called every frame
    void Update()
    {
        Move();
        Scale_Adjust();
        Animate();
        if (wallJumpCooldown > 0.2f)
        {
            GetJumpCount();
            IsAttachedToWall();
            Jump();
        }
        else 
        {
            wallJumpCooldown += Time.deltaTime;
        }
    }

    //Adjust the rotation of the player based on if the left or right key is pressed so that animations will play based on direction 
    void Scale_Adjust()
    {
        if (Input.GetKeyDown("left"))
        {
            transform.localScale=new Vector3(-1,1,1);
        }
        else if (Input.GetKeyDown("right"))
        {
            transform.localScale =new Vector3(1,1,1);
        }
    }
    //Sets animation states based on the movement of the character
    void Animate()
    {
        animator.SetBool("isGrounded", IsGrounded());

        if (Mathf.Abs(rb.velocity.x) > 0)
        {
            animator.SetInteger("AnimationState", (int)CharStates.walk);
        }
        else
        {
            animator.SetInteger("AnimationState", (int)CharStates.idle);
        }

        //Debug.Log(animator.GetInteger("AnimationState"));
    }
    // getting input and applying it to the rigidbody to make the player move
    void Move()
    {
        movement = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(movement * movementSpeed, rb.velocity.y);
    }

    //When the player attempts a jump, sets the animation to trigger if the player is grounded
    void Jump()
    {
        //if the player is grounded, they can jump/double jump

        if (Input.GetButtonDown("Jump"))
        {
            if ( IsGrounded() || jumpCount < 1)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetTrigger("jump");
                jumpCount++;
            }
            else if (IsAttachedToWall())
            {
                wallJumpCooldown = 0;
                rb.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * 1, 3);
            }
        }
    }

    int GetJumpCount() 
    {
        if (IsGrounded())
        {
            jumpCount = 0;
            return jumpCount;
        }
        else if (IsAttachedToWall())
        {
            jumpCount = 0;
            return jumpCount;
        }
        else 
        {
            return jumpCount;
        }
    }

    //if player objects collides with another box collider, do something - based on Tags
    /*   void OnCollisionEnter2D(Collision2D collision)
       {
           if (collision.gameObject.tag == "Ground")
           {
               IsGrounded();
           }

           if (collision.gameObject.tag == "Wall") 
           {
               OnWall();        
           }
       }
      */

    private bool IsAttachedToWall() 
    {
        if (OnWall() && !IsGrounded())
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    private bool IsGrounded()
    {
        //creates a box to cast below the player to determine if they are on the ground or not
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool OnWall()
    {
        //creates a box to cast below the player to determine if they are near a wall or not
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x,0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

}
