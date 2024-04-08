using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool runBegun;
    [SerializeField] private float speed = 5f;
    [SerializeField] private int jumpForce = 5;
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (runBegun)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }

       CheckInput();
    }

    private void CheckInput()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            runBegun = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.y, jumpForce);
        }
    }
}
