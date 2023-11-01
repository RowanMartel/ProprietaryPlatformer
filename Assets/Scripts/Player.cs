using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int speed;
    [SerializeField] int jumpForce;

    bool grounded;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Reset();
    }

    public void Reset()
    {
        rb.Sleep();
        transform.position = new Vector2(-8, FindObjectOfType<TerrainGen>().groundLevel / 4 - 3);
    }

    private void Update()
    {
        CheckGrounded();

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.Space) ||  Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
    }

    void MoveLeft()
    {
        transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);
        GetComponent<SpriteRenderer>().flipX = true;
    }
    void MoveRight()
    {
        transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
        GetComponent<SpriteRenderer>().flipX = false;
    }
    void Jump()
    {
        if (!grounded) return;

        rb.AddForce(new Vector2(0, jumpForce));
        grounded = false;
    }

    void CheckGrounded()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, .5f);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                grounded = true;
                return;
            }
            grounded = false;
        }
    }
}