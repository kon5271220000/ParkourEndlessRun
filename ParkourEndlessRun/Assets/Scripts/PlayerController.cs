using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Speed info")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float mileStoneIncrease;
    [SerializeField] private float speedMileStone;
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float defaultMileStone;

    [Header("Move variables")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private int jumpForce = 5;
    [SerializeField] private int doubleJumpForce;

    [Header("Slide Variables")]
    private bool isSliding;
    [SerializeField] private float slideCooldownCounter;
    [SerializeField] private float slideCooldown;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float timeSlideCounter;
    [SerializeField] private float timeSlide;

    [Header("Ledge Climb Variables")]
    public Transform ledgeDetect;
    private bool isTouchingLedge;
    [SerializeField] private float ledgeCheckDistance;
    private bool ledgeDetected;
    private bool canClimbLedge;
    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;
    [SerializeField] private float ledgeOffsetX1;
    [SerializeField] private float ledgeOffsetX2;
    [SerializeField] private float ledgeOffsetY1;
    [SerializeField] private float ledgeOffsetY2;


    private bool playerUnlock;
    private bool canDoubleJump;

    [Header("Collision variables")]
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    private bool wallDetected;
    private bool ceilingDetected;
    //[HideInInspector] public bool ledgeDetected;

    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private float ceilingDistance;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        speedMileStone = mileStoneIncrease;
        defaultSpeed = speed;
        defaultMileStone = speedMileStone;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            canDoubleJump = true;
        }

        if (playerUnlock && !wallDetected)
        {
            HorizontalMovement();
        }

        SpeedControl();
        CheckCeiling();
        CheckGround();
        CheckWall();
        CheckInput();
        CheckAnimation();
        CheckSliding();
        CheckLedge();
        //LedgeClimb();

        

    }

    private void SpeedReset()
    {
        speed = defaultSpeed;
        speedMileStone = defaultMileStone;
    }
    private void SpeedControl()
    {
        if(transform.position.x > speedMileStone)
        {
            speedMileStone = speedMileStone + mileStoneIncrease;
            speed *= speedMultiplier;
            mileStoneIncrease *= speedMultiplier;

            if(speed > maxSpeed)
            {
                speed = maxSpeed;
            }
        }
    }
    
    private void CheckLedgeClimb()
    {
        if(ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + ledgeCheckDistance) - ledgeOffsetX1, Mathf.Floor(ledgePosBot.y + ledgeOffsetY1));
            ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + ledgeCheckDistance) + ledgeOffsetX2, Mathf.Floor(ledgePosBot.y + ledgeOffsetY2));

        }

        if (canClimbLedge)
        {
            transform.position = ledgePos1;
        }
    }

    private void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        ledgeDetected = false;
    }

    private void CheckSliding()
    {
        if(timeSlideCounter < 0 && !ceilingDetected)
        {
            isSliding = false;
        }
        
        timeSlideCounter -= Time.deltaTime;     
        slideCooldownCounter -= Time.deltaTime;
    }
    
    private void HorizontalMovement()
    {
        if (wallDetected)
        {
            SpeedReset();
        }

        if (isSliding)
        {
            rb.velocity = new Vector2(slideSpeed, rb.velocity.y);
        }
        else 
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    private void SlideCheck()
    {
        isSliding = true;
        timeSlideCounter = timeSlide;
        slideCooldownCounter = slideCooldown;
    }
    private void CheckWall()
    {
        wallDetected = Physics2D.BoxCast(wallCheck.transform.position, wallCheckSize, 0, Vector2.zero,0, whatIsGround);
    }

    private void CheckCeiling()
    {
        ceilingDetected = Physics2D.Raycast(ceilingCheck.transform.position, Vector2.up, ceilingDistance, whatIsGround);
    }

    private void CheckLedge()
    {
        isTouchingLedge = Physics2D.Raycast(ledgeDetect.transform.position, Vector2.right, ledgeCheckDistance, whatIsGround);

        if(wallDetected && isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.transform.position;
        }
    }

    private void CheckAnimation()
    {
        anim.SetBool("isSlide", isSliding);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isGround", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetBool("canClimb", canClimbLedge);
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, whatIsGround);
    }

    private void CheckInput()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            playerUnlock = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && slideCooldownCounter < 0)
        {
            SlideCheck();
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if(canDoubleJump)
        {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        }
        
    }

    /*private void LedgeClimb()
    {
        if (ledgeDetected && canGrab)
        {
            canGrab = false;
            
            Vector2 ledgePosition = GetComponentInChildren<LedgeDetect>().transform.position;

            climbBegunPosition = ledgePosition + offSet1;
            climbOverPosition = ledgePosition + offSet2;

            canClimb = true;
        }

        if (canClimb)
        {
            transform.position = climbBegunPosition;
        }
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x , transform.position.y - groundDistance));
        Gizmos.DrawLine(ceilingCheck.position, new Vector2(transform.position.x, transform.position.y + ceilingDistance));
        Gizmos.DrawLine(ledgeDetect.position, new Vector2(ledgeDetect.position.x  + ledgeCheckDistance, ledgeDetect.position.y));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
