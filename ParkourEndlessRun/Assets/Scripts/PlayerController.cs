using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Move variables")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private int jumpForce = 5;

    private bool playerUnlock;

    [Header("Collision variables")]
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerUnlock)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }

        CheckGround();
        CheckInput();
        CheckAnimation();
        
    }

    private void CheckAnimation()
    {
        anim.SetBool("isGround", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetFloat("xVelocity", rb.velocity.x);
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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.y, jumpForce);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x , transform.position.y - groundDistance));
    }
}
